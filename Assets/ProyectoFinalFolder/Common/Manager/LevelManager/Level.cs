using System;
using System.Collections;
using System.Collections.Generic;
using ProyectoFinalFolder.Enemies.CommonEnemy;
using ProyectoFinalFolder.Player.Scripts.PlayerResources;
using UnityEngine;
using UnityEngine.Events;

namespace ProyectoFinalFolder.Common.Manager.LevelManager
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private List<Monster> monstersInLevel;
        [SerializeField] private int normalMonsters;
        private void Start()
        {
            if (monstersInLevel == null || monstersInLevel.Count == 0)
            {
                return;
            }
            GameManagerScript.Instance.playerHealthComponent.onDeath.AddListener(ResetEnemiesInCurrentLevel);
            StartCoroutine(DelayedMonsterCheck());
        }

        private void OnDestroy()
        {
            GameManagerScript.Instance.playerHealthComponent.onDeath.RemoveListener(ResetEnemiesInCurrentLevel);
        }

        private IEnumerator DelayedMonsterCheck()
        {
            yield return null;
            foreach (var enemy in monstersInLevel)
            {
                var id = enemy.monsterID;
                if (MonsterLevelManager.Instance.IsMonsterDefeated(id))
                {
                    enemy.gameObject.SetActive(false);
                }
            }
        }

        private void ResetEnemiesInCurrentLevel()
        {
            if (GetIfAllEnemiesAreDefeated()) return;
            
            bool anyEnemyReset = false;

            foreach (var enemy in monstersInLevel)
            {
                var id = enemy.monsterID;
                if (MonsterLevelManager.Instance.IsMonsterDefeated(id))
                {
                    enemy.gameObject.SetActive(true);
                    MonsterLevelManager.Instance.MarkMonsterAsNotDefeated(id);
                    anyEnemyReset = true;
                }
            }

            if (anyEnemyReset)
            {
                SaveSystem.SaveSystem.Save();
            }
        }

        public bool GetIfAllEnemiesAreDefeated()
        {
            return normalMonsters == MonsterLevelManager.Instance.normalMonstersDefeated;
        }
    }
}