using FishNet;
using FishNet.Connection;
using FishNet.Managing.Scened;
using FishNet.Managing.Logging;
using FishNet.Object;
using UnityEngine.SceneManagement;
using UnityEngine;

public sealed class SceneLoaderScriptFishNet : MonoBehaviour
{
    [Tooltip("The name of the scene to be loaded")]
    public string sceneName;


    /**
     * change the scene for all currently connected to the game.
     */
    public void LoadSceneGlobal()
    {
        SceneLoadData sld = new SceneLoadData(sceneName);
        InstanceFinder.SceneManager.LoadGlobalScenes(sld);
    }

    public void LoadSceneForConnections(NetworkConnection[] connections)
    {
        SceneLoadData sld = new SceneLoadData(sceneName);
        InstanceFinder.SceneManager.LoadConnectionScenes(sld);
    }
}