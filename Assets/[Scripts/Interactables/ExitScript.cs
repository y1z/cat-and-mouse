using System;
using System.Collections;
using System.Collections.Generic;
using FishNet;
using FishNet.Managing.Server;
using FishNet.Object;
using Managers;
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
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

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
        const string WINNER_SCENE = "You_Win_Scene";
        const string LOSER_SCENE = "You_Lose_Scene";
        if (other.CompareTag("Player"))
        {
            GameManager.instance.SetInit(false);
            bool isMouse = other.GetComponent<GeneralPlayer>().isMouse;

            #if UNITY_EDITOR
            Debug.Log(StringUtil.addColorToString($"isMouse = {isMouse}", Color.red));
            #endif
            if (!isMouse)
            {
                _loaderScript.sceneName = LOSER_SCENE; // LOSER_SCENE;
                _loaderScript.loadScene();
            }

            _loaderScript.sceneName = WINNER_SCENE;
            _loaderScript.loadScene();

            //var catPlayers = PlayerManager.instance.GetAllCatPlayers();
            //var mousePlayers = PlayerManager.instance.GetAllMousePlayers();
        }
    }
}