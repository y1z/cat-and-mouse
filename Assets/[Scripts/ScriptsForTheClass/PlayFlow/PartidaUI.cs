using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartidaUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nombrePartidaText;
    [SerializeField] private Image estadoServidorImage;


    private string ip;
    private string port;
    public PartidasManager pm;

    public void Setup(PlayflowClientRequest.Server server, PartidasManager pm)
    {
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
    }


    public void Unir()
    {
        pm.UnirPartida(ip, port);
    }
}