using UnityEngine;

public enum RolePermissons : byte
{
    NONE = 0b00_00_00_00,
    ATTACK_OTHER_PLAYERS = 0b00_00_00_01,
    CAN_COLLECT_CHEESE = 0b00_00_00_10,
    ALL_PERMISSONS = ATTACK_OTHER_PLAYERS | CAN_COLLECT_CHEESE,
}

/**
     * Controls what roles each player take 
     */
public abstract class RoleBase
{
    protected RolePermissons _rolePermissons = RolePermissons.NONE;

    /// <summary>
    /// Does anything necessary for the role to be initialized
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public abstract bool OnInit(GeneralPlayer player);

    /// <summary>
    /// What the role does every Update
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public abstract bool OnUpdate(GeneralPlayer player);

    /// <summary>
    /// Does action only the role can do 
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public abstract void DoRoleSpecialAction(GeneralPlayer player);

    /// <summary>
    /// What happen when the role ends 
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public abstract bool OnEnd(GeneralPlayer player);

    /// <summary>
    /// Returns what the current role of the player allows them to do 
    /// </summary>
    public RolePermissons Permissons => _rolePermissons;
}