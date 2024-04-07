using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ExitScript : MonoBehaviour
{
    private SceneLoaderScript _loaderScript;
    private Coroutine _coroutine = null;
    private Collider _collider = null;
    
    [Tooltip("how many time per second to update the Update Loop")]
    [SerializeField] private float _howManyTimesPerSecondUseUpdateLoop;
    
    void Awake()
    {
    }

    private void Start()
    {
        _loaderScript = GetComponent<SceneLoaderScript>();
        _collider = GetComponent<Collider>();

        if (_howManyTimesPerSecondUseUpdateLoop < float.Epsilon)
        {
            _howManyTimesPerSecondUseUpdateLoop = 3.0f;
        }
        
        float final_seconds = 1.0f / _howManyTimesPerSecondUseUpdateLoop;
        
        WaitForSeconds seconds = new WaitForSeconds(final_seconds);
        _coroutine = StartCoroutine(UpdateLoop(seconds));
        
    }

    IEnumerator UpdateLoop(WaitForSeconds seconds)
    {
        while (true)
        {
            CollectableManager instance = CollectableManager.instance;
            bool has_less_than_one = instance.CollectableCount < 1;
            if (has_less_than_one)
            {
                _collider.isTrigger = true;
            }
            else
            {
                _collider.isTrigger = false;
            }

            yield return seconds;
        }
        
        
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
