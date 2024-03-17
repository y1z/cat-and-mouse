using System.Collections;
using System.Collections.Generic;
using FishNet;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {

        var a = InstanceFinder.NetworkManager.ServerManager.Clients;
    }

    void Update()
    {
        
    }
}
