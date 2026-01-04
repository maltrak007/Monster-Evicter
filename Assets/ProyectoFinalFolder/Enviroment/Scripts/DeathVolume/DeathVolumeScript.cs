using System;
using ProyectoFinalFolder.Common.Manager;
using UnityEngine;

public class DeathVolumeScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManagerScript.Instance.playerHealthComponent.TakeDamage(3000);
        }
    }
}
