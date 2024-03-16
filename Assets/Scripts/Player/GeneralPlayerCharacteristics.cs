using System;
using FishNet;

using FishNet.Component.Spawning;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;
using UnityEngine.Assertions;

    /**
     * This class controls the general characteristics for each type of player
     * entity in this project.
     */
    public class GeneralPlayerCharacteristics : NetworkBehaviour
    {

        [Tooltip("Controls how heigh the entity jumps")]
        public float _jumpForce;
        
        protected Renderer _renderer;
        
        //[Tooltip("This object controls which way is forward for the player")]
        //[SerializeField] private Transform _forwardObject;

        [Tooltip("This object check if we are on the ground")]
        [SerializeField] private Transform _groundCheckObject;
        
        [Tooltip("Constrols where the camera is located ")]
        [SerializeField] private Transform _cameraPos;
        
        [SerializeField] private Rigidbody _body;

        [SerializeField] private LayerMask _layerMask;

        private IRole _role;


        [SyncVar] private Vector3 _playerPosition;
        protected void Awake()
        {

            if (Mathf.Abs(_jumpForce) < float.Epsilon)
            {
                _jumpForce = 1.0f;
            }

            _role = null;

            _renderer = GetComponent<Renderer>();
            Assert.IsNotNull(_body,"_body != null") ;
        }

        protected void Start()
        {

            if (!base.Owner.IsLocalClient)
            {
                return;
            }

            _playerPosition = transform.position;
            _renderer = GetComponent<Renderer>();

            Transform[] valid_spawn_points = InstanceFinder.NetworkManager.GetComponent<PlayerSpawner>().Spawns;

            Vector3 player_position = transform.position;
            bool is_in_spawn_point = false;
            foreach (var spawn_point in valid_spawn_points)
            {
                if (Vector3.Distance(player_position, spawn_point.position) <=
                    Globals.DEFAULT_MAX_INITIAL_DISTANCE_FROM_SPAWN)
                {
                    is_in_spawn_point = true;
                }

            }

            if (!is_in_spawn_point)
            {
               var spawn_point_index = UnityEngine.Random.Range(0, valid_spawn_points.Length - 1);
               transform.position = valid_spawn_points[spawn_point_index].position;
               print("SET PLAYER SPAWN POINT MANUALLY ");
            }
            
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            
        }


        private void Update()
        {
            if (!base.IsOwner)
                return;
            
            
            #if UNITY_EDITOR 
            

            if (Input.GetKeyDown(KeyCode.C))
            {
                Color new_color = UnityEngine.Random.ColorHSV(); 
                ChangeColor(new_color);
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                _role = new MouseRole();
            }
            
            #endif // UNITY_EDITOR
            
            
            DoPlayerJump();
            

            
            _playerPosition = transform.position;

        }


        [ServerRpc]
        public void ChangeColor(Color new_color)
        {
           ChangeColorRPC(new_color); 
        }


        [ObserversRpc]
        protected void ChangeColorRPC(Color new_color)
        {

            _renderer.material.color = new_color;
        }

        private void DoPlayerJump()
        {
            if (Input.GetButtonDown("Jump") && IsGrounded())
            {
                _body.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            }
            
        }

        private void DoPlayerRole()
        {
            if (_role != null)
            {
                _role.OnUpdate(this);
            }
            
        }


        private bool IsGrounded()
        {
            return Physics.CheckSphere(_groundCheckObject.position, 0.1f, _layerMask);
        }

    }