using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public sealed class GroundCheck : MonoBehaviour
{
    [Tooltip("Attach to the bottom of the player to check if they are touching the ground")] [SerializeField]
    private Transform _groundCheckObject;

    [Tooltip("Set it to the 'ground' layer or make a 'ground' layer to set it to ")] [SerializeField]
    private LayerMask _layerMask;

    // tells you if you are grounded
    public bool IsGrounded { get; private set; }

    private void Start()
    {
        Assert.IsNotNull(_groundCheckObject, "_groundCheckArea should NOT BE null ");
        IsGrounded = true;
    }

    private void Update()
    {
        UpdateGroundCheck();
    }

    private void UpdateGroundCheck()
    {
        IsGrounded = Physics.CheckSphere(_groundCheckObject.position, 0.1f, _layerMask);
    }
}