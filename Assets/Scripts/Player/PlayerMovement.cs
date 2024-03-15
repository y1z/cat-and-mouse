using System;
using FishNet.Object;
using UnityEngine;



[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : NetworkBehaviour 
{
    [Header("Movement")]
    public float movementSpeed;

    public Transform orientation;

    private Vector2 input;

    private Vector3 move_direction;

    private Rigidbody _body;


    private void Start()
    {
        _body = GetComponent<Rigidbody>();

        _body.freezeRotation = true;
    }


    private void Update()
    {
        if (!base.IsOwner)
        {
            return;
        }
        UpdateInput();
        
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void UpdateInput()
    {
       input.x =  Input.GetAxis("Horizontal");
       input.y = Input.GetAxis("Vertical");
    }

    private void MovePlayer()
    {
        move_direction = orientation.forward * input.y + orientation.right * input.x;
        
        
        _body.AddForce(move_direction.normalized * movementSpeed, ForceMode.Force);
        
    }
    
}
