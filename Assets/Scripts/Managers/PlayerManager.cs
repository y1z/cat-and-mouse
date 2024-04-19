using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FishNet;
using FishNet.Object;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Managing.Client;
using FishNet.Object.Synchronizing;
using UnityEngine;

// server 
namespace Managers
{
    /**
     * Keeps track of all relevant data of the players of the game 
     */
    public class PlayerManager : NetworkBehaviour
    {
        public static PlayerManager instance;

        // used to keep track of  stats for the players
        [SyncObject]
        public readonly SyncDictionary<NetworkConnection, PlayerData> _players =
            new SyncDictionary<NetworkConnection, PlayerData>();

        //contains 
        private List<NetworkConnection> _toBeRemovedConnections = new List<NetworkConnection>();

        private ClientManager _clientManager;

        private WaitForSecondsRealtime _secondsRealtime;

        public void Awake()
        {
            instance = this;
            _players.OnChange += _players_OnChange;

            _secondsRealtime = new WaitForSecondsRealtime(1.0f);

            _clientManager = InstanceFinder.ClientManager;
            //StartCoroutine(checkIfPlayersAreStillConnected(_secondsRealtime));
        }


        private void Update()
        {
            if (!base.IsServer)
            {
                return;
            }

#if UNITY_EDITOR
            var clinet_count = _clientManager.Clients.Count;
            if (clinet_count > 1)
            {
                string all_ids = "";
                foreach (var client in _clientManager.Clients)
                {
                    all_ids += client.Key.ToString() + ",";
                }

                print(all_ids);
            }
#endif
        }

        IEnumerator checkIfPlayersAreStillConnected(WaitForSecondsRealtime seconds)
        {
            if (!base.IsServer)
                yield break;

            while (true)
            {
                foreach (var key_and_value in _players)
                {
                    if (_clientManager.Clients.ContainsValue(key_and_value.Key))
                    {
                        continue;
                    }

                    _toBeRemovedConnections.Add(key_and_value.Key);
                }

                if (_toBeRemovedConnections.Count > 0)
                {
                    foreach (var connection in _toBeRemovedConnections)
                    {
                        _players.Remove(connection);
                    }

                    _toBeRemovedConnections.Clear();
                }

                yield return seconds;
            }
        }


        /// <summary>
        /// Called with one player damaged another 
        /// </summary>
        /// <param name="attacking_player"> The player doing the damage</param>
        /// <param name="victim_player"> the player receiving the damage</param>
        public void PlayerDamage(GeneralPlayer attacking_player, GeneralPlayer victim_player, float damage_amount)
        {
            if (!base.IsServer)
                return;

            foreach (var network_connection in _players.Keys)
            {
                if (network_connection == victim_player.Connection)
                {
                    //victim_player.takeDamage();


                    break;
                }
            }


            //attacking_player;
        }

        /// <summary>
        /// function that is called when _players variable is modified 
        /// https://fish-networking.gitbook.io/docs/manual/guides/synchronizing/attributes#syncvar
        /// </summary>
        private void _players_OnChange(SyncDictionaryOperation op, NetworkConnection key, PlayerData value,
            bool asServer)
        {
            switch (op)
            {
                //Adds key with value.
                case SyncDictionaryOperation.Add:

                    print("add op\nkey =" + key.ClientId + "\nvalue = " + value);
                    break;
                //Removes key.
                case SyncDictionaryOperation.Remove:
                    print("remove total obj = " + _players.Count);
                    break;
                //Sets key to a new value.
                case SyncDictionaryOperation.Set:
                    print("new key set =" + key);
                    print("new value set =" + value.health);
                    break;
                //Clears the dictionary.
                case SyncDictionaryOperation.Clear:
                    print("Dictionary cleared");
                    break;
                //Like SyncList, indicates all operations are complete.
                case SyncDictionaryOperation.Complete:

                    print("Dictionary Complete");
                    break;
            }
        }

        /// <summary>
        /// Finds what the servers registers as the players health 
        /// </summary>
        /// <param name="player_connection"></param>
        /// <returns></returns>
        public float FindPlayerHealth(NetworkConnection player_connection)
        {
            float result = 0.0f;
            var player = FindPlayer(player_connection);
            if (player.Item2 != null)
            {
                result = player.Item2.health;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player_connection"></param>
        /// <returns>A pair that contains the player networkconnection and data</returns>
        public (NetworkConnection, PlayerData) FindPlayer(NetworkConnection player_connection)
        {
            NetworkConnection nc = null;
            PlayerData player_data = null;

            foreach (var player in _players)
            {
                if (player.Key == player_connection)
                {
                    nc = player.Key;
                    player_data = player.Value;
                }
            }

            return (nc, player_data);
        }

        [ServerRpc(RequireOwnership = false)]
        public void AddHealthToPlayer(NetworkConnection player_connection, float health_to_add)
        {
            var player = FindPlayer(player_connection);
            player.Item2.health += health_to_add;
            _players[player.Item1] = new PlayerData() { health = player.Item2.health };
        }

        public void SetPlayerHealth(GeneralPlayer player, float health)
        {
            var player_dict = FindPlayer(player.Connection);

            player_dict.Item2.health = health;
            player.health = player_dict.Item2.health;
        }
    }
}