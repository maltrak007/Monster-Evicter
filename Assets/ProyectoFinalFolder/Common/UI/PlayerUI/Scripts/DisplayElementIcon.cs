using System;
using ProyectoFinalFolder.Common.Components.BaseComponent;
using ProyectoFinalFolder.Player.Scripts.PlayerResources;
using UnityEngine;
using UnityEngine.UI;

namespace ProyectoFinalFolder.Common.UI.PlayerUI
{
    public class DisplayElementIcon : MonoBehaviour
    {
        public PlayerSpells playerSpells;

        [SerializeField] private Image imageComponent;
        
        [Header("UI Icon Images")]
        [SerializeField] private Sprite fireIconImage;
        [SerializeField] private Sprite waterIconImage;
        [SerializeField] private Sprite electricIconImage;
        
        [Header("UI Icon Colors")]
        [SerializeField] private Color fireIconColor = Color.red;
        [SerializeField] private Color waterIconColor = Color.blue;
        [SerializeField] private Color electricIconColor = Color.magenta;
        
        
        public void Start()
        {
            imageComponent = GetComponent<Image>();
            playerSpells.onAffinityChanged.AddListener(HandleElementChange);
            
            HandleElementChange(playerSpells.currentAffinity);
        }
        
        private void OnDestroy()
        {
            playerSpells.onAffinityChanged.RemoveListener(HandleElementChange);
        }

        private void HandleElementChange(Affinities affinities)
        {
            switch (affinities)
            {
                case Affinities.Fire:
                    imageComponent.sprite = fireIconImage;
                    imageComponent.color = fireIconColor;
                    break;
                case Affinities.Water:
                    imageComponent.sprite = waterIconImage;
                    imageComponent.color = waterIconColor;
                    break;
                case Affinities.Electric:
                    imageComponent.sprite = electricIconImage;
                    imageComponent.color = electricIconColor;
                    break;
                default:
                    break;
            }
        }
    }
}