﻿    using FishNet.Demo.AdditiveScenes;
    using UnityEngine;

    public sealed class MouseRole : IRole
    {

        bool IRole.OnInit(GeneralPlayer player)
        {
            player.ChangeColor( Globals.DEFAULT_MOUSE_COLOR);
            return true;
        }

        bool IRole.OnUpdate(GeneralPlayer player)
        {


            return true;
        }

        void IRole.DoRoleSpecialAction(GeneralPlayer player)
        {
            PlayerDash pm = player.GetComponent<PlayerDash>();
            pm.Dash();
            Debug.Log("mouse Role");
        }

        bool IRole.OnEnd(GeneralPlayer player)
        {
            return true;
        }
    }