using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using PlayFlow;
using FishNet;
using FishNet.Managing;

public sealed class MatchMaking : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private Button BuscarBtn;

    public void BuscarPartida()
    {
        PlayFlowManager.PlayerData playerData = new PlayFlowManager.PlayerData();

        playerData.player_id = SystemInfo.deviceUniqueIdentifier;

#if UNITY_EDITOR
        playerData.player_id += Random.Range(100, 999);
#endif

        playerData.region = new List<string>() { "us-east", "us-west" };

        playerData.elo = 100; // skill

        playerData.game_type = "default";

        playerData.custom_parameters = new List<PlayFlowManager.CustomParameter>();
        PlayFlowManager.CustomParameter matchParameter = new PlayFlowManager.CustomParameter();
        matchParameter.key = "difficulty";
        matchParameter.value = "medium";
        playerData.custom_parameters.Add(matchParameter);
        StartCoroutine(PlayFlowManager.FindMatch(PlayflowClientRequest.Token, playerData, OnMatchFound));
    }


    private void OnMatchFound(Server server)
    {
        // Opcion 1: el Ui esta n la misma escena que el networManager
        NetworkManager nm = FishNet.InstanceFinder.NetworkManager;
        nm.TransportManager.Transport.SetClientAddress(server.ip);
        if (server.ports.TryGetValue("7770", out int nuevoPort))
        {
            nm.TransportManager.Transport.SetPort((ushort)nuevoPort);
        }

        nm.ClientManager.StartConnection();
    }
}