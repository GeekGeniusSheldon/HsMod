using System;
using System.Reflection;
using UnityEngine;
using static HsMod.PluginConfig;

namespace HsMod
{
    // Local-only Battlegrounds pet. The controller is initialized without calling
    // PetControllerGame.CreatePetObject(), because that method sends a level override request.
    public static class BgPetSpoofer
    {
        private const string PetActorPrefabPath = "Card_Pet.prefab:42b6fce151aab234fbfbc0391c5bbe9d";
        private const float SpawnRetryDelay = 1.0f;
        private const BindingFlags InstanceMembers = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        private static readonly FieldInfo ActorField = typeof(PetControllerBoard).GetField("m_actor", InstanceMembers);
        private static readonly FieldInfo ModelManagerField = typeof(PetControllerBoard).GetField("m_modelManager", InstanceMembers);
        private static readonly FieldInfo LocationTagsField = typeof(PetControllerBoard).GetField("m_locationTags", InstanceMembers);
        private static readonly PropertyInfo PetObjectProperty = typeof(PetController).GetProperty("PetObject", InstanceMembers);
        private static readonly PropertyInfo DataHandlerProperty = typeof(PetControllerBoard).GetProperty("DataHandler", InstanceMembers);
        private static readonly PropertyInfo PetIdProperty = typeof(PetControllerBoard).GetProperty("PetId", InstanceMembers);
        private static readonly PropertyInfo PetVariantIdProperty = typeof(PetControllerBoard).GetProperty("PetVariantId", InstanceMembers);
        private static readonly PropertyInfo PlayerIdProperty = typeof(PetControllerBoard).GetProperty("PlayerId", InstanceMembers);
        private static readonly PropertyInfo IsOverridenProperty = typeof(PetControllerBoard).GetProperty("IsOverriden", InstanceMembers);
        private static readonly MethodInfo InitializeMethod = typeof(PetControllerGame).GetMethod("Initialize", InstanceMembers);
        private static readonly FieldInfo MouseDownField = typeof(PetInputManager).GetField("m_mouseDownOnPetInteractable", InstanceMembers);
        private static readonly FieldInfo MouseHoverField = typeof(PetInputManager).GetField("m_mouseHoverOnPetInteractable", InstanceMembers);

        private static GameObject spawnedContainer;
        private static GameObject spawnedModel;
        private static PetControllerGame spawnedController;
        private static int spawnedVariant = -1;
        private static float nextSpawnAttempt;

        public static void Init()
        {
            Reset();
        }

        public static void Shutdown()
        {
            Reset();
        }

        public static void Tick()
        {
            try
            {
                int variant = skinPet.Value;
                GameMgr gameMgr;
                GameState gameState;
                if (variant <= 0 || !CanRun(out gameMgr, out gameState))
                {
                    ResetIfSpawned();
                    return;
                }

                Player friendlyPlayer = gameState.GetPlayerBySide(Player.Side.FRIENDLY);
                if (friendlyPlayer == null)
                {
                    ResetIfSpawned();
                    return;
                }

                bool realPet = friendlyPlayer.GetPet() != null;
                bool registeredPet = HasServerPet(friendlyPlayer);
                if (realPet || registeredPet)
                {
                    if (spawnedContainer != null)
                    {
                        Utils.MyLogger(BepInEx.Logging.LogLevel.Warning,
                            $"[petspoof] real pet present (entity={realPet} manager={registeredPet}), removing local pet");
                    }
                    ResetIfSpawned();
                    return;
                }

                if (spawnedContainer != null && spawnedController != null && spawnedModel != null && spawnedVariant == variant)
                    return;

                if (spawnedContainer != null || spawnedController != null || spawnedModel != null)
                    Reset();

                if (Time.realtimeSinceStartup < nextSpawnAttempt)
                    return;

                ZoneCosmetic zone = ZoneMgr.Get()?.FindZoneOfType<ZoneCosmetic>(Player.Side.FRIENDLY);
                Transform petPosition = zone?.PetPosition;
                Actor ownerActor = friendlyPlayer.GetHero()?.GetCard()?.GetActor();
                if (petPosition == null || ownerActor == null || InputManager.Get()?.PetInputManager == null)
                {
                    DelayRetry();
                    return;
                }

                if (!TrySpawn(variant, friendlyPlayer, ownerActor, petPosition))
                {
                    DelayRetry();
                    return;
                }

                // This tag only updates the local pet-corner decoration.
                friendlyPlayer.SetTag(GAME_TAG.PET_VARIANT_ID, variant);
                Utils.MyLogger(BepInEx.Logging.LogLevel.Warning,
                    $"[petspoof] spawned interactive local BG pet variant {variant}");
            }
            catch (Exception ex)
            {
                Utils.MyLogger(BepInEx.Logging.LogLevel.Error, Unwrap(ex));
                ResetIfSpawned();
                DelayRetry();
            }
        }

