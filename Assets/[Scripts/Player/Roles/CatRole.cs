using System;
using System.Reflection;
using UnityEngine;
using Unity;
using UnityEngine.Assertions;
using Managers;
using UnityEditor;
using Utility;

public sealed class CatRole : RoleBase
{
    private SphereDrawer _sphereDrawer;


    public override bool OnInit(GeneralPlayer player)
    {
        _sphereDrawer = player._sphereDrawer;
        player.ChangeColor(Globals.DEFAULT_CAT_COLOR);
        _rolePermissons = RolePermissons.ATTACK_OTHER_PLAYERS;
        return true;
    }


    public override bool OnUpdate(GeneralPlayer player)
    {
        return true;
    }

    public override void DoRoleSpecialAction(GeneralPlayer player)
    {
        const float dist_from_player = 2.0f;

        /*Debug.DrawRay(player_pos + (offset * 0.5f),
            player.CameraTranform.forward * dist_from_player, Color.red, 3.0f);
            */

        Vector3 ray_end_pos = calculateRayEndPosition(player.transform.position, player.CameraTransform.forward,
            player.height, dist_from_player);


        var result = Physics.OverlapSphere(ray_end_pos,
            player._sphereCollider.radius,
            Globals.PLAYER_MASK);

        foreach (var collider in result)
        {
            if (collider.CompareTag("Player"))
            {
                GeneralPlayer player_ref = collider.GetComponent<GeneralPlayer>();
                
                float damage = Globals.DEFAULT_PLAYER_DAMAGE * 1.0f;
                
                player_ref.LoseHealth(damage);
                
                EDebug.Log($"Current health = {player_ref.health}");
                
                var server_player_data = PlayerManager.instance.FindPlayer(player_ref.Connection);
                
                EDebug.Log("reached code here in " + nameof(CatRole));

                server_player_data.Item2.health += damage;
                PlayerManager.instance.SetPlayerHealth(player_ref, server_player_data.Item2.health);

                //Debug.Log("Player health = " + player_ref.health);
            }
        }

        _sphereDrawer.StartDraw(0.5f, ray_end_pos, 1.0f, Color.red);
    }

    public override bool OnEnd(GeneralPlayer player)
    {
        return true;
    }

    private Vector3 calculateRayEndPosition(
        Vector3 player_position, Vector3 camera_forward,
        float player_height, float distance_from_origin)
    {
        Vector3 offset = new Vector3(0, player_height * 0.5f, 0);

        Ray ray = new Ray(player_position + (offset * 0.5f), camera_forward * distance_from_origin);

        return ray.GetPoint(distance_from_origin);
    }
}