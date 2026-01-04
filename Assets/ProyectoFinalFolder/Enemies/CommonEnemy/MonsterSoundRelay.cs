using UnityEngine;

namespace ProyectoFinalFolder.Enemies.CommonEnemy
{
    public class MonsterSoundRelay : MonoBehaviour
    {
        public void PlaySFXFromAnimation(string soundName)
        {
            if (SoundManagerScript.Instance != null)
            {
                SoundManagerScript.Instance.PlaySound(soundName);
            }
        }
    }
}