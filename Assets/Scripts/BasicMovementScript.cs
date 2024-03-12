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
    
    [Tooltip("Controls how fast the character rotates in the x and y axis")]
    [SerializeField]
    private Vector2 _rotationSpeed;

    [SerializeField]
    private Camera _cam_ref;


    [SerializeField] private Vector2 _rotations = Vector2.zero;
    
    
    private void Awake()
    {
        _renderer = GetComponent<Renderer>();

        if ( Mathf.Abs(_movementSpeed ) < float.Epsilon)
        {
            _movementSpeed = 1.0f;
        }

        if (Mathf.Abs(_rotationSpeed.x) < float.Epsilon)
        {
            _rotationSpeed.x = 1.0f;
        }
        
        if (Mathf.Abs(_rotationSpeed.y) < float.Epsilon)
        {
            _rotationSpeed.x = 1.0f;
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

        Vector3 forward_dir = transform.forward;
        Vector3 side_dir = transform.right;

        Vector3 forward_movement = input_direction.y * new Vector3(forward_dir.x, 0, forward_dir.z).normalized;
        Vector3 side_movement = input_direction.x * new Vector3( side_dir.x,0,side_dir.z).normalized ;

        Vector3 final_movement = forward_movement + side_movement;
        transform.Translate(final_movement * (_movementSpeed * Time.deltaTime) );
        // Get the mouse delta. This is not in the range -1...1
        float mouse_x = _rotationSpeed.x * Input.GetAxis("Mouse X") * Time.deltaTime;
        
        float mouse_y = _rotationSpeed.y * Input.GetAxis("Mouse Y") * Time.deltaTime;
        //float v = _rotationSpeed * Input.GetAxis("Mouse Y") * 0;

        _rotations.y += mouse_x;
        _rotations.x -= mouse_y;

        if (Input.GetKey(KeyCode.R))
        {
            transform.Rotate(Vector3.up,1.0f);
        }
        
        
    }

    
    [ServerRpc] // la funcion se va a ejecutar en el lado del servidor 
     //[ServerRpc(RequireOwnership = false)] // permite ejecutar la funcion aunque no sea mi personaje
    void CambiarDeColor(Color new_color)
    {
        CambiarDeColorRPC(new_color);
    }

    [ObserversRpc]
    //[ObserversRpc(RunLocally = true)]
    void CambiarDeColorRPC(Color new_color)
    {
        _renderer.material.color = new_color;
    }
}
