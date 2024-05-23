using System;
using System.Collections;
using System.Collections.Generic;
using FishNet;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;


public class NetworkRigidbodyV4 : NetworkBehaviour
{
    public Vector2 Direction;
    public float force;

    private Rigidbody2D _rb2d;
    public Vector2 startPos;

    public GameObject prefab;

    public PredictionManager predictionManager;

    // SendRate : how often (in seconds) does the variable get updated
    [SyncVar(SendRate = 1f)] private ReconcilationData _reconcilationData;

    //float timeToSend


    void OnReconcilationDataChange(ReconcilationData before, ReconcilationData next, bool asServer)
    {
        if (base.IsServer)
            return;

        if (next == null)
            return;

        // Es la misma logica que DispaarRPC
        float passedTime = (float)base.TimeManager.TimePassed(next.tick);

        float stepInterval = Time.fixedDeltaTime;
        // cuantos Frames de fisica vamos a calcular 
        int steps = (int)(passedTime / stepInterval);

        _rb2d.position = next.position;
        (Vector2 finalpos, Vector2 velocity) = predictionManager.Predict(gameObject, next.velocidad, steps);

        _rb2d.position = finalpos;
        _rb2d.velocity = velocity;
    }

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

        _reconcilationData = new ReconcilationData()
        {
            position = _rb2d.position,
            velocidad = _rb2d.velocity,
            tick = base.TimeManager.Tick,
        };

        // Actualizar la informacion de reconcilacion 
    }

    [ObserversRpc()]
    void DisparaRpc(uint _serverTick)
    {
        float passedTime = (float)base.TimeManager.TimePassed(_serverTick);

        float stepInterval = 0.02f;

        // cuantos Frames de fisica vamos a calcular 
        int steps = (int)(passedTime / stepInterval);

        float vel = force * stepInterval;

        (Vector2 finalpos, Vector2 velocity) = predictionManager.Predict(gameObject, force * Direction, steps);

        _rb2d.position = finalpos;
        _rb2d.velocity = velocity;
    }


    [ObserversRpc(RunLocally = true)]
    void ReiniciarRPC()
    {
        _rb2d.position = startPos;
        _rb2d.velocity = Vector2.zero;
        _rb2d.angularVelocity = 0f;
    }

    [System.Serializable]
    public class ReconcilationData
    {
        public Vector2 position;
        public Vector2 velocidad;
        public uint tick;
    }
}