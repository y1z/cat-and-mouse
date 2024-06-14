using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class PartidaUILoadScene : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nombrePartidaText;
    [SerializeField] private Image estadoServidorImage;
    [SerializeField] private SceneLoaderScript _loaderScript;


    private string ip;
    private string port;
    public PartidasManager partidasManager;
    
    public void Setup(PlayflowClientRequest.Server server, PartidasManager pm)
    {
        _loaderScript = GetComponent<SceneLoaderScript>();
        nombrePartidaText.SetText(server.match_id);
        switch (server.status)
        {
            case "running":
                estadoServidorImage.color = Color.green;
                break;
            default:
                estadoServidorImage.color = Color.yellow;
                break;
        }

        ip = server.ip;
        port = server.ports;
        partidasManager = pm;
    }


    public void Unir()
    {
        partidasManager.UnirPartida(ip, port);
        _loaderScript.sceneName = "Project_scene_mouse_test";
        _loaderScript.loadScene();
    }
}