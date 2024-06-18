using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public sealed class PartidaUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nombrePartidaText;
    [SerializeField] private Image estadoServidorImage;


    private string ip;
    private string port;
    [FormerlySerializedAs("partidasManager")] public SessionManager sessionManager;

    public void Setup(PlayflowClientRequest.Server server, SessionManager pm)
    {
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
    }
}