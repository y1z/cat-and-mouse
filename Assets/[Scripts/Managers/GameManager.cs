using System.Collections;
using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

namespace Managers
{
    
/**
 * 
 */
public class GameManager : MonoBehaviour
{
        public CollectableManager collectableManager;

        public PlayerManager playerManager;

    public static GameManager instance;
    private int _playerCount = 0;

    private bool _initalize_game = false;

    void Start()
    {
    }

    void Update()
    {
        UpdatePlayerCount();
        var clients = InstanceFinder.NetworkManager.ServerManager.Clients;
        //Debug.Log("player count = " + _playerCount);
        string result = "";
        foreach (var client in clients)
        {
            result += $" {client}, ";
        }

        Debug.Log(result, this);
    }

    void UpdatePlayerCount()
    {
        _playerCount = InstanceFinder.NetworkManager.ServerManager.Clients.Count;
    }
}
    
}