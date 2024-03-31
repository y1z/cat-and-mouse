using System;
using System.Collections.Generic;
using FishNet;
using FishNet.Object;
using UnityEngine;

namespace Collectables
{
    public sealed class CollectableSpawner : NetworkBehaviour
    {
        
        [Header("Spawner variables")]
        [SerializeField] private GameObject _prefab;
        [SerializeField] private List<Vector3> _SpawnLocations;
        [Tooltip("This is the transform that will be the parent for all object collectables")]
        [SerializeField] private Transform _parentTransform;

        private void Start()
        {
           //SpawnCollectables(); 
        }

        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.Y))
            {
                SpawnCollectables();
            }
        }


        [ServerRpc(RequireOwnership = false)]
        public void SpawnCollectables()
        {
            
            var serverManager = InstanceFinder.ServerManager;
            foreach (var location in _SpawnLocations)
            {
              GameObject spawn_object = Instantiate(_prefab, location, Quaternion.identity, _parentTransform);
              serverManager.Spawn(spawn_object);
            }
            //SpawnCollectablesRPC();
        }

        [ObserversRpc(RunLocally = true)]
        private void SpawnCollectablesRPC()
        {
            
        }
        
        
        public override void OnStartServer()
        {
            base.OnStartServer();
        }
    }
}