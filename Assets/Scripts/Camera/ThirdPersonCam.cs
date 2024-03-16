using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using FishNet;
using FishNet.Object;
using UnityEngine;
using UnityEngine.Assertions;

public class ThirdPersonCam : NetworkBehaviour 
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObject;
    
    [SerializeField]
    private Rigidbody _body;

    public float _rotationSpeed;

    [SerializeField] private Camera _camera;

    [SerializeField] private CinemachineFreeLook _cinemachine;
    public void Start()
    {
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!base.Owner.IsLocalClient)
        {
            return;
        }
        
        _camera = GetComponent<Camera>();
        Assert.IsNotNull(_camera,"_camera != null");
        _camera.gameObject.SetActive(true);
        
        _cinemachine = _camera.GetComponent<CinemachineFreeLook>();
        _cinemachine.Follow = player;
        _cinemachine.LookAt = player;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //InstanceFinder.NetworkManager.spawns
    }


    public void Update()
    {

        Vector3 position = transform.position; 
        Vector3 viewDir = player.position - new Vector3(position.x,player.position.y, position.z);
        orientation.forward = viewDir.normalized;


        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 input_dir = orientation.forward * vertical + orientation.right * horizontal;

        if (input_dir != Vector3.zero)
        {
            playerObject.forward = Vector3.Slerp(player.forward, input_dir.normalized, _rotationSpeed * Time.deltaTime);
        }

    #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            switch (Cursor.lockState)
            {
                case CursorLockMode.Locked:
                    Cursor.lockState = CursorLockMode.None;
                    break;
                case CursorLockMode.None:
                    Cursor.lockState = CursorLockMode.Locked;
                    break;
                default:
                    Cursor.lockState = CursorLockMode.None;
                    break;
            }
            
            
        }
    #endif
        
    }
    
}
