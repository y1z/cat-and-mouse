    public sealed class MouseRole: IRole
    {

        bool IRole.OnInit(GeneralPlayerCharacteristics player)
        {
            player.ChangeColor( Globals.DEFAULT_MOUSE_COLOR);
            return true;
        }

         bool IRole.OnUpdate(GeneralPlayerCharacteristics player)
        {


            return true;
        }

         void IRole.DoRoleSpecialAction(GeneralPlayerCharacteristics player)
        {
            //throw new System.NotImplementedException();
        }

         bool IRole.OnEnd(GeneralPlayerCharacteristics player)
        {
            return true;
        }
    }