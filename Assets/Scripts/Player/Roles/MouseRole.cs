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
            Rigidbody rigidbody = player.GetComponent<Rigidbody>();
            Debug.Log(rigidbody.position);
        }

        bool IRole.OnEnd(GeneralPlayer player)
        {
            return true;
        }
    }