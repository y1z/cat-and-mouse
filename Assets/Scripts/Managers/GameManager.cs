using System.Collections;
using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

/**
 * 
 */
public class GameManager : MonoBehaviour
{
    private int _playerCount = 0;

    private bool _initalize_game = false;
    
    void Start()
    {
    }

    void Update()
    {
        
       UpdatePlayerCount(); 
    }

    void UpdatePlayerCount()
    {
        //_playerCount = InstanceFinder.NetworkManager.ServerManager.Clients.Count;
    }
    
    
    
}
