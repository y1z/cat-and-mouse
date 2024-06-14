using System;
using System.Collections;
using System.Collections.Generic;
using FishNet;
using FishNet.Object;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Rigidbody))]
public sealed class Cheese : CollectableBase
{
    [SerializeField] [Tooltip("The collider for the cheese")]
    private SphereCollider _Collider;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        if (_Collider == null)
        {
            _Collider = GetComponent<SphereCollider>();
        }
        Assert.IsNotNull(_Collider);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isCollected = true;
        }

        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<GeneralPlayer>();
            DisappearObject( player);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void DisappearObject(GeneralPlayer player)
    {
        DisappearObjectRPC(player);
    }

    [ObserversRpc]
    private void DisappearObjectRPC(GeneralPlayer player)
    {
        bool get_collected = player.CanCurrentRoleCollect();
        if (get_collected)
        {
#if UNITY_EDITOR
            string debug_string =
                Utility.StringUtil.addColorToString("In method = " + nameof(OnTriggerEnter), Color.yellow);
            Debug.Log(debug_string, this);
#endif

            CollectableManager.instance.spawner.DespawnCollectable(this.NetworkObject);
        }
    }
}