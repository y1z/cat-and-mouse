using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForCommandAndLoadScene : MonoBehaviour
{
    [SerializeField]
    private SceneLoaderScript _loaderScript;

    public KeyCode _Code;
    // Start is called before the first frame update
    void Start()
    {
        _loaderScript = GetComponent < SceneLoaderScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(_Code))
        {
            _loaderScript.loadScene();
        }
        
    }
}
