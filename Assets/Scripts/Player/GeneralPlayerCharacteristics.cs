﻿using FishNet.Object;
using UnityEngine;

namespace Player
{
    public class GeneralPlayer : NetworkBehaviour
    {
        // characteristics 
        public float _movementSpeed = 0;
        public float _rotationSpeed;
        private Renderer _renderer;
        // This object controls which way is forward for the player
        private Transform _forwardObject;
        
    }
}