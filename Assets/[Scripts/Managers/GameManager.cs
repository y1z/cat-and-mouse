using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using GameKit.Utilities;
using TMPro;
using UnityEngine;
using Utility;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;
using UnityEngine.SceneManagement;

namespace Managers
{
    /**
 * 
 */
    public sealed class GameManager : NetworkBehaviour
    {
        public static GameManager instance;
        public CollectableManager collectableManager;

        public PlayerManager playerManager;

        public float _howManyTimesPerSecond = 3.0f;

        public float initTime = 40.0f;

        private int _playerCount = 0;

        private bool _initalize_game = false;

        private Coroutine _coroutine;

        [SerializeField] private TextMeshProUGUI _textMeshPro;

        [SerializeField] private Transform[] spawnLocations;

        [SerializeField] private GameObject objectToDespawn;

        void Start()
        {
        }

        private void Update()
        {
            if (!_initalize_game)
                return;

            _textMeshPro.text = initTime.ToString("F");

            initTime = initTime - Time.deltaTime;

            if (initTime < float.Epsilon)
            {
                //SceneManager   SceneManager.name;
                String scene_name = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                UnityEngine.SceneManagement.SceneManager.LoadScene(scene_name);
            }
        }

        public override void OnStartServer()
        {
            base.OnStartServer();

            collectableManager = GetComponent<CollectableManager>();
            playerManager = GetComponent<PlayerManager>();
            _coroutine = StartCoroutine(UpdatePlayerCount(_howManyTimesPerSecond));
            Debug.Log("Started : " + nameof(GameManager), this);
        }


        IEnumerator UpdatePlayerCount(float howManyTimesPreSecond)
        {
            float inverse_time = 1.0f / howManyTimesPreSecond;
            WaitForSeconds timeToWait = new WaitForSeconds(inverse_time);
            while (true)
            {
                _playerCount = InstanceFinder.NetworkManager.ServerManager.Clients.Count;
#if UNITY_EDITOR
                Debug.Log(Utility.StringUtil.addColorToString($"player_count =" + _playerCount, Color.magenta), this);
#endif

                if (_playerCount > 2)
                {
                    yield return new WaitForSeconds(2.0f);
                    moveAllPlayer();

                    yield break;
                }

                yield return timeToWait;
            }

            yield return new WaitForSeconds(0.1f);
        }


        public GeneralPlayer[] getAllPlayers()
        {
            return GameObject.FindObjectsOfType<GeneralPlayer>();
        }

        [ObserversRpc(RunLocally = true)]
        public void moveAllPlayerRpc()
        {
            var players = getAllPlayers();
            players.Shuffle();
            //objectToDespawn.SetActive(false);
            int locationCount = spawnLocations.Length;
            foreach (var player in players)
            {
                int index = Random.Range(0, locationCount);

                player.transform.position = spawnLocations[index].position;
            }

            /*
            int locationCount = spawnLocations.Length;
            foreach (var player in players)
            {
                int index = Random.Range(0, locationCount);
                player.TeleportToLocation(spawnLocations[index].position);
            }*/
            objectToDespawn.SetActive(false);
            collectableManager.spawner.SpawnCollectables();

            for (int i = players.Length - 1; i > 0; --i)
            {
                players[i].InitMouseRole();
            }

            players[0].InitCatRole();

            _initalize_game = true;
        }

        [ServerRpc(RequireOwnership = false)]
        public void moveAllPlayer()
        {
            //Debug.Log(nameof(moveAllPlayer) ,this);
            moveAllPlayerRpc();
        }
    }
}