using System;
using UnityEngine;

namespace ProyectoFinalFolder.Player.Scripts
{
    public class PlayerInteractProxy : MonoBehaviour
    {
        [SerializeField] private PlayerInteractScript playerInteractScript;

        private void OnTriggerEnter(Collider other)
        {
            playerInteractScript.OnTriggerEnterProxy(other);
        }

        private void OnTriggerStay(Collider other)
        {
            playerInteractScript.OnTriggerStayProxy(other);
        }

        private void OnTriggerExit(Collider other)
        {
            playerInteractScript.OnTriggerExitProxy(other);
        }
    }
}