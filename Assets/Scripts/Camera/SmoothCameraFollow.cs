using System;
using FishNet.Object;
using UnityEngine;

namespace Camera
{
    public class SmoothCameraFollow : NetworkBehaviour
    {

        private Vector3 _offset;
        [SerializeField] private Transform _target;
        [SerializeField] private float _smoothTime;
        private Vector3 _currentVelocity = Vector3.zero;
        private bool _isCameraInitialized = false;

        private void Awake()
        {
            _offset = transform.position - _target.position;
        }


        private void LateUpdate()
        {
            Vector3 targetPosition = _target.position + _offset;
            transform.position =
                Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, _smoothTime);
        }

        public void InitCameraOnPlayer(Player.Player player, float distance_from_target)
        {
            _target = player.ForwardObject;
            Vector3 player_position = player.transform.position;
            _offset = player_position - _target.position;
            Vector3 delta_from_player = player.BackwardsVector * distance_from_target;
            Vector3 starting_position = player_position + delta_from_player;
            UnityEngine.Camera.main.transform.position = starting_position;

        }
        
        public bool IsCameraInitialized
        {
            get => _isCameraInitialized;
        }
        
    }
}