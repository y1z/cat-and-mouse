using System;
using FishNet;
using FishNet.Object;
using UnityEngine;



[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : NetworkBehaviour
{

    [Tooltip("Controls how fast the characters moves")] [Header("Movement")]
    public float movementSpeed;
    [Tooltip("Controls the orientation of our movement")]
    [SerializeField] private Transform orientation;

    private Vector2 _input;

    public UnityEngine.Vector3 _moveDirection;

    [SerializeField] private Rigidbody _body;


    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
        _moveDirection = new Vector3(0, 0, 0);
        _body.freezeRotation = true;
    }

    private void Start()
    {
        
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
        
        _moveDirection = orientation.forward * _input.y + orientation.right * _input.x;

        _body.AddForce(_moveDirection.normalized * movementSpeed, ForceMode.Force);
        //MovePlayer();
    }

    private void UpdateInput()
    {
       _input.x = Input.GetAxis("Horizontal");
       _input.y = Input.GetAxis("Vertical");
    }

    private void MovePlayer()
    {
    }
    
}
