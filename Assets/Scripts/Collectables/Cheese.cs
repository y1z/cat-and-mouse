using System;
using System.Collections;
using System.Collections.Generic;
using FishNet;
using UnityEngine;
using UnityEngine.Assertions;

public sealed class Cheese : CollectableBase
{
    [SerializeField] [Tooltip("The collider for the cheese")]
    private SphereCollider _Collider;

    private LayerMask _mask;
    
    void Start()
    {
        if (_Collider == null)
        {
            _Collider = GetComponent<SphereCollider>();
        }
        _mask = Globals.PLAYER_MASK;
        Assert.IsNotNull(_Collider);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.TryGetComponent(out GeneralPlayer player))
        {
            bool get_collected = player.canCurrentRoleCollect();
            if (get_collected)
            {
                Collect(player);
            }
        }
        
    }

    public override void Collect(GeneralPlayer player)
    {
        base.Collect(player);
    }
}
