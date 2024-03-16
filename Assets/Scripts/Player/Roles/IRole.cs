    /**
     * Controls what roles each player take 
     */
    public interface IRole
    {

        /// <summary>
        /// Does anything necessary for the role to be initialized
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool OnInit(GeneralPlayerCharacteristics player);
        /// <summary>
        /// What the role does every Update
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool OnUpdate(GeneralPlayerCharacteristics player);
        
        /// <summary>
        /// Does action only the role can do 
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public void DoRoleSpecialAction(GeneralPlayerCharacteristics player );
        /// <summary>
        /// What happen when the role ends 
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool OnEnd(GeneralPlayerCharacteristics player);

    }