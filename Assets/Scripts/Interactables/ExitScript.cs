using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ExitScript : MonoBehaviour
{
    private SceneLoaderScript _loaderScript;
    
    // Start is called before the first frame update
    void Awake()
    {
        _loaderScript = GetComponent<SceneLoaderScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           _loaderScript.loadScene();
        }
    }
}
