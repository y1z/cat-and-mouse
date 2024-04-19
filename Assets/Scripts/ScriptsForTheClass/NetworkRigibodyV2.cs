using System;
using System.Collections;
using System.Collections.Generic;
using FishNet;
using FishNet.Object;
using UnityEngine;

public sealed class NetworkRigibodyV2 : NetworkBehaviour
{
    public Vector2 Direction;
    public float force;

    private Rigidbody2D _rb2d;
    public Vector2 startPos;

    public GameObject prefab;


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
            //_rb2d.velocity = 
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            ReiniciarRPC();
        }
    }

    [ObserversRpc()]
    void DisparaRpc(uint _serverTick)
    {
        float passedTime = (float)base.TimeManager.TimePassed(_serverTick);
        // solo si no existe  gravedad 
        // Vector2 predictedPosition = Direction * (force * passedTime);
        //_rb2d.position += predictedPosition;


        float stepInterval = 0.02f;

        // cuantos Frames de fisica vamos a calcular 
        int steps = (int)(passedTime / stepInterval);

        float vel = force * stepInterval;

        _rb2d.velocity = Direction * force;
        Vector2 predictedPosition = _rb2d.position;
        float gravedadAplicada = 0f;
        for (int i = 0; i < steps; ++i)
        {
            predictedPosition = _rb2d.position + Direction * (vel * i * stepInterval);
            gravedadAplicada = Physics2D.gravity.y / 2f * Mathf.Pow(i * stepInterval, 2);
            predictedPosition.y += gravedadAplicada;


            /*GameObject go = Instantiate( prefab );
            go.transform.position = transform.position;
            Destroy(go.GetComponent<Rigidbody2D>());*/
            //Debug.Log("dslkjf;lakdsjf;kld");
        }

        _rb2d.position = predictedPosition;
        Vector2 newVelocity = Direction * force;
        newVelocity.y += gravedadAplicada;
        _rb2d.velocity = newVelocity;
    }


    [ObserversRpc(RunLocally = true)]
    void ReiniciarRPC()
    {
        _rb2d.position = startPos;
        _rb2d.velocity = Vector2.zero;
        _rb2d.angularVelocity = 0f;
    }
}