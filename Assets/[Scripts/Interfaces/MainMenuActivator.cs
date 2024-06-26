using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Transporting;
using UnityEngine;

public class MainMenuActivator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FishNet.InstanceFinder.ClientManager.OnClientConnectionState += OnLocalClientConnectionState;
    }

    private void OnDisable()
    {
    }

    private void OnDestroy()
    {
        FishNet.InstanceFinder.ClientManager.OnClientConnectionState -= OnLocalClientConnectionState;
    }

    void OnLocalClientConnectionState(ClientConnectionStateArgs _clientState)
    {
        switch (_clientState.ConnectionState)
        {
            
            case LocalConnectionState.Stopped:
                print("true");
                
                if( gameObject != null )
                {
                    gameObject.SetActive(true);
                }
                break;
            case LocalConnectionState.Started :
                
                print("false");
                if (gameObject != null)
                {
                    
                    gameObject.SetActive(false);
                }
                break;
        }

    }
}
