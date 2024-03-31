using System;
using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;
using Object = System.Object;

namespace Collectables
{
    public sealed class CollectableSpawner : NetworkBehaviour
    {
        
        [Header("Spawner variables")]
        [SerializeField] private GameObject _prefab;
        [SerializeField] private List<Vector3> _SpawnLocations;
        [Tooltip("This is the transform that will be the parent for all object collectables")]
        [SerializeField] private Transform _parentTransform;

        private List<GameObject> _spawnedObjects = new List<GameObject>();

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
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                DespawnAllCollectables();
            }
        }


        [ServerRpc(RequireOwnership = false)]
        public void SpawnCollectables()
        {
            SpawnCollectablesRPC();
        }

        [ObserversRpc()]
        private void SpawnCollectablesRPC()
        {
            var serverManager = InstanceFinder.ServerManager;
            foreach (var location in _SpawnLocations)
            {
              GameObject spawn_object = Instantiate(_prefab, location, Quaternion.identity, _parentTransform);
              _spawnedObjects.Add(spawn_object);
              serverManager.Spawn(spawn_object);
            }
           CollectableManager.instance.GetEveryCollectable(); 
            
        }


        [ServerRpc(RequireOwnership = false)]
        public void DespawnAllCollectables()
        {
            DespawnAllCollectablesRpc();
        }

        [ObserversRpc]
        private void DespawnAllCollectablesRpc()
        {
           var serverManager = InstanceFinder.ServerManager; 
            foreach (var objs in _spawnedObjects)
            {
                serverManager.Despawn(objs);
            }
            
            _spawnedObjects.Clear();
        }

        [ServerRpc(RequireOwnership = false)]
        public void DespawnCollectable(NetworkObject object_to_despawn)
        {
           var serverManager = InstanceFinder.ServerManager;
           GameObject reference_to_object = null;
           //object_to_despawn.gameObject;
           foreach (var obj in _spawnedObjects)
           {
               if (obj == object_to_despawn)
               {
                   reference_to_object = obj;
                   break;
               }
           }

           serverManager.Despawn(object_to_despawn);
           if (reference_to_object != null)
           {
           //_spawnedObjects.Remove(reference_to_object);
               
           Debug.Log("Function =" +nameof(DespawnCollectable));
           }

        }
        
        public override void OnStartServer()
        {
            base.OnStartServer();
        }
    }
}