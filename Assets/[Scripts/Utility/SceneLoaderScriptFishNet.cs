using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Managing.Scened;
using FishNet.Managing.Logging;
using FishNet.Object;
using UnityEngine.SceneManagement;
using UnityEngine;
/**
 * Script for loading scene using the FishNet API.
 */
public sealed class SceneLoaderScriptFishNet : MonoBehaviour
{
    [Tooltip("The name of the scene to be loaded")]
    public string sceneName;

    /// <summary>
    /// change the scene for all currently connected to the game.
    /// </summary>
    public void LoadSceneGlobal()
    {
        SceneLoadData sld = new SceneLoadData(sceneName);
        sld.ReplaceScenes = ReplaceOption.All;
        InstanceFinder.SceneManager.LoadGlobalScenes(sld);
    }

    
    /// <summary>
    /// Load a scene for an individual networkObject
    /// </summary>
    /// <param name="objects"></param>
    public void LoadSceneForObject(NetworkObject[] objects)
    {
        SceneLoadData sld = new SceneLoadData(sceneName);
        sld.ReplaceScenes = ReplaceOption.All;
        sld.MovedNetworkObjects = objects;
        InstanceFinder.SceneManager.LoadConnectionScenes(sld);
    }
}