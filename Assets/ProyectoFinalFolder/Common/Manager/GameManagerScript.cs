using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using ProyectoFinalFolder.Common.Manager.LevelManager;
using ProyectoFinalFolder.Common.Manager.SaveSystem;
using ProyectoFinalFolder.Player.Scripts.PlayerController;
using ProyectoFinalFolder.Player.Scripts.PlayerInventory;
using ProyectoFinalFolder.Player.Scripts.PlayerResources;
using UnityEngine;
using UnityEngine.Events;

namespace ProyectoFinalFolder.Common.Manager
{
    public enum GameState
    {
        InGame,
        Menu,
        Pause,
        GameOver,
        Win
    }

    public class GameManagerScript : MonoBehaviour
    {
        public static GameManagerScript Instance { get; private set; }
        public GameState CurrentState { get; private set; }

        [Header("Fill")] [SerializeField] public PlayerUpgrades playerUpgrades;

        [SerializeField] public PlayerInventoryScript playerInventory;

        [SerializeField] public PlayerLocomotionInput playerLocomotion;

        [SerializeField] public PlayerHealthComponent playerHealthComponent;

        [SerializeField] public Level currentLevel;
        
        public SceneManagerScript sceneManager;
        
        public Canvas pauseMenuCanvas;
        
        public Canvas winMenuCanvas;
        
        public Canvas deathMenuCanvas;

        public GameObject player;

        [Header("Do not fill")] public Vector3 currentPlayerRespawnPoint;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            SaveSystem.SaveSystem.Load();
            SetGameState(GameState.InGame);
            SetPlayerInPosition();
            player.GetComponent<PlayerSpells>().UpdatePlayerSpellParameters();
        }

        private void OnEnable()
        {
            playerHealthComponent.onDeath.AddListener(() => SetGameState(GameState.GameOver));
        }

        private void OnDestroy()
        {
            playerHealthComponent.onDeath.RemoveListener(() => SetGameState(GameState.GameOver));
        }

#if UNITY_EDITOR
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.L))
            {
                playerInventory.AddCoinAmount(10000);
            }
        }
#endif


        public void SetGameState(GameState gameState)
        {
            CurrentState = gameState;
            Cursor.lockState = CurrentState == GameState.InGame ? CursorLockMode.Locked : CursorLockMode.Confined;
            HandleCurrentGameState(CurrentState);
        }

        public void HandleCurrentGameState(GameState _currentGameState)
        {
            switch (_currentGameState)
            {
                case GameState.Menu:
                    playerLocomotion.PlayerInput.PlayerLocomotion.Disable();
                    playerLocomotion.PlayerInput.PlayerUI.Enable();
                    break;
                case GameState.InGame:
                    playerLocomotion.PlayerInput.PlayerUI.Disable();
                    playerLocomotion.PlayerInput.PlayerLocomotion.Enable();
                    Time.timeScale = 1f;
                    break;
                case GameState.Pause:
                    playerLocomotion.PlayerInput.PlayerLocomotion.Disable();
                    playerLocomotion.PlayerInput.PlayerUI.Enable();
                    pauseMenuCanvas.gameObject.SetActive(true);
                    Time.timeScale = 0f;
                    break;
                case GameState.GameOver:
                    playerLocomotion.PlayerInput.PlayerUI.Disable();
                    playerLocomotion.PlayerInput.PlayerLocomotion.Disable();
                    deathMenuCanvas.gameObject.SetActive(true);
                    StartCoroutine(ResetGame());
                    break;
                case GameState.Win:
                    playerLocomotion.PlayerInput.PlayerLocomotion.Disable();
                    playerLocomotion.PlayerInput.PlayerUI.Enable();
                    winMenuCanvas.gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
        }

        private void SetPlayerInPosition()
        {
            if (currentPlayerRespawnPoint == Vector3.zero) return;
            player.transform.position = currentPlayerRespawnPoint;
        }

        public void SaveGameExternally()
        {
            SaveSystem.SaveSystem.Save();
        }
        
        public void NewGamePlusSave()
        {
            SaveSystem.SaveSystem.DeleteSave();
            SetGameState(GameState.Menu);
            sceneManager.LoadSceneByIndex(0);
            playerInventory.AddCoinAmount(500000);
            SaveSystem.SaveSystem.Save();
        }
        
        public IEnumerator ResetGame()
        {
            yield return new WaitForSeconds(2.5f);
            SaveSystem.SaveSystem.Save();
            sceneManager.LoadSceneByIndex(1);
        }
        
        public void Save(ref PlayerMiscData data)
        {
            data.currentplayerRespawnPointData = currentPlayerRespawnPoint;
        }

        public void Load(PlayerMiscData data)
        {
            currentPlayerRespawnPoint = data.currentplayerRespawnPointData;
        }
    }

    [System.Serializable]
    public struct PlayerMiscData
    {
        [JsonProperty] public SerializableVector3 currentplayerRespawnPointData;
    }
}