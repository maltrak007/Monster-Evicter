using ProyectoFinalFolder.Common.Components.BaseComponent;
using ProyectoFinalFolder.Components;
using ProyectoFinalFolder.Enemies.CommonEnemy;
using UnityEngine;
using UnityEngine.Events;

namespace ProyectoFinalFolder.Enemies.EnemyComponents
{
    public class EnemyInteractableComponent : InteractableComponent
    {
        public bool hasPlayerEvicted = false;
        
        public bool GetPlayerEvicted()
        {
            return hasPlayerEvicted;
        }
        
        public void SetPlayerEvicted(bool value)
        {
            hasPlayerEvicted = value;
        }

        public override void Interact()
        {
            if (canInteract && hasPlayerEvicted)
            {
                base.Interact();
                DeactivateCanvas();
            }
        }
        
        public override bool CheckConditionToActivateCanvas()
        {
            return HasCanvas() && gameObject.GetComponentInParent<Monster>().isStunned && !hasPlayerEvicted;
        }
    }
}