        private static bool CanRun(out GameMgr gameMgr, out GameState gameState)
        {
            gameMgr = GameMgr.Get();
            gameState = GameState.Get();

            if (!isPluginEnable.Value || !isBgsUnlockCollectionEnable.Value)
                return false;
            if (gameMgr == null || gameState == null || !gameMgr.IsBattlegrounds() || gameMgr.IsSpectator())
                return false;
            if (!gameState.IsGameCreated() || gameState.IsGameOver())
                return false;

            SceneMgr sceneMgr = SceneMgr.Get();
            if (sceneMgr == null || sceneMgr.GetMode() != SceneMgr.Mode.GAMEPLAY || sceneMgr.IsTransitioning())
                return false;

            GameEntity gameEntity = gameState.GetGameEntity();
            return gameEntity != null && !gameEntity.IsMulliganActiveRealTime();
        }

        private static bool HasServerPet(Player friendlyPlayer)
        {
            try
            {
                PetGameplayManager manager = PetGameplayManager.Get();
                return manager != null && manager.TryGetPet(friendlyPlayer.GetPlayerId(), out _);
            }
            catch
            {
                return false;
            }
        }

        private static bool TrySpawn(int variant, Player friendlyPlayer, Actor ownerActor, Transform petPosition)
        {
            GameObject container = null;
            GameObject model = null;
            PetControllerGame controller = null;

            try
            {
                ValidateReflectionContract();

                PetVariantDbfRecord record = GameDbf.PetVariant.GetRecord(variant);
                if (record == null)
                    throw new InvalidOperationException($"Pet variant {variant} is missing from GameDbf");

                string cardId = GameUtils.TranslateDbIdToCardId(record.CardId);
                PetDataHandlerGameplay handler = new PetDataHandlerGameplay();
                if (string.IsNullOrEmpty(cardId) || !handler.TryLoadData(cardId))
                    throw new InvalidOperationException($"Cannot load gameplay data for pet variant {variant}");

                string modelPath = handler.GetModelGameplayAssetPath();
                if (string.IsNullOrEmpty(modelPath))
                    throw new InvalidOperationException($"Pet variant {variant} has no gameplay model");

                container = AssetLoader.Get()?.InstantiatePrefab(PetActorPrefabPath, default(AssetLoadingOptions));
                if (container == null)
                    throw new InvalidOperationException("Cannot instantiate Card_Pet controller prefab");

                controller = container.GetComponentInChildren<PetControllerGame>(true);
                if (controller == null)
                    throw new InvalidOperationException("Card_Pet prefab has no PetControllerGame");

                container.name = $"HsMod_LocalBgPet_{variant}";
                container.transform.SetParent(petPosition, false);
                container.transform.localPosition = Vector3.zero;
                container.transform.localRotation = Quaternion.identity;
                container.transform.localScale = Vector3.one;

                Actor templateActor = container.GetComponentInChildren<Actor>(true);
                if (templateActor != null && templateActor != ownerActor)
                    templateActor.enabled = false;

                model = AssetLoader.Get()?.InstantiatePrefab(modelPath, default(AssetLoadingOptions));
                if (model == null)
                    throw new InvalidOperationException($"Cannot instantiate gameplay model for pet variant {variant}");

                model.transform.SetParent(controller.transform, false);
                model.transform.localPosition = Vector3.zero;
                model.transform.localRotation = Quaternion.identity;
                model.transform.localScale = Vector3.one;

                PetModelManager modelManager = model.GetComponentInChildren<PetModelManager>(true);
                if (modelManager == null)
                    throw new InvalidOperationException($"Pet variant {variant} has no PetModelManager");

                SetValue(ActorField, controller, ownerActor);
                SetValue(DataHandlerProperty, controller, handler);
                SetValue(PetIdProperty, controller, record.PetId);
                SetValue(PetVariantIdProperty, controller, variant);
                SetValue(PlayerIdProperty, controller, friendlyPlayer.GetPlayerId());
                SetValue(IsOverridenProperty, controller, true);
                SetValue(PetObjectProperty, controller, model);
                SetValue(ModelManagerField, controller, modelManager);

                modelManager.SetPetController(controller);
                modelManager.HeroModel?.SetVisible(true);
                modelManager.DummyModel?.SetVisible(false);

                // Calls only the protected initialization chain. Do not replace this with
                // CreatePetObject(): its PetControllerGame override sends a network request.
                InitializeMethod.Invoke(controller, null);

                PetLocationTags locationTags = modelManager.HeroModel?.GameObject?.GetComponent<PetLocationTags>();
                SetValue(LocationTagsField, controller, locationTags);

                EnsureInputRegistration(controller);
                if (controller.transform.GetComponentsInChildren<Collider>().Length == 0)
                    throw new InvalidOperationException($"Pet variant {variant} has no active click colliders");

                spawnedContainer = container;
                spawnedModel = model;
                spawnedController = controller;
                spawnedVariant = variant;
                nextSpawnAttempt = 0f;
                return true;
            }
            catch (Exception ex)
            {
                Utils.MyLogger(BepInEx.Logging.LogLevel.Error, Unwrap(ex));
                DestroyLocalPet(controller, container, model);
                return false;
            }
        }

