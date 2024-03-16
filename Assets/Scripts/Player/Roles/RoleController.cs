using System;
using System.Collections;
using System.Collections.Generic;
using FishNet;
using UnityEngine;

public sealed class RoleController : MonoBehaviour
{

    private IRole _role;
    // Start is called before the first frame update
    void Start()
    {
        _role.GetType();

    }

    // Update is called once per frame
    void Update()
    {
        
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
