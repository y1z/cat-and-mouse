    public class CatRole : IRole
    {
        public bool OnInit(GeneralPlayerCharacteristics player)
        {
           player.ChangeColor(Globals.DEFAULT_CAT_COLOR);
           return true;
        }
        

        public bool OnUpdate(GeneralPlayerCharacteristics player)
        {


            return true;
        }

        public void DoRoleSpecialAction(GeneralPlayerCharacteristics player)
        {
        }

        public bool OnEnd(GeneralPlayerCharacteristics player)
        {
            return true;
        }
    }