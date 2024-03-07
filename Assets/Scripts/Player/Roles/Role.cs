﻿namespace Player
{
    
    /**
     * Controls what roles each player take 
     */
    public abstract class Role
    {

        public abstract bool OnInit(GeneralPlayerCharacteristics player);
        /// <summary>
        /// What the role does every Update
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public abstract bool OnUpdate(GeneralPlayerCharacteristics player);
        
        /// <summary>
        /// Does action only the role can do 
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public abstract void DoRoleSpecialAction(GeneralPlayerCharacteristics player );
        /// <summary>
        /// What happen when the role ends 
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public abstract bool OnEnd(GeneralPlayerCharacteristics player);

    }
}