        private static void EnsureInputRegistration(PetControllerGame controller)
        {
            PetInputManager input = InputManager.Get()?.PetInputManager;
            if (input == null)
                throw new InvalidOperationException("PetInputManager is not available");

            foreach (PetInputManager.PetInteractionValues values in input.InteractionValues)
            {
                if (values.PetController == controller)
                    return;
            }
            input.RegisterPet(controller);
        }

        private static void ResetIfSpawned()
        {
            if (spawnedContainer != null || spawnedController != null || spawnedModel != null)
                Reset();
        }

        private static void Reset()
        {
            PetControllerGame controller = spawnedController;
            GameObject container = spawnedContainer;
            GameObject model = spawnedModel;

            spawnedController = null;
            spawnedContainer = null;
            spawnedModel = null;
            spawnedVariant = -1;
            nextSpawnAttempt = 0f;

            DestroyLocalPet(controller, container, model);
        }

        private static void DestroyLocalPet(PetControllerGame controller, GameObject container, GameObject model)
        {
            try
            {
                PetInputManager input = InputManager.Get()?.PetInputManager;
                if (input != null && controller != null)
                {
                    ClearInputReference(input, MouseDownField, controller);
                    ClearInputReference(input, MouseHoverField, controller);
                    input.UnregisterPet(controller);
                }
            }
            catch (Exception ex)
            {
                Utils.MyLogger(BepInEx.Logging.LogLevel.Error, Unwrap(ex));
            }

            try
            {
                if (container != null)
                    UnityEngine.Object.Destroy(container);
                else if (model != null)
                    UnityEngine.Object.Destroy(model);
            }
            catch (Exception ex)
            {
                Utils.MyLogger(BepInEx.Logging.LogLevel.Error, Unwrap(ex));
            }
        }

        private static void ClearInputReference(PetInputManager input, FieldInfo field, PetControllerGame controller)
        {
            if (field != null && ReferenceEquals(field.GetValue(input), controller))
                field.SetValue(input, null);
        }

        private static void DelayRetry()
        {
            nextSpawnAttempt = Time.realtimeSinceStartup + SpawnRetryDelay;
        }

        private static void ValidateReflectionContract()
        {
            if (ActorField == null || ModelManagerField == null || LocationTagsField == null || PetObjectProperty == null ||
                DataHandlerProperty == null || PetIdProperty == null || PetVariantIdProperty == null ||
                PlayerIdProperty == null || IsOverridenProperty == null || InitializeMethod == null)
            {
                throw new MissingMemberException("Hearthstone pet controller contract has changed");
            }
        }

        private static void SetValue(FieldInfo field, object target, object value)
        {
            field.SetValue(target, value);
        }

        private static void SetValue(PropertyInfo property, object target, object value)
        {
            property.SetValue(target, value, null);
        }

        private static Exception Unwrap(Exception exception)
        {
            return exception is TargetInvocationException invocation && invocation.InnerException != null
                ? invocation.InnerException
                : exception;
        }
    }
}
