using ProyectoFinalFolder.Player.Scripts.MiscScripts;
using UnityEngine;
using TMPro;

namespace ProyectoFinalFolder.Common.UI.PlayerUI
{
    public class DisplayPotionsScript : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI potionText;
        public PlayerHealthPotion playerHealthPotion;
        
        public void Start()
        {
            potionText = GetComponent<TextMeshProUGUI>();
            
            playerHealthPotion.onPotionChanged.AddListener(UpdatePotionText);
            UpdatePotionText();
        }


        private void OnDestroy()
        {
            playerHealthPotion.onPotionChanged.RemoveListener(UpdatePotionText);
        }
        
        public void UpdatePotionText()
        {
            potionText.text = playerHealthPotion.currentPotionUses.ToString();
        }
    }
}
