using System;
using FishNet.Object;
using TMPro;
using UnityEngine;
using Vector2 =  UnityEngine.Vector2;

    public class NetworkRigidbodyV1 : NetworkBehaviour
    {
        public Vector2 Direction;
        public float force;

        private Rigidbody2D _rb2d;
        public Vector2 startPos;


        private void Start()
        {
            _rb2d = GetComponent<Rigidbody2D>();
            startPos = transform.position;
        }

        private void Update()
        {
            if (base.IsServer == false)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                DisparaRpc(base.TimeManager.Tick);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
               ReiniciarRPC(); 
            }
        }

        [ObserversRpc(RunLocally = true)]

        void DisparaRpc(uint _serverTick)
        {
            float passedTime = (float)base.TimeManager.TimePassed(_serverTick);
            _rb2d.velocity = Direction * force;
        }


        [ObserversRpc(RunLocally = true)]
        void ReiniciarRPC()
        {
            _rb2d.position = startPos;
            _rb2d.velocity = Vector2.zero;
            _rb2d.angularVelocity = 0f;

        }
    }