using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using util = Utility;

/**
 * Communicates with the server to start play sessions
 */
public sealed class PlayflowClientRequest : MonoBehaviour
{
    private const string ApiUrl = "https://api.cloud.playflow.app/";
    public const string Token = "ddcb7e6fa32cf4a7cfcc78883a517bdd";
    private const string Version = "2";


    public async Task<string> StartServer(string region, string arguments = "", string ssl = "false")
    {
        string result = "";

        string url = $"{ApiUrl}start_game_server";

        using var client = new HttpClient();
        using var formData = new MultipartFormDataContent();

        formData.Headers.Add("token", Token);
        formData.Headers.Add("region", region);
        formData.Headers.Add("version", Version);
        formData.Headers.Add("arguments", arguments);
        formData.Headers.Add("ssl", ssl);

        var response = await client.PostAsync(url, formData);
        if (!response.IsSuccessStatusCode)
        {
            Debug.Log(await response.Content.ReadAsStringAsync());
        }

        return await response.Content.ReadAsStringAsync();
    }


    public async void GetServers(Action<ServerList> callback )
    {
        string responds = await GetServersAsync();

        ServerList serverlist = JsonUtility.FromJson<ServerList>(responds);

        util.EDebug.ColorLog("Cantidad de servidores " + serverlist.total_servers, Color.magenta);
        
        util.EDebug.ColorLog("servidores: " + serverlist.servers.Length, Color.magenta);
        //Debug.Log("Cantidad de servidores " + serverlist.total_servers);

        if (serverlist.total_servers > 0)
        {
            Debug.Log("Match id " + serverlist.servers[0].match_id);
            Debug.Log("IP " + serverlist.servers[0].ip);
            Debug.Log("Port " + serverlist.servers[0].ports);
            Debug.Log("status " + serverlist.servers[0].status);
        }
        
        callback?.Invoke(serverlist);
    }


    async Task<string> GetServersAsync()
    {
        string url = $"{ApiUrl}list_servers";

        using var client = new HttpClient();
        using var formData = new MultipartFormDataContent();

        formData.Headers.Add("token", Token);
        formData.Headers.Add("version", Version);

        formData.Headers.Add("includelaunchingservers", "true");
        ;

        var response = await client.PostAsync(url, formData);
        if (!response.IsSuccessStatusCode)
        {
            Debug.Log(await response.Content.ReadAsStringAsync());
        }

        return await response.Content.ReadAsStringAsync();
    }
    

[System.Serializable]
public class Server
{
    public string match_id;
    public string status;
    public string region;
    public string instance_type;
    public int server_version;
    public string server_arguments;
    public bool ssl_enabled; // bool puede ser tipo string al leer jsons
    public string ip;
    public string start_time;
    public string ports; // no sabemos que , asi que usamos string
}

[System.Serializable]
public class ServerList
{
    public int total_servers;
    public Server[] servers;
}
    
    
}