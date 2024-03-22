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
            //throw new System.NotImplementedException();
        }

         bool IRole.OnEnd(GeneralPlayer player)
        {
            return true;
        }
    }