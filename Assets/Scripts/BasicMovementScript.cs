using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using Random = UnityEngine.Random;

public class BasicMovementScript : NetworkBehaviour 
{
    
    [SerializeField]
    float _movementSpeed;

    private Renderer _renderer;
    // Start is called before the first frame update

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();

        if ( Mathf.Abs(_movementSpeed ) < float.Epsilon)
        {
            _movementSpeed = 1.0f;
        }
        
    }


    void Start()
    {
    }


    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        if (base.Owner.IsLocalClient)
        {
            _renderer.material.color = Color.cyan;
        }
    }


    // Update is called once per frame
    void Update()
    {

        if (!base.IsOwner)
        {
            return;
        }


        if (Input.GetKeyDown(KeyCode.C))
        {
            Color new_color = Random.ColorHSV();
            CambiarDeColor(new_color);
        }


        Vector3 input_direction = Vector3.zero;
        input_direction.x = Input.GetAxis("Horizontal");
        input_direction.y = Input.GetAxis("Vertical");

        transform.Translate(input_direction * (_movementSpeed * Time.deltaTime) );
    }

    
    [ServerRpc] // la funcion se va a ejecutar en el lado del servidor 
     //[ServerRpc(RequireOwnership = false)] // permite ejecutar la funcion aunque no sea mi personaje
    void CambiarDeColor(Color new_color)
    {
        CambiarDeColorRPC(new_color);
    }

    [ObserversRpc]
    void CambiarDeColorRPC(Color new_color)
    {
        
        _renderer.material.color = new_color;
    }
}
