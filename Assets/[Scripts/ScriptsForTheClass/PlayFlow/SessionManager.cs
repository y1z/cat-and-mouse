using System;
using FishNet.Managing;
using FishNet.Transporting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public sealed class SessionManager : MonoBehaviour
{
    [SerializeField] private PlayflowClientRequest _playflowClientRequest;

    [SerializeField] private Transform servidoresContent;
    [SerializeField] private GameObject servidoreUiPrefab;

    public SceneLoaderScript sceneLoader;

    [FormerlySerializedAs("cargandoGo")] [SerializeField] private GameObject loadingScreen;

    private void Start()
    {
        FishNet.InstanceFinder.ClientManager.OnClientConnectionState += OnLocalClientConnectionState;
        sceneLoader = GetComponent<SceneLoaderScript>();
    }

    void OnLocalClientConnectionState(ClientConnectionStateArgs _clientState)
    {
        
        
        switch (_clientState.ConnectionState)
        {
            /*
            case LocalConnectionState.Starting:
            case LocalConnectionState.Stopping:
                gameObject.SetActive(true);
                break;
                */


            case LocalConnectionState.Stopped:
            case LocalConnectionState.Started:
                //this.loadingScreen?.SetActive(false);
                break;
        }
    }


    public void CrearPartida()
    {
        _playflowClientRequest.StartServer("us-west");
    }


    public void RefrescarListaPartidas()
    {
        _playflowClientRequest.GetServers(OnServerListFetch);
    }

    void OnServerListFetch(PlayflowClientRequest.ServerList serverList)
    {

        for (int i = servidoresContent.childCount - 1; i >= 0; --i)
        {
            Destroy(servidoresContent.GetChild(i).gameObject);
        }
        

        foreach (PlayflowClientRequest.Server server in serverList.servers)
        {
            var serverUiGo = Instantiate(servidoreUiPrefab, servidoresContent);
            serverUiGo.GetComponent<PartidaUI>().Setup(server, this);
            //serverUiGo.transform.SetParent(servidoresContent);
        }
    }

    public void UnirPartida(string _ip, string _port)
    {
        //TurnOn
        GameObject[] gos = GameObject.FindGameObjectsWithTag("TurnOn");

        print(nameof(UnirPartida));
        foreach (var thing in gos)
        {
            thing.SetActive(true);
            print(thing.name);
            print("==============================================================");
        }
        
        // Opcion 1: el Ui esta n la misma escena que el networManager
        NetworkManager nm = FishNet.InstanceFinder.NetworkManager;
        nm.TransportManager.Transport.SetClientAddress(_ip);
        if (!string.IsNullOrWhiteSpace(_port))
        {
            nm.TransportManager.Transport.SetPort(ushort.Parse(_port));
        }

        nm.ClientManager.StartConnection();
        loadingScreen.SetActive(true);
        // TODO : Mostrar un cargando/conectando
//[Scenes/

        sceneLoader.loadScene();
        //sceneLoader.LoadSceneGlobal();
        //SceneManager.LoadScene();
    }
} 