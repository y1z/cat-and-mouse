using System;
using System.Collections;
using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;
using Utility;

namespace Managers
{
    /**
 * 
 */
    public class GameManager : NetworkBehaviour
    {
        public CollectableManager collectableManager;

        public PlayerManager playerManager;

        public static GameManager instance;

        public float _howManyTimesPerSecond = 3.0f;
        private int _playerCount = 0;

        private bool _initalize_game = false;

        private Coroutine _coroutine;

        void Start()
        {
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            
            collectableManager = GetComponent<CollectableManager>();
            playerManager = GetComponent<PlayerManager>();
            _coroutine = StartCoroutine(UpdatePlayerCount(_howManyTimesPerSecond));
            Debug.Log("Started : " + nameof(GameManager),this);
        }


        IEnumerator UpdatePlayerCount(float howManyTimesPreSecond)
        {
            float inverse_time = 1.0f / howManyTimesPreSecond;
            WaitForSeconds timeToWait = new WaitForSeconds(inverse_time);
            while (true)
            {
                // _playerCount = InstanceFinder.NetworkManager.ServerManager.Clients.Count;
                Debug.Log(Utility.StringUtil.addColorToString(inverse_time.ToString(), Color.magenta), this);
                yield return timeToWait;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}