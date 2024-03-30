using System;
using System.Collections;
using System.Collections.Generic;
using FishNet;
using UnityEngine;

/**
 *  Used to keep track of which role the player currently is and helps change the role when necessary 
 */
public sealed class RoleController : MonoBehaviour
{

    private RoleBase _roleBase;
    
   [SerializeField] private bool _isInitialized = false;

   [SerializeField] private GeneralPlayer _player_ref;
    

    void Update()
    {
        if (!_isInitialized)
        {
            return;
        }

        _roleBase.OnUpdate(_player_ref);

            if (Input.GetButtonDown("Fire1"))
            {
                _roleBase.DoRoleSpecialAction(_player_ref);
            }

    }
    
    public bool Initialize(GeneralPlayer player, RoleBase startingRoleBase)
    {
        _player_ref = player;
        _roleBase = startingRoleBase;
        _isInitialized = true;
        _roleBase.OnInit(player);
        return _isInitialized;
    }

    public void UnInitialize()
    {
        _roleBase = null;
        _player_ref = null;
        _isInitialized = false;
    }

    public string getRoleName()
    {
        return _roleBase.GetType().ToString();
    }


    public void SetRole(RoleBase newRoleBase)
    {
        _roleBase = newRoleBase;
    }
}
