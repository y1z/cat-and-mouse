    using Unity.VisualScripting.FullSerializer;
    using UnityEditor;
    using UnityEngine;

    public sealed class CatRole : IRole
    {
        public bool OnInit(GeneralPlayerCharacteristics player)
        {
           player.ChangeColor(Globals.DEFAULT_CAT_COLOR);
           
           return true;
        }
        

        public bool OnUpdate(GeneralPlayerCharacteristics player)
        {


            return true;
        }

        public void DoRoleSpecialAction(GeneralPlayerCharacteristics player)
        {
            const float dist_from_player = 2.0f;
            
            Vector3 player_pos = player.transform.position;
            Vector3 offset = new Vector3(0, player.height * 0.5f, 0);
            
            Ray ray = new Ray(player_pos + (offset * 0.5f),
                player.CameraTranform.forward * dist_from_player);

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
                        collider.GetComponent<GeneralPlayerCharacteristics>().takeDamage(Globals.DEFAULT_PLAYER_DAMAGE);
                    }
                }
        }

        public bool OnEnd(GeneralPlayerCharacteristics player)
        {
            return true;
        }
    }