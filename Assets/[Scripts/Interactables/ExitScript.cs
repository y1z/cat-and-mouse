using System;
using System.Collections;
using System.Collections.Generic;
using FishNet;
using FishNet.Managing.Server;
using FishNet.Object;
using Unity.VisualScripting;
using UnityEngine;
using StringUtil = Utility.StringUtil;

[RequireComponent(typeof(Collider))]
public sealed class ExitScript : NetworkBehaviour
{
    private SceneLoaderScript _loaderScript;
    private Collider _collider = null;

    [Tooltip("how many time per second to update the Update Loop")] [Range(0.0f, 10.0f)] [SerializeField]
    private float _howManyTimesPerSecondUseUpdateLoop;

    // ReSharper disable Unity.PerformanceAnalysis
    public override void OnStartServer()
    {
        base.OnStartServer();

        _loaderScript = GetComponent<SceneLoaderScript>();
        _collider = GetComponent<Collider>();
        Debug.Assert(_loaderScript != null, "_loaderScript != null");
        Debug.Assert(_collider != null, "_collider != null");

        _collider.isTrigger = false;

        if (_howManyTimesPerSecondUseUpdateLoop < float.Epsilon)
        {
            _howManyTimesPerSecondUseUpdateLoop = 3.0f;
        }

        float final_seconds = 1.0f / _howManyTimesPerSecondUseUpdateLoop;

        string msg = "Start Coroutine in :" + nameof(OnStartServer) + nameof(ExitScript);
        Debug.Log(StringUtil.addColorToString(msg, Color.yellow));
        Debug.Log(StringUtil.addColorToString("waiting for this many second=" + final_seconds, Color.yellow));

        StartCoroutine(UpdateLoop(final_seconds));
    }


    IEnumerator UpdateLoop(float seconds)
    {
        WaitForSeconds delay = new WaitForSeconds(seconds);
        while (true)
        {
            CollectableManager instance = CollectableManager.instance;
            var CollectableCount = instance.CollectableCount;
            bool has_less_than_one = CollectableCount < 1;
            Debug.Log(StringUtil.addColorToString("collectableCount =" + CollectableCount.ToString(), Color.green));

            _collider.isTrigger = has_less_than_one;

            yield return delay;
        }

        yield return delay;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<NetworkBehaviour>().NetworkManager.ClientManager.Connection.Disconnect(false);
            //InstanceFinder.ClientManager.
            //_loaderScript.loadScene();
        }
    }
}