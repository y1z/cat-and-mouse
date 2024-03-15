    
    using System;
    using UnityEngine;

    
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerJump : MonoBehaviour
    {
        [Tooltip("Controls high the player jumps")]
        public float jumpForce;

        [SerializeField] private Rigidbody _rb;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            
        }

        private void FixedUpdate()
        {
            
        }
    }