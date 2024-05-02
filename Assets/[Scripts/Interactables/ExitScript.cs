using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ExitScript : NetworkBehaviour
{
    private SceneLoaderScript _loaderScript;
    private Collider _collider = null;

    [Tooltip("how many time per second to update the Update Loop")] [Range(0.0f, 10.0f)] [SerializeField]
    private float _howManyTimesPerSecondUseUpdateLoop;

    void Awake()
    {
    }

    private void Start()
    {
        _loaderScript = GetComponent<SceneLoaderScript>();
        _collider = GetComponent<Collider>();
        Debug.Assert(_loaderScript != null, "_loaderScript != null");
        Debug.Assert(_collider != null, "_collider != null");

        if (_howManyTimesPerSecondUseUpdateLoop < float.Epsilon)
        {
            _howManyTimesPerSecondUseUpdateLoop = 3.0f;
        }

        float final_seconds = 1.0f / _howManyTimesPerSecondUseUpdateLoop;

        WaitForSeconds seconds = new WaitForSeconds(final_seconds);
        string msg = "Start Coroutine in :" + nameof(Start) + nameof(ExitScript);
        Debug.Log(Utility.StringUtil.addColorToString(msg, Color.yellow));
        StartCoroutine(UpdateLoop(seconds));
    }

    IEnumerator UpdateLoop(WaitForSeconds seconds)
    {
        while (true)
        {
            CollectableManager instance = CollectableManager.instance;
            bool areAllColectablesCollected = instance.IsEveryCollectableCollected();
            //bool has_less_than_one = instance.CollectableCount < 1;
            if (areAllColectablesCollected)
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _loaderScript.loadScene();
        }
    }
}