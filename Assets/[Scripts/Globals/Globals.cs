﻿using UnityEngine;

public static class Globals
{
    public static readonly Color DEFAULT_MOUSE_COLOR = Color.yellow;
    
    public static readonly Color DEFAULT_CAT_COLOR = Color.blue;
    
    public static readonly float DEFAULT_MAX_INITIAL_DISTANCE_FROM_SPAWN = 4.0F;
    
    public static readonly LayerMask PLAYER_MASK = LayerMask.GetMask("Player");

    
    public const float DEFAULT_PLAYER_HEALTH = 100.0f;
    
    public const float INVERSE_DEFAULT_PLAYER_HEALTH = 1.0f / DEFAULT_PLAYER_HEALTH; 

    public const float DEFAULT_PLAYER_DAMAGE = DEFAULT_PLAYER_HEALTH * .25f;

    public const int DEFAULT_MINIMUM_PLAYERS = 3;
}