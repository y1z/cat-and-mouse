using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using FishNet;

/// <summary>
///  Controls how the player dashes 
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public sealed class PlayerDash : MonoBehaviour
{
    [Header("Dash variables")] public float dashForce;
    public float dashTime;
    public float dashCooldown;


    // keeps tracks of the times between cool down 
    [SerializeField] private float currentDashCooldown;

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
        currentDashCooldown = dashCooldown;
        Assert.IsNotNull(_player);
        Assert.IsNotNull(_body);
        Assert.IsNotNull(_playerMovement);
    }

    void Update()
    {
        float tickDelta = (float)InstanceFinder.TimeManager.TickDelta;


        currentDashCooldown += tickDelta;
    }

    public void Dash()
    {
        WaitForSeconds seconds = new WaitForSeconds(dashTime);
        bool is_cooldown_finished = currentDashCooldown >= dashCooldown;
        if (!IsDashing && is_cooldown_finished)
        {
            currentDashCooldown = 0.0f;
            StartCoroutine(DoDash(seconds));
        }
    }

    private IEnumerator DoDash(WaitForSeconds seconds)
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