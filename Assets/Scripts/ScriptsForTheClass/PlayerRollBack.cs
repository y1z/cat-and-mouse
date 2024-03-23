using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Component.ColliderRollback;
using FishNet.Managing.Timing;
using FishNet.Object;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerRollBack : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (base.IsOwner == false)
        {
            return;

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            PreciseTick pt = base.TimeManager.GetPreciseTick(base.TimeManager.Tick);
            FireServerRPC(ray.origin, ray.direction, pt);
            // hacemos el mismo raycast que el ervidor, solo para cuestiones de feedback visuales y audiotivas
            // poner el marcador de x de impacto, reprodu ir sonido de impato, reproducir particulares de sangre

        }
        
    }


    [ServerRpc]
    void FireServerRPC(Vector3 position,
        Vector3 direction,
        PreciseTick tick)
    {
        //mueve los colisionadores a donde estaban en el tick indicado
        base.RollbackManager.Rollback(tick,RollbackPhysicsType.Physics);
        
        RaycastHit hit;
        if (Physics.Raycast(position, direction, out hit))
        {
            Debug.Log($"le dio a {hit.collider}");
            // hacer danio, avisarle a los otros jugadores que fue exitoso el impacto
        }
        else
        {
            Debug.Log($"le dio a nada");
        }
        
        //Regresa colisionadores a su posicion actual en tiempo
        base.RollbackManager.Return();
        
    }
    
    
    
}
