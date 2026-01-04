using UnityEngine;

namespace ProyectoFinalFolder.Common.Components.BaseComponent
{
    public class AffinityComponent : MonoBehaviour
    {
        [Header("Affinity Settings")] 
        [SerializeField] private Affinities affinityWeakness;
        [SerializeField] private Affinities affinityResistance;
    
        public Affinities GetAffinityWeakness()
        {
            return affinityWeakness;
        }
    
        public Affinities GetAffinityResistance()
        {
            return affinityResistance;
        }
    }

    public enum Affinities
    {
        None,
        Fire,
        Water,
        Electric
    }
}