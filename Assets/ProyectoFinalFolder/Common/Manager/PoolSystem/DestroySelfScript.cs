using System;
using System.Collections;
using UnityEngine;

namespace ProyectoFinalFolder.Components.PoolSystem
{
    public class DestroySelfScript : MonoBehaviour
    {
        [SerializeField] private float destroyTime = 3f;

        private void OnEnable()
        {
            StartCoroutine(DestroyCoroutine());
        }

        private IEnumerator DestroyCoroutine()
        {
            yield return new WaitForSeconds(destroyTime);
            ObjectPoolManager.ReturnToPool(gameObject);
        }
    }
}