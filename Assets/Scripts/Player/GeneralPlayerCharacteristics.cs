﻿using System;
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
        
        [Tooltip("Constrols where the camera is located ")]
        [SerializeField] private Transform _cameraPos;
        
        [SerializeField] private Rigidbody _body;

        [SerializeField] private LayerMask _layerMask;

        [SerializeField] private RoleController _roleController;

        private GroundCheck _groundCheck;


        protected void Awake()
        {

            if (Mathf.Abs(_jumpForce) < float.Epsilon)
            {
                _jumpForce = 1.0f;
            }


            _renderer = GetComponent<Renderer>();
            Assert.IsNotNull(_body,"_body != null") ;
        }

        private void Start()
        {

            if (!base.Owner.IsLocalClient)
            {
                return;
            }

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

    }
    
    