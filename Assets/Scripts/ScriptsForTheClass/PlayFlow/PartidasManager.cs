using UnityEngine;

public class PartidasManager : MonoBehaviour
{
    [SerializeField] private PlayflowClientRequest _playflowClientRequest;

    [SerializeField] private Transform servidoresContent;
    [SerializeField] private Transform servidoreUiPrefab;

    public void CrearPartida()
    {

        _playflowClientRequest.StartServer("us-west");
    }


    public void RefrescarListaPartidas()
    {
        _playflowClientRequest.GetServers( OnListaDeSererFetch );
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
        }
        
    }
    
}