using System;
using FishNet.Transporting;
using UnityEngine;
using Unity;

public class MainMenuActivador : MonoBehaviour
{
    private void Start()
    {
        FishNet.InstanceFinder.ClientManager.OnClientConnectionState += OnLocalClientConnectionState;
    }

    private void OnDestroy()
    {
        if (FishNet.InstanceFinder.ClientManager)
        {
            FishNet.InstanceFinder.ClientManager.OnClientConnectionState -= OnLocalClientConnectionState;
        }
    }


    void OnLocalClientConnectionState(ClientConnectionStateArgs _clientState)
    {
        switch (_clientState.ConnectionState)
        {
            /*
            case LocalConnectionState.Starting:
                break;
            
            case LocalConnectionState.Stopped:
                break;
                */

            case LocalConnectionState.Stopping:
                gameObject.SetActive(true);
                break;

            case LocalConnectionState.Started:
                gameObject.SetActive(false);
                break;
        }
    }
}