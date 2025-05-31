using System;
using UnityEngine;

namespace Code
{
    /// <summary>
    /// Physics-based cube component that uses Unity's Rigidbody and collision system
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class PhysicsCube : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private float _constantSpeed;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            
            // Configure rigidbody for bouncing behavior
            _rigidbody.useGravity = false;
            _rigidbody.angularDamping = 0;
            _rigidbody.linearDamping = 0;
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }

        public void Initialize(Vector3 velocity, PhysicsMaterial bouncyMaterial)
        {
            GetComponent<Collider>().material = bouncyMaterial;
            
            // Store the initial speed to maintain it after collisions
            _constantSpeed = velocity.magnitude;
            
            // Set initial velocity
            _rigidbody.linearVelocity = velocity;
        }
        
        private void FixedUpdate()
        {
            // Check if the current speed deviates from the constant speed
            if (!Mathf.Approximately(_rigidbody.linearVelocity.magnitude, _constantSpeed) && _rigidbody.linearVelocity.sqrMagnitude > 0)
            {
                // Normalize the current direction and multiply by the constant speed
                _rigidbody.linearVelocity = _rigidbody.linearVelocity.normalized * _constantSpeed;
            }
        }
    }
}

