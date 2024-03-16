using System;
using FishNet;
using FishNet.Object;
using Unity.Mathematics;
using UnityEngine;



[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : NetworkBehaviour
{

    [Tooltip("Controls how fast the characters moves")] [Header("Movement")]
    public float movementSpeed;
    [Tooltip("Controls the orientation of our movement")]
    [SerializeField] private Transform orientation;

    private Vector2 _input;

    public Vector3 _moveDirection;

    [SerializeField] private Rigidbody _body;

    [SerializeField] private float _defaultDrag;

    [SerializeField] private float _minimumDrag;

    private void Start()
    {
        
        _body = GetComponent<Rigidbody>();
        _moveDirection = new Vector3(0, 0, 0);
        _body.freezeRotation = true;
        _defaultDrag = _body.drag;
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
       _input.x = Input.GetAxisRaw("Horizontal");
       _input.y = Input.GetAxisRaw("Vertical");

       if (MathF.Abs(_input.x) < float.Epsilon && MathF.Abs(_input.y) < float.Epsilon)
       {
           _body.drag = Mathf.Clamp(_body.velocity.magnitude, _minimumDrag, float.MaxValue  ) ;
       }
    }

    private void MovePlayer()
    {
    }
    
}
