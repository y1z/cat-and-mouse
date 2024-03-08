using System;
using Camera;
using FishNet.Object;
using Unity.VisualScripting;
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
        
        protected Renderer _renderer;
        
        [Tooltip("This object controls which way is forward for the player")]
        [SerializeField] private Transform _forwardObject;

        [Tooltip("This object check if we are on the ground")]
        [SerializeField] private Transform _groundCheckObject;
        
        [Tooltip("Constrols where the camera is located ")]
        [SerializeField] private Transform _cameraPos;

        [SerializeField] private Transform _forwardPosition;
        
        [SerializeField] private Transform _backwardsPosition;
        
        protected Vector3 _forwardVector = Vector3.forward;
        
        [SerializeField] private Rigidbody _body;

        [SerializeField] private LayerMask _layerMask;

        private Role _role;


        [SerializeField]
        public UnityEngine.Camera _cam;

        protected void Awake()
        {
            if(Mathf.Abs(_movementSpeed) < float.Epsilon) 
            {
                _movementSpeed = 1.0f;
            }

            if (Mathf.Abs(_jumpForce) < float.Epsilon)
            {
                _jumpForce = 1.0f;
            }

            _forwardPosition.position = transform.position;
            _forwardPosition.rotation = transform.rotation;


            _backwardsPosition.position = transform.position;
            _backwardsPosition.rotation = transform.rotation;

            Assert.IsNotNull(_body,"_body != null") ;
            Assert.IsNotNull(_forwardObject, "_forwardObject != null");
        }

        protected void Start()
        {
            _renderer = GetComponent<Renderer>();
            var temp = _cam.GetComponent<FollowPlayerCam>();
            
           temp.InitCam(_backwardsPosition, _forwardPosition);
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
            
            _forwardPosition.position = transform.position + (CalculateForward() * 5);
            _backwardsPosition.position = transform.position + CalculateForward() * (10 * -1);

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
            Vector3 dist = (_forwardObject.transform.position - transform.position).normalized;
            
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

            Vector3 right = Vector3.Cross(Vector3.up, _forwardVector).normalized;

            Vector3 side_movement = input_direction.x * right;
            Vector3 foward_movement = input_direction.y * _forwardVector;
            
            Vector3 final_movement = (foward_movement + side_movement);

            
            transform.Translate ( Vector3.forward * (_movementSpeed * input_direction.y * Time.deltaTime), _cam.transform );
            
        }

//_movementSpeed *
        private void DoPlayerJump()
        {
            if (Input.GetButtonDown("Jump") && IsGrounded())
            {
                _body.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            }
            
        }


        private bool IsGrounded()
        {
            return Physics.CheckSphere(_groundCheckObject.position, 0.1f, _layerMask);
        }


        public Transform ForwardObject
        {
            get { return _forwardObject; }
        }

        public Vector3 BackwardsVector => _forwardVector * -1;
    }
}