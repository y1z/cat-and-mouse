    using FishNet.Demo.AdditiveScenes;
    using UnityEngine;

    public sealed class MouseRole : RoleBase
    {
        public override bool OnInit(GeneralPlayer player)
        {
            player.ChangeColor( Globals.DEFAULT_MOUSE_COLOR);
            _rolePermissons = RolePermissons.CAN_COLLECT_CHEESE;
            return true;
        }

        public override bool OnUpdate(GeneralPlayer player)
        {


            return true;
        }

        public override void DoRoleSpecialAction(GeneralPlayer player)
        {
            PlayerDash pm = player.GetComponent<PlayerDash>();
            pm.Dash();
            Debug.Log("mouse Role");
        }

        public override bool OnEnd(GeneralPlayer player)
        {
            return true;
        }
    }