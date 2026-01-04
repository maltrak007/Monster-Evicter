using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using ProyectoFinalFolder.Enemies.CommonEnemy;
using UnityEngine;

namespace ProyectoFinalFolder.Common.Manager.LevelManager
{
    public class BossLevelManager : MonoBehaviour
    {
        public static BossLevelManager Instance { get; private set; }
        public HashSet<string> bossMonstersInLevel = new HashSet<string>();
        [SerializeField] private int bossMonsters;
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
            GameManagerScript.Instance.playerHealthComponent.onDeath.AddListener(ResetDefeatedBoss);
        }
        
        private void OnDestroy()
        {
            GameManagerScript.Instance.playerHealthComponent.onDeath.RemoveListener(ResetDefeatedBoss);
        }
        
        private void Update()
        {
            if (GetIfAllBossesAreDefeated())
            {
                GameManagerScript.Instance.SetGameState(GameState.Win);
            }
        }

        public void MarkBossAsDefeated(string id)
        {
            bossMonstersInLevel.Add(id);
        }
        
        public void ResetDefeatedBoss()
        {
            bossMonstersInLevel.Clear();
        }
        
        public bool GetIfAllBossesAreDefeated()
        {
            return bossMonsters == bossMonstersInLevel.Count;
        }
    }
}