using ProyectoFinalFolder.Components.PoolSystem;
using UnityEngine;

namespace ProyectoFinalFolder.Components
{
    public static class ManaDropSpawner
    {
        public static void SpawnManaDrops(Vector3 origin, GameObject prefab, int count, float launchForce, float arcHeight)
        {
            for (int i = 0; i < count; i++)
            {
                Vector2 randomDir2D = Random.insideUnitCircle.normalized;
                Vector3 randomDir = new Vector3(randomDir2D.x, 0, randomDir2D.y);
                Vector3 forceDir = (randomDir + Vector3.up * arcHeight).normalized;
        
                GameObject orb = ObjectPoolManager.SpawnGameObject(prefab, origin, Quaternion.identity);
                Rigidbody rb = orb.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity  = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    rb.AddForce(forceDir * launchForce, ForceMode.Impulse);
                }
            }
        }
    }
}