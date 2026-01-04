using UnityEngine;

namespace ProyectoFinalFolder.Player.Scripts.PlayerResources
{
    [System.Serializable]
    public class Upgrade<T>
    {
        public T value;
        public bool unlocked;

        public Upgrade(T value, bool unlocked = false)
        {
            this.value = value;
            this.unlocked = unlocked;
        }
    }
}