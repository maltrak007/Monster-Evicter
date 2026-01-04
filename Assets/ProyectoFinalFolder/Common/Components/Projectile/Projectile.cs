using System;
using System.Collections;
using ProyectoFinalFolder.Common.Components.BaseComponent;
using ProyectoFinalFolder.Components.PoolSystem;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float speed = 10f;
    
    [SerializeField] private float damage = 10f;
    [SerializeField] private float weaknessMultiplicator = 2f;
    
    [SerializeField] private Affinities affinity = Affinities.None;
    
    private Rigidbody projectileRb;
    
    private Vector3 launchDirection;
    
    private Coroutine returnToPoolCoroutine;
    
    private void Awake()
    {
        projectileRb = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        
        if (launchDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(launchDirection);
        }
    }
    
    public void Launch(Vector3 direction)
    {
        transform.parent = null; 
        launchDirection = direction.normalized;
        projectileRb.linearVelocity = launchDirection * speed;
    }

    private void OnCollisionEnter(Collision other)
    {
        ObjectPoolManager.ReturnToPool(gameObject, ObjectPoolManager.PoolType.Projectiles);
    }
    
    public float GetDamage()
    {
        return damage;
    }
    
    public void SetDamage(float _damage)
    {
        damage = _damage;
    }
    
    public float GetWeaknessMultiplicator()
    {
        return weaknessMultiplicator;
    }
    
    public void SetWeaknessMultiplicator(float _weaknessMultiplicator)
    {
        weaknessMultiplicator = _weaknessMultiplicator;
    }
    
    public Affinities GetAffinity()
    {
        return affinity;
    }
}
