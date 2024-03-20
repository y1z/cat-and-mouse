using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FishNet;
using FishNet.Object;
using FishNet.Connection;
using FishNet.Object.Synchronizing;
using UnityEngine;

namespace Sever
{
    /**
     * Keeps track of all relevant data of the players of the game 
     */
    public class PlayerManager : NetworkBehaviour
    {
        
        public static PlayerManager instance;
        

        // used to keep track of  stats for the players
        [SyncObject] 
        public readonly SyncDictionary<NetworkConnection, PlayerData> _players = new SyncDictionary<NetworkConnection, PlayerData>();


        private List<NetworkConnection> _toBeRemovedConnections = new List<NetworkConnection>();

        public void Awake()
        {
            instance = this;
            _players.OnChange += _players_OnChange;

        }

        private void Update()
        {
            if (!base.IsServer)
            {
                return;
            }


            foreach (var key_and_value in _players)
            {
                if (!key_and_value.Key.IsActive)
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

        }


        public void DamagePlayer(int player_id, int attacker_id, float damage)
        {
            if (!base.IsServer)
                return;

            // TODO : CHANGE DICTIONARY TO USE 'NetworkConnection ' instead of index
            //var a = base.NetworkObject.LocalConnection;
            

            /*
            PlayerData player_data = playersData[player_id];
            player_data.health -= damage;
            print("Player " + player_id.ToString() + " was damaged" + damage.ToString()  );

            if (player_data.health < 1)
            {
               PlayerKilled(player_id,attacker_id); 
            }
            */

        }


        // TODO : REMOVE FUNCTION
        private void PlayerKilled(int player_id, int attacker_id)
        {
            print("Player " + player_id.ToString() + " was killed by " + attacker_id.ToString());
        }
        
        //SyncDictionaries also include the asServer parameter.
        private void _players_OnChange(SyncDictionaryOperation op, NetworkConnection key, PlayerData value, bool asServer)
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
            break;
            //Clears the dictionary.
                case SyncDictionaryOperation.Clear:
            break;
            //Like SyncList, indicates all operations are complete.
                case SyncDictionaryOperation.Complete:
            break;
            }
            
        }


    }
}