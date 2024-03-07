using System;
using FishNet.Object;
using Unity.VisualScripting;
using UnityEngine;

namespace Player
{
    public sealed class MousePlayer : GeneralPlayerCharacteristics 
    {

        private new void Awake()
        {
        }


        private new void Start()
        {
            ChangeColor(Globals.Globals.DEFAULT_MOUSE_COLOR);
        }

    }
}