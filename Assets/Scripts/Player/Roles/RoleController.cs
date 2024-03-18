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

    private IRole _role;
    
   [SerializeField] private bool _isInitialized = false;

   [SerializeField] private GeneralPlayerCharacteristics _player_ref;
    

    void Update()
    {
        if (!_isInitialized)
        {
            return;
        }

        _role.OnUpdate(_player_ref);

            if (Input.GetKeyDown(KeyCode.F))
            {
                _role.DoRoleSpecialAction(_player_ref);
            }

    }
    
    public bool Initialize(GeneralPlayerCharacteristics player, IRole starting_role)
    {
        _player_ref = player;
        _role = starting_role;
        _isInitialized = true;
        _role.OnInit(player);
        return _isInitialized;
    }

    public void UnInitialize()
    {
        _role = null;
        _player_ref = null;
        _isInitialized = false;
    }

    public string getRoleName()
    {
        return _role.GetType().ToString();
    }


    public void SetRole(IRole new_role)
    {
        _role = new_role;
    }
}
