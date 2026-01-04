using UnityEngine;

namespace ProyectoFinalFolder.Common.Components.BaseComponent
{
    public abstract class CollectibleComponent : MonoBehaviour
    {
        protected Collider pickUpCollider;
        
        protected abstract void CollectInteraction();
    }
}