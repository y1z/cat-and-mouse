using System;
using System.Collections;
using System.Collections.Generic;
using Collectables;
using FishNet;
using FishNet.Object;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Assertions;

public sealed class CollectableManager : NetworkBehaviour
{
    public static CollectableManager instance;

    /// <summary>
    /// Takes care of spawning and despawning collectables 
    /// </summary>
    public CollectableSpawner spawner;

    private List<CollectableBase> _collectables = new List<CollectableBase>();

    private int totalCollected = 0;

    void Awake()
    {
        instance = this;

        Assert.IsNotNull(spawner,
            "Please assign an instance of " + nameof(CollectableSpawner) + "\nto spawner variable");
    }

    void Update()
    {
        //CheckEveryCollectable();
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("total collected = " + _collectables.Count);
        }
#endif
    }


    [ServerRpc(RequireOwnership = false)]
    public void GetEveryCollectable()
    {
        var objects = GameObject.FindGameObjectsWithTag("Collectable");

        totalCollected = objects.Length;
        foreach (var obj in objects)
        {
            //obj.GetComponent()
            if (obj.TryGetComponent(out CollectableBase collectable))
            {
                _collectables.Add(collectable);
            }
        }

        //Debug.Log("Collectables found = " + _collectables.Count);
    }

    public bool IsEveryCollectableCollected()
    {
        bool result = true;
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Collectable");

        foreach (var obj in objects)
        {
            if (!obj.TryGetComponent(out CollectableBase collectable))
            {
                continue;
            }

            if (!collectable.isCollected)
            {
                continue;
            }

            result = false;
            break;
        }


        return result;
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


    public int CollectableCount => _collectables.Count;

    public int total => totalCollected;
}