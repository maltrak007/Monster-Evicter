using System;
using UnityEngine;

namespace ProyectoFinalFolder.Components.Collectible.CollectibleCommon
{
    public class RotateScript : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 30f;
        [SerializeField] private Vector3 rotationAxis = Vector3.up;

        private void Update()
        {
            Rotate();
        }

        private void Rotate()
        {
            transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
        }
    }
}