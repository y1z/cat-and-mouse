using System;
using FishNet.Object;
using UnityEngine;
using UnityEngine.Assertions;


namespace Player
{
    /**
     * This class controls the general characteristics for each type of player
     * entity in this project.
     */
    public class GeneralPlayerCharacteristics : NetworkBehaviour
    {
        [Tooltip("Controls how fast the entity moves")]
        public float _movementSpeed = 0;

        [Tooltip("Controls how heigh the entity jumps")]
        public float _jumpForce;
        
        public float _rotationSpeed;
        private Renderer _renderer;
        [Tooltip("This object controls which way is forward for the player")]
        [SerializeField] private Transform _forwardObject;
        private Vector3 _forwardVector = Vector3.forward;
        [SerializeField] private Rigidbody _body;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            if(Mathf.Abs(_movementSpeed) < float.Epsilon) 
            {
                _movementSpeed = 1.0f;
            }

            if (Mathf.Abs(_jumpForce) < float.Epsilon)
            {
                _jumpForce = 1.0f;
            }

            //_body = GetComponent<Rigidbody>();
            Assert.IsNotNull(_body,"_body != null") ;
            Assert.IsNotNull(_forwardObject, "_forwardObject != null");
        }

        private void Start()
        {
            
        }


        private void Update()
        {
            
            if (!base.IsOwner)
            {
                return;
            }

            
            #if UNITY_EDITOR 
            

            if (Input.GetKeyDown(KeyCode.C))
            {
                Color new_color = UnityEngine.Random.ColorHSV(); 
                ChangeColor(new_color);
            }
            #endif // UNITY_EDITOR
            
            DoMouseRotation();

            DoPlayerMovement(); 
            
            DoPlayerJump();
            
        }


        [ServerRpc]
        protected void ChangeColor(Color new_color)
        {
           ChangeColorRPC(new_color); 
        }


        [ObserversRpc]
        protected void ChangeColorRPC(Color new_color)
        {

            _renderer.material.color = new_color;
        }


        //Calculates what forward is the character
        // Note: we ignore the y-axis because we don't need it for movement
        private Vector3 CalculateForward()
        {
            Vector3 dist = _forwardObject.transform.position - transform.position;
            
            return new Vector3(dist.x,0,dist.z);
        }

        private void DoMouseRotation()
        {
            
            // Get the mouse delta. This is not in the range -1...1
            float h = _rotationSpeed * Input.GetAxis("Mouse X");
            float v = _rotationSpeed * Input.GetAxis("Mouse Y") * 0;

            transform.Rotate(v, h, 0);
        }

        private void DoPlayerMovement()
        {
            _forwardVector = CalculateForward();

            Vector3 input_direction = Vector3.zero;
            input_direction.x = Input.GetAxis("Horizontal");
            input_direction.y = Input.GetAxis("Vertical");

            Vector3 right = Vector3.Cross(Vector3.up, _forwardVector);

            Vector3 side_movement = input_direction.x * right;
            Vector3 foward_movement = input_direction.y * _forwardVector;
            
            Vector3 final_movement = foward_movement + side_movement;

            transform.Translate(final_movement * (_movementSpeed * Time.deltaTime));
            
        }


        private void DoPlayerJump()
        {
            if (Input.GetButtonDown("Jump"))
            {
               _body.AddForce(Vector3.up * _jumpForce,ForceMode.Impulse); 
            }
            
        }
    }
}