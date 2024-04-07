using System;
using System.Collections;
using System.Collections.Generic;
using Collectables;
using FishNet;
using FishNet.Object;
using UnityEngine;
using UnityEngine.Assertions;

public sealed class CollectableManager : NetworkBehaviour 
{
    
    public static CollectableManager instance;

    
    private List<CollectableBase> _collectables = new List<CollectableBase>();

    private int totalCollected = 0;
    void Awake()
    {
        instance = this;
        
        Assert.IsNotNull(spawner ,"Please assign an instance of " +nameof(CollectableSpawner) + "\nto spawner variable");
    }


    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //CheckEveryCollectable();
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("total collected = " + totalCollected);
        }
#endif
        
    }


    [ServerRpc(RequireOwnership = false)]
    public void GetEveryCollectable()
    {
       GameObject[] objects = GameObject.FindGameObjectsWithTag("Collectable");

       foreach (var obj in objects)
       {
           if (obj.TryGetComponent(out CollectableBase collectable))
           {
               _collectables.Add(collectable);
           }

       }
        
       //Debug.Log("Collectables found = " + _collectables.Count);
    }

    [ServerRpc(RequireOwnership = false)]
    public void CheckEveryCollectable()
    {
        if (!base.IsServer)
            return;
        
        foreach (var collectable in _collectables)
        {
            if (collectable.isCollected)
            {
                collectable.isCollected = false;
                InstanceFinder.ServerManager.Despawn(collectable.gameObject);
                totalCollected += 1;
            }
            
        }
    }
}
