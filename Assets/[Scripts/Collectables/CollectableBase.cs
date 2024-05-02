using System.IO;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

/// <summary>
/// The Interface for all collectable item in the game
/// </summary>
public abstract class CollectableBase : NetworkBehaviour
{
    [SyncVar(OnChange = nameof(OnChange_IsCollected))]
    public bool isCollected = false;

    [ServerRpc(RequireOwnership = false)]
    public void Collect(GeneralPlayer player)
    {
        _CollectRpc();
    }

    [ObserversRpc]
    private void _CollectRpc()
    {
        isCollected = true;
    }

    private void OnChange_IsCollected(bool prev, bool next, bool asServer)
    {
        Debug.Log("Called " + nameof(OnChange_IsCollected) + "\nIn class " + nameof(CollectableBase));
    }
}