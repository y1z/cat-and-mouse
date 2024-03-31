using FishNet.Object;
using UnityEngine;

/// <summary>
/// The Interface for all collectable item in the game
/// </summary>
public abstract class CollectableBase : NetworkBehaviour 
{
   public bool isCollected = false;

   public virtual void Collect(GeneralPlayer player)
   {
      isCollected = true;
   }

}
