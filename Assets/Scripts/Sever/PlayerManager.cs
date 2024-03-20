using System;
using System.Collections;
using System.Collections.Generic;
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
        
        // keeps track 
        public Dictionary<int, PlayerData> playersData = new Dictionary<int, PlayerData>();

        [SyncObject] 
        public readonly SyncDictionary<NetworkConnection, PlayerData> _players = new SyncDictionary<NetworkConnection, PlayerData>();

        public void Awake()
        {
            instance = this;
            _players.OnChange += _players_OnChange;

        }
        

        public void DamagePlayer(int player_id, int attacker_id, float damage)
        {
            if (!base.IsServer)
                return;

            // TODO : CHANGE DICTIONARY TO USE 'NetworkConnection ' instead of index
            //var a = base.NetworkObject.LocalConnection;

            PlayerData player_data = playersData[player_id];
            player_data.health -= damage;
            print("Player " + player_id.ToString() + " was damaged" + damage.ToString()  );

            if (player_data.health < 1)
            {
               PlayerKilled(player_id,attacker_id); 
            }

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