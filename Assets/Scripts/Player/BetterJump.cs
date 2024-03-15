﻿using System;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class BetterJump : MonoBehaviour
{
    [SerializeField] private float fall_multiplier;
    [SerializeField] private float low_jump_multiplier;

    private Rigidbody _body;

    private void Start()
    {
        _body = GetComponent<Rigidbody>();
    }


    private void Update()
    {
        if (_body.velocity.y < 0.0f)
        {
            print("chaging velocity ");
            _body.velocity += Vector3.up * (Physics.gravity.y * (fall_multiplier - 1) * Time.deltaTime);
        }

    }
}