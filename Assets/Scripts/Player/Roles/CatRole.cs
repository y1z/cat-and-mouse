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
            Transform player_orientation = player.Orientation;
            Vector3 player_pos = player.transform.position;
            const float distance_from_player = 20.0f;
            
            var a= Physics.Raycast(player_pos,player_orientation.forward,10.0f ,Globals.PLAYER_MASK);

            const float dist_from_player = 2.0f;
            
            Vector3 offset = new Vector3(0, player.height * 0.5f, 0);
            Ray ray = new Ray(player_pos + (offset * 0.5f),
                player.CameraTranform.forward * dist_from_player);

            Debug.DrawRay(player_pos + (offset * 0.5f),
                player.CameraTranform.forward * dist_from_player, Color.red, 3.0f);

            Vector3 ray_end_pos = ray.GetPoint(dist_from_player);

            var result = Physics.OverlapSphere(ray_end_pos,
                player._sphereCollider.radius,
                Globals.PLAYER_MASK);

            var count = result.Length;
            Debug.Log("total collider : " + count);
                
                foreach (var collider in result)
                {
                    collider.GetComponent<GeneralPlayerCharacteristics>().testFunction();
                }
        }

        public bool OnEnd(GeneralPlayerCharacteristics player)
        {
            return true;
        }
    }