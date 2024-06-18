using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public sealed class PartidaUILoadScene : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nombrePartidaText;
    [SerializeField] private Image estadoServidorImage;
    [SerializeField] private SceneLoaderScript _loaderScript;


    private string ip;
    private string port;
    [FormerlySerializedAs("partidasManager")] public SessionManager sessionManager;
    
    public void Setup(PlayflowClientRequest.Server server, SessionManager pm)
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
        sessionManager = pm;
    }


    public void Unir()
    {
        sessionManager.UnirPartida(ip, port);
        _loaderScript.sceneName = "Project_scene_mouse_test";
        _loaderScript.loadScene();
    }
}