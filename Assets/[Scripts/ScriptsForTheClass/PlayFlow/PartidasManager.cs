using System;
using FishNet.Managing;
using FishNet.Transporting;
using UnityEngine;

public sealed class PartidasManager : MonoBehaviour
{
    [SerializeField] private PlayflowClientRequest _playflowClientRequest;

    [SerializeField] private Transform servidoresContent;
    [SerializeField] private Transform servidoreUiPrefab;

    [SerializeField] private GameObject cargandoGo;

    private void Start()
    {
        FishNet.InstanceFinder.ClientManager.OnClientConnectionState += OnLocalClientConnectionState;
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
                this.cargandoGo.SetActive(false);
                break;
        }
    }


    public void CrearPartida()
    {
        _playflowClientRequest.StartServer("us-west");
    }


    public void RefrescarListaPartidas()
    {
        _playflowClientRequest.GetServers(OnListaDeSererFetch);
    }

    void OnListaDeSererFetch(PlayflowClientRequest.ServerList serverList)
    {
        for (int i = servidoresContent.childCount - 1; i >= 0; --i)
        {
            Destroy(servidoresContent.GetChild(i).gameObject);
        }


        foreach (var server in serverList.servers)
        {
            var serverUiGo = Instantiate(servidoreUiPrefab);
            serverUiGo.GetComponent<PartidaUI>().Setup(server, this);
        }
    }

    public void UnirPartida(string _ip, string _port)
    {
        // Opcion 1: el Ui esta n la misma escena que el networManager
        NetworkManager nm = FishNet.InstanceFinder.NetworkManager;
        nm.TransportManager.Transport.SetClientAddress(_ip);
        if (!string.IsNullOrWhiteSpace(_port))
        {
            nm.TransportManager.Transport.SetPort(ushort.Parse(_port));
        }

        nm.ClientManager.StartConnection();
        // TODO : Mostrar un cargando/conectando
    }
}