using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObject;
    
    [SerializeField]
    private Rigidbody _body;

    public float _rotationSpeed;


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
        
    }
}
