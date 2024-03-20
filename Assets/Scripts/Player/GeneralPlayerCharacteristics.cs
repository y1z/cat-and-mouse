using System;
using System.Reflection;
using FishNet;

using FishNet.Component.Spawning;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Sever;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

    /**
     * This class controls the general characteristics for each type of player
     * entity in this project.
     */
    [RequireComponent(typeof(Rigidbody))]
    public class GeneralPlayerCharacteristics : NetworkBehaviour
    {

        [Tooltip("Controls how heigh the entity jumps")]
        public float _jumpForce;
        
        protected Renderer _renderer;
        
        [SerializeField] private Rigidbody _body;

        [SerializeField] private RoleController _roleController;

        private GroundCheck _groundCheck;

        [SerializeField] private Transform _orientation;

        [Tooltip("Attach an object with a sphere collider ")] [SerializeField]
        private Transform _sphereColliderObj;

        public SphereCollider _sphereCollider;

        [Tooltip("A reference to the players camera")]
        [SerializeField] private Camera _camera;

        public  float height;


        [SyncVar(OnChange = nameof(OnChange_health))]
        public float health = 100.0f;
        
        protected void Awake()
        {

            if (Mathf.Abs(_jumpForce) < float.Epsilon)
            {
                _jumpForce = 1.0f;
            }

            _renderer = GetComponent<Renderer>();
            _sphereCollider = _sphereColliderObj.GetComponent<SphereCollider>();
            Vector3 size = _body.GetComponent<CapsuleCollider>().bounds.size;
            height = size.y;
            Assert.IsNotNull(_body,"_body != null") ;
            Assert.IsNotNull(_sphereCollider,"_sphereCollider should NOT be null") ;
            Assert.IsNotNull(_sphereColliderObj,"_sphereColliderObj should NOT be null") ;
        }

        private void Start()
        {
            
        }

        public override void OnStartClient()
        {
            base.OnStartClient();

            if (base.IsServer)
            {
                PlayerManager.instance.playersData.Add(gameObject.GetInstanceID(), new PlayerData(){health = 1.0f}  );
                PlayerManager.instance._players.Add(base.Owner ,new PlayerData() {health = Globals.DEFAULT_PLAYER_HEALTH });
            }
            
            if (!base.Owner.IsLocalClient)
            {
                return;
            }
            Assert.IsNotNull(_orientation, "Orientation should NOT be null"); 

            _renderer = GetComponent<Renderer>();
            _roleController = GetComponent<RoleController>();
            _groundCheck = GetComponent<GroundCheck>();
            
            Transform[] valid_spawn_points = InstanceFinder.NetworkManager.GetComponent<PlayerSpawner>().Spawns;
            bool is_in_spawn_point = isPlayerInSpawn(valid_spawn_points);
            
            if (!is_in_spawn_point)
            {
               var spawn_point_index = UnityEngine.Random.Range(0, valid_spawn_points.Length - 1);
               transform.position = valid_spawn_points[spawn_point_index].position;
            }
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
                var temp = new MouseRole();
                _roleController.Initialize(this, temp);
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                var temp = new CatRole();
                _roleController.Initialize(this, temp);
            }
            
        #endif // UNITY_EDITOR

            DoPlayerJump();
            
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
            if (Input.GetButtonDown("Jump") && _groundCheck.IsGrounded)
            {
                _body.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            }
            
        }


        private bool isPlayerInSpawn(Transform[] valid_spawn_points)
        {
            Vector3 player_position = transform.position;
            bool result = false;
            
            foreach (var spawn_point in valid_spawn_points)
            {
                if (Vector3.Distance(player_position, spawn_point.position) <=
                    Globals.DEFAULT_MAX_INITIAL_DISTANCE_FROM_SPAWN)
                {
                    result = true;
                }
            }

            return result;
        }

        // TODO : DELETE THIS FUNCION LATER 
        
        [ServerRpc(RequireOwnership = false)]
        public void testFunction()
        {
            Debug.Log("excuted testfunction");
            health -= 5;
        }

        [ObserversRpc]
        private void testFunctionRPC()
        {
            
        }

        private void OnChange_health(float prev, float next, bool as_server)
        {

            Debug.Log("prev = " + prev + "\n" + "next = " + next);

        }

        public Transform CameraTranform => _camera.transform;
    }
    
    