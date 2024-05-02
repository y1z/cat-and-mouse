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

    public bool IsInitialized { get; private set; }

    [SerializeField] private GeneralPlayer _player_ref;

    private void Start()
    {
        IsInitialized = false;
    }

    void Update()
    {
        if (!IsInitialized)
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
        IsInitialized = true;
        // initialized 
        bool isRoleInitalized = _roleBase.OnInit(player);
        return IsInitialized && isRoleInitalized;
    }

    public void UnInitialize()
    {
        _roleBase = null;
        _player_ref = null;
        IsInitialized = false;
    }

    public string getRoleName()
    {
        return _roleBase.GetType().ToString();
    }

    public RolePermissons Permissons => _roleBase.Permissons;

    public void SetRole(RoleBase newRoleBase)
    {
        _roleBase = newRoleBase;
    }
}