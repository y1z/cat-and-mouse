using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the role for player who have not been assigned a role 
/// </summary>
public sealed class UndecidedRole : RoleBase
{
    public override bool OnInit(GeneralPlayer player)
    {
        _rolePermissons = RolePermissons.NONE;
        return true;
    }

    public override bool OnUpdate(GeneralPlayer player)
    {
        return true;
    }

    public override void DoRoleSpecialAction(GeneralPlayer player)
    {
    }

    public override bool OnEnd(GeneralPlayer player)
    {
        return true;
    }
}