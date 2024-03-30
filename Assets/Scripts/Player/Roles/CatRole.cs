using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Assertions;
using Managers;

public sealed class CatRole : RoleBase
{
    public override bool OnInit(GeneralPlayer player)
    {
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
        
        Vector3 player_pos = player.transform.position;
        Vector3 offset = new Vector3(0, player.height * 0.5f, 0);
        
        Ray ray = new Ray(player_pos + (offset * 0.5f),
            player.CameraTransform.forward * dist_from_player);

        /*Debug.DrawRay(player_pos + (offset * 0.5f),
            player.CameraTranform.forward * dist_from_player, Color.red, 3.0f);
            */

        Vector3 ray_end_pos = ray.GetPoint(dist_from_player);

        var result = Physics.OverlapSphere(ray_end_pos,
            player._sphereCollider.radius,
            Globals.PLAYER_MASK);
            
            foreach (var collider in result)
            {
                if (collider.CompareTag("Player"))
                {
                   GeneralPlayer player_ref = collider.GetComponent<GeneralPlayer>();
                   float damage = Globals.DEFAULT_PLAYER_DAMAGE * -1.0f;
                   var server_player_data = PlayerManager.instance.FindPlayer(player_ref.Connection);
                   
                   server_player_data.Item2.health += damage;
                   PlayerManager.instance.SetPlayerHealth(player_ref, server_player_data.Item2.health );

                   //Debug.Log("Player health = " + player_ref.health);
                }
            }
    }

    public override bool OnEnd(GeneralPlayer player)
    {
        return true;
    }
}