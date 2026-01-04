using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using ProyectoFinalFolder.Common.Manager.LevelManager;
using ProyectoFinalFolder.Player.Scripts.PlayerInventory;
using ProyectoFinalFolder.Player.Scripts.PlayerResources;
using UnityEngine;

namespace ProyectoFinalFolder.Common.Manager.SaveSystem
{
    public class SaveSystem
    {
        private static SaveData _saveData = new SaveData();

        [System.Serializable]
        public struct SaveData
        {
            public PlayerUpgradeSaveData playerUpgradeSaveData;
            public PlayerInventorySaveData playerInventorySaveData;
            public PlayerMiscData playerMiscelaneousData;
            public MonsterSaveData monsterSaveData;
        }

        public static string SaveFileName()
        {
            string saveFile = Application.persistentDataPath + "/save" + ".save";
            return saveFile;
        }

        public static void Save()
        {
            HandleSaveData();
            File.WriteAllText(SaveFileName(), JsonConvert.SerializeObject(_saveData, Formatting.Indented));
        }

        private static void HandleSaveData()
        {
            GameManagerScript.Instance.playerUpgrades.Save(ref _saveData.playerUpgradeSaveData);
            GameManagerScript.Instance.playerInventory.Save(ref _saveData.playerInventorySaveData);
            MonsterLevelManager.Instance.Save(ref _saveData.monsterSaveData);
            GameManagerScript.Instance.Save(ref _saveData.playerMiscelaneousData);
        }

        public static void Load()
        {
            if (File.Exists(SaveFileName()))
            {
                string saveContent = File.ReadAllText(SaveFileName());
                _saveData = JsonConvert.DeserializeObject<SaveData>(saveContent);
                HandleLoadData();
            }
            else
            {
                Debug.LogWarning("No save file found. Starting with default data.");
                _saveData = new SaveData();
                MonsterLevelManager.Instance?.ResetDefeatedMonsters();
                Save();
            }
        }

        private static void HandleLoadData()
        {
            GameManagerScript.Instance.playerUpgrades.Load(_saveData.playerUpgradeSaveData);
            GameManagerScript.Instance.playerInventory.Load(_saveData.playerInventorySaveData);
            MonsterLevelManager.Instance.Load(_saveData.monsterSaveData);
            GameManagerScript.Instance.Load(_saveData.playerMiscelaneousData);
        }

        public static void DeleteSave()
        {
            if (File.Exists(SaveFileName()))
            {
                File.Delete(SaveFileName());
                Debug.Log("Save file deleted.");
            }
            else
            {
                Debug.LogWarning("No save file found to delete.");
            }
        }
    }
}