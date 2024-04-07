using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script is used for loading new scenes
/// </summary>
public class SceneLoaderScript : MonoBehaviour
{

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
