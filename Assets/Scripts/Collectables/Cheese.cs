using System;
using System.Collections;
using System.Collections.Generic;
using FishNet;
using FishNet.Object;
using UnityEngine;
using UnityEngine.Assertions;

public sealed class Cheese : CollectableBase
{
    [SerializeField] [Tooltip("The collider for the cheese")]
    private SphereCollider _Collider;

    private void Start()
    {
        if (_Collider == null)
        {
            _Collider = GetComponent<SphereCollider>();
        }
        Assert.IsNotNull(_Collider);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.TryGetComponent(out GeneralPlayer player))
        {
            DisappearObject(player);
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
                print("In method = " + nameof(OnTriggerEnter));
                
               // gameObject.SetActive(false);
                //Collect(player);
                CollectableManager.instance.spawner.DespawnCollectable(this.NetworkObject);
            }
    }
    
    
}
