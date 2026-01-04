using System;
using ProyectoFinalFolder.Common.Components.BaseComponent;
using ProyectoFinalFolder.Components.PoolSystem;
using UnityEngine;

namespace ProyectoFinalFolder.Components
{
    //TODO: FIX MANA COLLISIONS
    public class ManaDropScript : CollectibleComponent
    {
        //TODO: CHANGE FOR THE PLAYER UPGRADE VALUES
        [SerializeField] private float manaRestoreAmount = 2f;

        [Header("Floating Effect")]
        [SerializeField] private float floatingHeight = 1.5f;
        [SerializeField] private float raycastLength = 0.6f; 
        [SerializeField] private float floatForce = 10f;
        [SerializeField] private float damping = 0.5f;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private string groundTag = "Ground";

        private bool hasTouchedGround = false;
        private Rigidbody rb;
        private ManaComponent manaComponent;

        private void Awake()
        {
            pickUpCollider = GetComponent<Collider>();
            if (pickUpCollider == null)
                Debug.LogError("No collider found on the collectible object.");

            rb = GetComponent<Rigidbody>();
            if (rb == null)
                Debug.LogError("No Rigidbody found on the collectible object.");
        }

        private void OnEnable()
        {
            hasTouchedGround = false;
            manaComponent = null;
        }

        private void FixedUpdate()
        {
            Ray ray = new Ray(transform.position, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, raycastLength, groundLayer))
            {
                if (!hasTouchedGround && hit.collider.CompareTag(groundTag))
                {
                    hasTouchedGround = true;
                }

                if (hasTouchedGround)
                {
                    MaintainFloatingHeight(hit);
                }
            }
        }

        private void MaintainFloatingHeight(RaycastHit hit)
        {
            float heightDifference = floatingHeight - hit.distance;
            float lift = heightDifference * floatForce - rb.linearVelocity.y * damping;
            rb.AddForce(Vector3.up * lift, ForceMode.Acceleration);
            
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
        }

        protected override void CollectInteraction()
        {
            ObjectPoolManager.ReturnToPool(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
            manaComponent = other.gameObject.GetComponentInParent<ManaComponent>();
            if (manaComponent == null) return;
            if(manaComponent.currentMana >= manaComponent.maxMana)
            {
                CollectInteraction();
            }
            manaComponent.RestoreMana(manaRestoreAmount);
            //TODO:Insert sound and effect here
            CollectInteraction();
        }

       

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, Vector3.down * floatingHeight);
        }
    }
}
