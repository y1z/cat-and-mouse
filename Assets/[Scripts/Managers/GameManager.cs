using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using GameKit.Utilities;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    /**
     * manages the state of the game
     */
    public sealed class GameManager : NetworkBehaviour
    {
        public static GameManager instance;

        public CollectableManager collectableManager;

        public PlayerManager playerManager;

        public float _howManyTimesPerSecond = 3.0f;

        [SyncVar]
        public float initTime = 40.0f;

        private int _playerCount = 0;

        private bool _initalize_game = false;

        private Coroutine _coroutine;

        [SerializeField] private TextMeshProUGUI _textMeshPro;

        [SerializeField] private Transform[] spawnLocations;

        [SerializeField] private GameObject objectToDespawn;

        [SerializeField] private bool isBeingTested = false;

        private void Awake()
        {
            instance = this;
        }
        
        void Start()
        {
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            collectableManager = GetComponent<CollectableManager>();
            playerManager = GetComponent<PlayerManager>();
            _coroutine = StartCoroutine(UpdatePlayerCount(_howManyTimesPerSecond));
            Debug.Log("Started : " + nameof(GameManager), this);
        }

        private void Update()
        {
            if (!_initalize_game && !isBeingTested)
                return;

            //Utility.EDebug.Log($" {nameof(GameManager)}",this);
            _textMeshPro.text = initTime.ToString("F");


            if (initTime < float.Epsilon)
            {
                EndGame(true);
            }

            if (base.IsServer)
            {
                initTime = initTime - Time.deltaTime;
            }
            
        }

        public void EndGame(bool did_cats_win)
        {
            const string WINNER_SCENE = "You_Win_Scene";
            const string LOSER_SCENE = "You_Lose_Scene";
            var catPlayers = PlayerManager.instance.GetAllCatPlayers();
            var mousePlayers = PlayerManager.instance.GetAllMousePlayers();
            if (did_cats_win)
            {
                foreach (var cat in catPlayers)
                {
                    cat.GetComponent<GeneralPlayer>().LoadSceneFromName(WINNER_SCENE);
                }

                foreach (var mousePlayer in mousePlayers)
                {
                    mousePlayer.GetComponent<GeneralPlayer>().LoadSceneFromName(LOSER_SCENE);
                }
            }
            else
            {
                foreach (var cat in catPlayers)
                {
                    cat.GetComponent<GeneralPlayer>().LoadSceneFromName(LOSER_SCENE);
                }

                foreach (var mousePlayer in mousePlayers)
                {
                    mousePlayer.GetComponent<GeneralPlayer>().LoadSceneFromName(WINNER_SCENE);
                }
            }
        }



        IEnumerator UpdatePlayerCount(float howManyTimesPreSecond)
        {
            float inverse_time = 1.0f / howManyTimesPreSecond;
            WaitForSeconds timeToWait = new WaitForSeconds(inverse_time);
            while (true)
            {
                _playerCount = InstanceFinder.ClientManager.Clients.Count;// InstanceFinder.NetworkManager.ServerManager.Clients.Count;
                
#if UNITY_EDITOR
                {
                    string debugMsg =
                        Utility.StringUtil.addColorToString($"player_count =" + _playerCount, Color.magenta);
                    Debug.Log(debugMsg, this);
                }
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
            //collectableManager.spawner.SpawnCollectables();

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

        [ServerRpc(RequireOwnership = false)]
        private void UpdateGameLoop()
        {
            UpdateGameLoopRFC();
        }

        [ObserversRpc(RunLocally = true)]
        private void UpdateGameLoopRFC()
        {
            _textMeshPro.text = initTime.ToString("F");

            initTime = initTime - Time.deltaTime;
        }

        public void SetInit(bool isInit)
        {
            _initalize_game = isInit;
        }
    }
}