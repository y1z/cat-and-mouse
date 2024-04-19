using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;

public class TimeManagerEvents : MonoBehaviour
{
    private Queue<Vector3> historialPosiciones = new Queue<Vector3>(10);

    void Start()
    {
        // si el script es de monobehaviour 
        //InstanceFinder.TimeManager
        InstanceFinder.TimeManager.OnTick += OnTick;

        //SendMessage();
        // si heredados del networkBehaviour
        // base.TimeManager
    }


    private void Update()
    {
    }

    private void OnCollisionEnter(Collision other)
    {
        //other.gameObject.SendMessage("Danio", 5);
    }

    /**
     * todas las funciones son del tiem manager
     */
    void OnTick()
    {
        historialPosiciones.Enqueue(transform.position);
        if (historialPosiciones.Count > 10)
        {
            historialPosiciones.Dequeue();
        }
    }

    void OnPostTick()
    {
    }

    void OnPreTick()
    {
    }

    void OnUpdate()
    {
    }

    void OnFixedUpdate()
    {
    }

    void OnLateUpdate()
    {
    }


    void OnPostPhysicsSimulation(float dt)
    {
    }

    void OnPrePhysicisSimulation(float dt)
    {
    }

    void OnRoundTripTimeUpdate(float dt)
    {
    }
}