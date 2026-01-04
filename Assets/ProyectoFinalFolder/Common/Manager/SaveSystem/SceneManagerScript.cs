#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProyectoFinalFolder.Common.Manager.SaveSystem
{
    public class SceneManagerScript : MonoBehaviour
    {
#if UNITY_EDITOR
        [Header("Scene Assets (solo Editor)")]
        public List<SceneAsset> sceneAssets = new List<SceneAsset>();
        
        private void OnValidate()
        {
            sceneNames.Clear();
            foreach (var scene in sceneAssets)
            {
                if (scene != null)
                {
                    string path = AssetDatabase.GetAssetPath(scene);
                    string name = System.IO.Path.GetFileNameWithoutExtension(path);
                    sceneNames.Add(name);
                }
            }
            
            if (!Application.isPlaying)
            {
                UnityEditor.EditorUtility.SetDirty(this);
            }
        }
#endif

        [Header("Scene Names (build ready)")]
        [Tooltip("Estos nombres deben coincidir con los nombres exactos de las escenas en Build Settings.")]
        public List<string> sceneNames = new List<string>();

        public string currentScene;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            SetCurrentScene(SceneManager.GetActiveScene().name);
            SoundManagerScript.Instance.PlayMusic(currentScene == "MainTitle" ? "MainTheme" : "ForestTheme");
        }

        public void SetCurrentScene(string sceneName)
        {
            currentScene = sceneName;
        }

        public void LoadSceneByName(string sceneName)
        {
            if (sceneNames.Contains(sceneName))
            {
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                Debug.LogWarning($"La escena '{sceneName}' no está registrada.");
            }
        }

        public void LoadSceneByIndex(int index)
        {
            if (index >= 0 && index < sceneNames.Count)
            {
                LoadSceneByName(sceneNames[index]);
            }
            else
            {
                Debug.LogWarning("Índice de escena fuera de rango.");
            }
        }

        public void ExitScene()
        {
            Application.Quit();
        }
        
    }
}
