using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script is used for loading new scenes
/// </summary>
public sealed class SceneLoaderScript : MonoBehaviour
{
    [Tooltip("The name of the scene to be loaded")]
    public string sceneName;

    // Update is called once per frame
    void Update()
    {
    }

    // scene 
    public void loadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}