using System;
using System.Collections;
using System.Collections.Generic;
using FishNet;
using FishNet.Object;
using FishNet.Connection;
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


        public void Awake()
        {
            instance = this;
        }
        

        public void DamagePlayer(int player_id, int attacker_id, float damage)
        {
            if (!base.IsServer)
                return;

            PlayerData player_data = playersData[player_id];
            player_data.health -= damage;
            print("Player " + player_id.ToString() + " was damaged" + damage.ToString()  );

            if (player_data.health < 1)
            {
               PlayerKilled(player_id,attacker_id); 
            }

        }


        private void PlayerKilled(int player_id, int attacker_id)
        {
            print("Player " + player_id.ToString() + " was killed by " + attacker_id.ToString());
        }


    }
}