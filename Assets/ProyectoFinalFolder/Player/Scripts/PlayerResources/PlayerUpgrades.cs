using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace ProyectoFinalFolder.Player.Scripts.PlayerResources
{
    public class PlayerUpgrades : MonoBehaviour
    {
        public Dictionary<string, Upgrade<float>> floatUpgrades;
        public Dictionary<string, Upgrade<int>> intUpgrades;

        private void Awake()
        {
            floatUpgrades = new Dictionary<string, Upgrade<float>>
            {
                { "eBallDamage_1", new Upgrade<float>(10f) },
                { "eBallDamage_2", new Upgrade<float>(20f) },
                { "eBallDamage_3", new Upgrade<float>(30f) },

                { "eBallWeaknessCrit_1", new Upgrade<float>(3f) },
                { "eBallWeaknessCrit_2", new Upgrade<float>(4f) },
                { "eBallWeaknessCrit_3", new Upgrade<float>(5f) },

                { "eBallAttackSpeedMultiplier_1", new Upgrade<float>(1.2f) },
                { "eBallAttackSpeedMultiplier_2", new Upgrade<float>(1.5f) },
                { "eBallAttackSpeedMultiplier_3", new Upgrade<float>(2f) },

                { "eShieldConsumption_1", new Upgrade<float>(0.185f) },
                { "eShieldConsumption_2", new Upgrade<float>(0.15f) },
                { "eShieldConsumption_3", new Upgrade<float>(0.12f) },
                
                { "healthUpgrade_1", new Upgrade<float>(130f) },
                { "healthUpgrade_2", new Upgrade<float>(160f) },
                { "healthUpgrade_3", new Upgrade<float>(200f) },

                { "manaUpgrade_1", new Upgrade<float>(120f) },
                { "manaUpgrade_2", new Upgrade<float>(150f) },
                { "manaUpgrade_3", new Upgrade<float>(180f) },

                { "healthPotionRestore_1", new Upgrade<float>(60f) },
                { "healthPotionRestore_2", new Upgrade<float>(80f) },
                { "healthPotionRestore_3", new Upgrade<float>(120f) },

                { "manaOrbRestore_1", new Upgrade<float>(30f) },
                { "manaOrbRestore_2", new Upgrade<float>(40f) }
            };

            intUpgrades = new Dictionary<string, Upgrade<int>>
            {
                { "manaOrbsSpawn_1", new Upgrade<int>(3) },
                { "manaOrbsSpawn_2", new Upgrade<int>(4) },

                { "healthPotionsUses_1", new Upgrade<int>(4) },
                { "healthPotionsUses_2", new Upgrade<int>(5) },
                { "healthPotionsUses_3", new Upgrade<int>(6) }
            };
        }
        
        public bool TryGetFloat(string key, out float value)
        {
            if (floatUpgrades.TryGetValue(key, out var upgrade) && upgrade.unlocked)
            {
                value = upgrade.value;
                return true;
            }

            value = 0;
            return false;
        }
        
        public bool TryGetInt(string key, out int value)
        {
            if (intUpgrades.TryGetValue(key, out var upgrade) && upgrade.unlocked)
            {
                value = upgrade.value;
                return true;
            }

            value = 0;
            return false;
        }
        
        public bool TryGetBestFloatUpgrade(string upgradePrefix, out float bestValue)
        {
            bestValue = 0f;
            float maxFound = float.MinValue;
            bool found = false;

            foreach (var pair in floatUpgrades)
            {
                if (pair.Key.StartsWith(upgradePrefix) && pair.Value.unlocked)
                {
                    if (pair.Value.value > maxFound)
                    {
                        maxFound = pair.Value.value;
                        bestValue = pair.Value.value;
                        found = true;
                    }
                }
            }

            return found;
        }

        public bool TryGetBestIntUpgrade(string upgradePrefix, out int bestValue)
        {
            bestValue = 0;
            int maxFound = int.MinValue;
            bool found = false;

            foreach (var pair in intUpgrades)
            {
                if (pair.Key.StartsWith(upgradePrefix) && pair.Value.unlocked)
                {
                    if (pair.Value.value > maxFound)
                    {
                        maxFound = pair.Value.value;
                        bestValue = pair.Value.value;
                        found = true;
                    }
                }
            }
            return found;
        }

        public void Save(ref PlayerUpgradeSaveData data)
        {
            data.floatPlayerUpgrades = floatUpgrades;
            data.intPlayerUpgrades = intUpgrades;
        }
        
        public void Load(PlayerUpgradeSaveData data)
        {
            floatUpgrades = data.floatPlayerUpgrades;
            intUpgrades = data.intPlayerUpgrades;
        }
    }
    
    
    [System.Serializable]
    public struct PlayerUpgradeSaveData
    {
        [JsonProperty] public Dictionary<string, Upgrade<float>> floatPlayerUpgrades;
        [JsonProperty] public Dictionary<string, Upgrade<int>> intPlayerUpgrades;
    }
}