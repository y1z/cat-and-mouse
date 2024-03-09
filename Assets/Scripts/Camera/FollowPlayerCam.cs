using System;
using UnityEngine;
using FishNet.Object;

    public class FollowPlayerCam :  NetworkBehaviour
    {
        [Tooltip("Controls where the camera is located ")]
        [SerializeField] private Transform _cameraTarget;
        
        public float speed = 10.0f;
        
        public Vector3 distance;
        
        [Tooltip("Controls where the camera looks at")]
        [SerializeField]  private Transform _lookTarget;
        
        private bool _isCameraInitalized = false;

        private void LateUpdate()
        {

            if (!base.IsOwner)
                return;
            
            if (_isCameraInitalized)
            {
                Vector3 dPos = _cameraTarget.position + distance;
                Vector3 sPos = Vector3.Lerp(transform.position, dPos, speed * Time.deltaTime);
                transform.position = sPos;
                transform.LookAt(_lookTarget.position);
                
            }

        }


        public void InitCam(Transform cameraTarget, Transform lookTarget)
        {
            _cameraTarget = cameraTarget;
            _lookTarget = lookTarget;
            _isCameraInitalized = true;
        }
    }