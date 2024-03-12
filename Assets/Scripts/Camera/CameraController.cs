using FishNet.Object;
using UnityEngine;
using Cinemachine;

    public class CameraController : NetworkBehaviour
    {
        [SerializeField] private CinemachineFreeLook _cinemachine;
        public override void OnStartClient()
        {
            base.OnStartClient();
            if (!base.IsOwner)
            {
                return;
            }

            Camera c = GetComponentInChildren<Camera>();
            _cinemachine = c.GetComponent<CinemachineFreeLook>();
            _cinemachine.Follow = transform;
            _cinemachine.LookAt  = transform;
            

        }
    }