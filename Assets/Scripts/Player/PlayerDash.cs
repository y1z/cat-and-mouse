using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
///  Controls how the player dashes 
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public sealed class PlayerDash : MonoBehaviour
{

    [Header("Dash variables")]
    public float dashForce;
    public float dashTime;

    private GeneralPlayer _player;
    
    public bool IsDashing { get; private set; }

    private Coroutine _coroutine;
    private PlayerMovement _playerMovement;
    private Rigidbody _body;
    
    // Start is called before the first frame update
    void Start()
    {
        IsDashing = false;
        _player = GetComponent<GeneralPlayer>();
        _body = GetComponent<Rigidbody>();
        _playerMovement = GetComponent<PlayerMovement>();
        Assert.IsNotNull(_player);
        Assert.IsNotNull(_body);
        Assert.IsNotNull(_playerMovement);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Dash()
    {
        WaitForSeconds seconds = new WaitForSeconds(dashTime);
        if (!IsDashing)
        {
            StartCoroutine(DoDash(seconds));
        }

    }

    private IEnumerator DoDash( WaitForSeconds seconds)
    {
        IsDashing = true;
        bool original_gravity = _body.useGravity;
        _body.useGravity = !_body.useGravity;

        Vector3 move_dir_without_y =
            new Vector3(_playerMovement.moveDirection.x, 0.0f, _playerMovement.moveDirection.z);
        _body.velocity = move_dir_without_y * dashForce;


       yield return seconds;
       IsDashing = false;
       _body.useGravity = original_gravity;
       yield return null;
    }
}
