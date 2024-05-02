using System;
using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;
using Utility;

namespace Collectables
{
    public sealed class CollectableSpawner : NetworkBehaviour
    {
        [Header("Spawner variables")] [SerializeField]
        private GameObject _prefab;

        [SerializeField] private List<Transform> _SpawnLocations;

        [Tooltip("This is the transform that will be the parent for all object collectables")] [SerializeField]
        private Transform _parentTransform;

        private List<GameObject> _spawnedObjects = new List<GameObject>();

        public int DespawnedCollectablesCount { get; private set; }

        private void Start()
        {
            DespawnedCollectablesCount = 0;
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
                GameObject spawn_object =
                    Instantiate(_prefab, location.position, Quaternion.identity, _parentTransform);
                _spawnedObjects.Add(spawn_object);
                serverManager.Spawn(spawn_object);
            }
            //CollectableManager.instance.GetEveryCollectable(); 
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
            DespawnedCollectablesCount = _spawnedObjects.Count;
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

            serverManager.Despawn(object_to_despawn);
            bool is_obj_remove = _spawnedObjects.Remove(object_to_despawn.gameObject);

#if UNITY_EDITOR
            if (!is_obj_remove)
            {
                string message = StringUtil.addColorToString("Object was not removed", Color.red);
                Debug.Log(message);
            }
#endif

            DespawnedCollectablesCount += 1;
        }

        /// <summary>
        /// Returns how many objects are spawned in
        /// </summary>
        public int CollectableCount => _spawnedObjects.Count;
    }
}