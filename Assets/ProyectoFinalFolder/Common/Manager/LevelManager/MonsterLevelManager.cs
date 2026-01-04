using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace ProyectoFinalFolder.Common.Manager.LevelManager
{
    public class MonsterLevelManager : MonoBehaviour
    {
        public static MonsterLevelManager Instance { get; private set; }
        private HashSet<string> defeatedMonsters = new HashSet<string>();
        public int normalMonstersDefeated;
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
            normalMonstersDefeated = defeatedMonsters.Count;
        }

        public void MarkMonsterAsDefeated(string id)
        {
            defeatedMonsters.Add(id);
            normalMonstersDefeated++;
        }
        
        public void MarkMonsterAsNotDefeated(string id)
        {
            defeatedMonsters.Remove(id);
        }
        
        public bool IsMonsterDefeated(string id)
        {
            return defeatedMonsters.Contains(id);
        }
        
        public void ResetDefeatedMonsters()
        {
            defeatedMonsters.Clear();
        }
        
        public void Save(ref MonsterSaveData data)
        {
            data.defeatedMonsterIDs = new List<string>(defeatedMonsters);
        }

        public void Load(MonsterSaveData data)
        {
            defeatedMonsters = new HashSet<string>(data.defeatedMonsterIDs ?? new List<string>());
        }
    }
    
    [System.Serializable]
    public struct MonsterSaveData
    {
        [JsonProperty] public List<string> defeatedMonsterIDs;
    }
}