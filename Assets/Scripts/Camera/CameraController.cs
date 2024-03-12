using FishNet.Object;
using UnityEngine;
using Cinemachine;

    public class CameraController : NetworkBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _cinemachine;
        public override void OnStartClient()
        {
            base.OnStartClient();
            if (!base.IsOwner)
            {
                return;
            }

            Camera c = GetComponentInChildren<Camera>();
            _cinemachine = c.GetComponent<CinemachineVirtualCamera>();
            _cinemachine.Follow = transform;
            _cinemachine.LookAt  = transform;
            

        }
    }