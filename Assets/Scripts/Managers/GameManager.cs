using System.Collections;
using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using UnityEngine;

/**
 * Controls global aspects of the game 
 * 
 */
public class GameManager : MonoBehaviour
{
    private int _playerCount = 0;

    private const int DEFAULT_EXPECTED_PLAYERS = 3;
    
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
