using FishNet.Object;
using UnityEngine;

    public class CameraController : NetworkBehaviour
    {
        public override void OnStartClient()
        {
            base.OnStartNetwork();
            if (!base.IsOwnereeeee)
            {
                return;
            }
        }
    }