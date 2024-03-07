﻿using System;
using FishNet.Object;
using UnityEngine;

namespace Camera
{
    public class SmoothCameraFollow : NetworkBehaviour
    {

        private Vector3 _offset;
        [SerializeField] private Transform _target;
        [SerializeField] private float _smoothTime;
        private Vector3 _currentVelocity = Vector3.zero;

        private void Awake()
        {
            _offset = transform.position - _target.position;
        }


        private void LateUpdate()
        {
            Vector3 targetPosition = _target.position + _offset;
            transform.position =
                Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, _smoothTime);
        }
    }
}