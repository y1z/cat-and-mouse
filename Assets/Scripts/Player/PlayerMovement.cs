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

    private GroundCheck _groundCheck;

    private void Start()
    {
        
        _body = GetComponent<Rigidbody>();
        _moveDirection = new Vector3(0, 0, 0);
        _body.freezeRotation = true;
        _defaultDrag = _body.drag;
        _groundCheck = GetComponent<GroundCheck>();
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

       bool is_moving = MathF.Abs(_input.x) < float.Epsilon && MathF.Abs(_input.y) < float.Epsilon;
       bool is_grounded = _groundCheck.IsGrounded;

       if (is_moving && is_grounded && _body.velocity.magnitude > 1.0f)
       {
           _body.drag = Mathf.Clamp(_body.velocity.magnitude, _minimumDrag, float.MaxValue  ) ;
       }
       else if( is_grounded)
       {
           _body.drag = _defaultDrag;
       }
    }
    
}
