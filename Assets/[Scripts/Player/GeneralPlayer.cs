using System;
using System.Reflection;
using System.Text;
using FishNet;
using FishNet.Component.Spawning;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;
using UnityEngine.Assertions;
using Managers;
using util = Utility;

/**
     * This class controls the general characteristics for each type of player
     * entity in this project.
     */
[RequireComponent(typeof(Rigidbody))]
public class GeneralPlayer : NetworkBehaviour
{
    [Tooltip("Controls how heigh the entity jumps")] [SerializeField]
    protected float _jumpForce;

    protected Renderer _renderer;

    [SerializeField] protected Rigidbody _body;

    [SerializeField] protected RoleController _roleController;

    protected GroundCheck _groundCheck;

    [SerializeField] protected Transform _orientation;

    [Tooltip("Attach an object with a sphere collider ")] [SerializeField]
    protected Transform _sphereColliderObj;

    public SphereCollider _sphereCollider;

    public SphereDrawer _sphereDrawer;

    [Tooltip("A reference to the players camera")] [SerializeField]
    private Camera _camera;

    public float height;

    [SyncVar(OnChange = nameof(OnChange_health))]
    public float health = 100.0f;
    
    public AudioClip[] audiosClips;

    public TextController textController;
    
    protected void Awake()
    {
        if (Mathf.Abs(_jumpForce) < float.Epsilon)
        {
            _jumpForce = 1.0f;
        }

        _renderer = GetComponent<Renderer>();
        _sphereCollider = _sphereColliderObj.GetComponent<SphereCollider>();
        Vector3 size = _body.GetComponent<CapsuleCollider>().bounds.size;
        _roleController = GetComponent<RoleController>();
        _groundCheck = GetComponent<GroundCheck>();
        _sphereDrawer = gameObject.AddComponent<SphereDrawer>();
        _roleController.Initialize(this, new UndecidedRole());
        textController = GetComponent<TextController>();
        
        height = size.y;
        Assert.IsNotNull(_body, "_body != null");
        Assert.IsNotNull(_sphereCollider, "_sphereCollider should NOT be null");
        Assert.IsNotNull(_sphereColliderObj, "_sphereColliderObj should NOT be null");
        Assert.IsNotNull(_sphereColliderObj, "_sphereColliderObj should NOT be null");
        
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (base.IsServer)
        {
            //PlayerManager.instance.playersData.Add(gameObject.GetInstanceID(), new PlayerData(){health = 1.0f}  );
            PlayerManager.instance._players.Add(base.Owner,
                new PlayerData() { health = Globals.DEFAULT_PLAYER_HEALTH });
        }

        if (!base.Owner.IsLocalClient)
        {
            return;
        }
        //Assert.IsNotNull(_orientation, "Orientation should NOT be null");

        //_renderer = GetComponent<Renderer>();

        /*
        Transform[] valid_spawn_points = InstanceFinder.NetworkManager.GetComponent<PlayerSpawner>().Spawns;
        bool is_in_spawn_point = IsPlayerInSpawn(valid_spawn_points);
        
        if (!is_in_spawn_point)
        {
           var spawn_point_index = UnityEngine.Random.Range(0, valid_spawn_points.Length - 1);
           transform.position = valid_spawn_points[spawn_point_index].position;
        }
        */
    }


    private void Update()
    {
        if (!base.IsOwner)
            return;


#if UNITY_EDITOR

        if (Input.GetKeyDown(KeyCode.O))
        {
            var temp = new MouseRole();
            _roleController.Initialize(this, temp);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            var temp = new CatRole();
            _roleController.Initialize(this, temp);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            //CollectableManager.instance.GetEveryCollectable();
        }

#endif // UNITY_EDITOR

        UpdateText();

        DoPlayerJump();
    }

    [ServerRpc]
    public void UpdateText()
    {
        UpdateTextRPC();
    }

    [ObserversRpc]
    public void UpdateTextRPC()
    {
        const string start = "[ ";
        float invert_default_health = 1.0f / Globals.DEFAULT_PLAYER_HEALTH;

        const int HOW_MANY_BARS_MAX_HEALTH = 4;
        
        int how_many_bars_to_generate = Mathf.FloorToInt( health * invert_default_health * HOW_MANY_BARS_MAX_HEALTH);
        StringBuilder sb = new StringBuilder(start, 1);

        string red_hex_color = ColorUtility.ToHtmlStringRGBA(Color.red);

        sb.Append($"<color=#{red_hex_color}>");
        //sb.Append("▉");
        for (int i = 0; i < how_many_bars_to_generate; ++i)
        {
            //$"<color=#{color_str}> {input} </color>" 
            // ▉
            sb.Append("█ ");
        }
        
        sb.Append("</color>");
        sb.Append("]");
        textController.changeText(sb.ToString());

    }


    [ServerRpc]
    public void ChangeColor(Color new_color)
    {
        ChangeColorRPC(new_color);
    }


    [ObserversRpc]
    protected void ChangeColorRPC(Color new_color)
    {
        _renderer.material.color = new_color;
    }

    private void DoPlayerJump()
    {
        if (Input.GetButtonDown("Jump") && _groundCheck.IsGrounded)
        {
            _body.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
    }
    


    private bool IsPlayerInSpawn(Transform[] valid_spawn_points)
    {
        Vector3 player_position = transform.position;
        bool result = false;

        foreach (var spawn_point in valid_spawn_points)
        {
            if (Vector3.Distance(player_position, spawn_point.position) <=
                Globals.DEFAULT_MAX_INITIAL_DISTANCE_FROM_SPAWN)
            {
                result = true;
            }
        }

        return result;
    }

    [ServerRpc(RequireOwnership = false)]
    private void MoveToSpawnPoint()
    {
        //if (!base.IsServer)
        //return;
        
        MoveToSpawnPointRPC();
    }

    [ObserversRpc(RunLocally = true)]
    private void MoveToSpawnPointRPC()
    {
        //if (!base.IsServer)
        //   return;
        Transform[] valid_spawn_points = InstanceFinder.NetworkManager.GetComponent<PlayerSpawner>().Spawns;
        int total_spawn_points = valid_spawn_points.Length;
        var spawn_point_index = UnityEngine.Random.Range(0, total_spawn_points);
        transform.position = valid_spawn_points[spawn_point_index].position;
        //_body.transform.Translate(valid_spawn_points[spawn_point_index].position);
#if UNITY_EDITOR
        util.EDebug.Log($"Function {nameof(MoveToSpawnPointRPC)} has been called");
#endif
    }

    void OnChange_health(float prev, float next, bool as_server)
    {
        if (next < 0.01f)
        {
            MoveToSpawnPoint();
            PlayerManager.instance.SetPlayerHealth(this, Globals.DEFAULT_PLAYER_HEALTH);
        }

#if UNITY_EDITOR
        print("player health = " + next);

#endif
    }

    public bool CanCurrentRoleCollect()
    {
        bool result = false;
        int bitmask = (byte)_roleController.Permissons & (byte)RolePermissons.CAN_COLLECT_CHEESE;
        if (bitmask > 0)
        {
            result = true;
        }

        return result;
    }

    [ObserversRpc(RunLocally = true)]
    public void TeleportToLocation(Vector3 location)
    {
        TeleportToLocationRpc(location);
    }

    [ServerRpc(RequireOwnership = false)]
    public void TeleportToLocationRpc(Vector3 location)
    {
        this.transform.position = location;
    }

    [ServerRpc(RequireOwnership = false)]
    public void InitCatRole()
    {
        InitCatRoleRPC();
    }

    [ObserversRpc(RunLocally = true)]
    private void InitCatRoleRPC()
    {
        var temp = new CatRole();
        _roleController.Initialize(this, temp);
    }

    [ServerRpc(RequireOwnership = false)]
    public void InitMouseRole()
    {
        InitMouseRoleRPC();
    }

    [ObserversRpc(RunLocally = true)]
    private void InitMouseRoleRPC()
    {
        var temp = new MouseRole();
        _roleController.Initialize(this, temp);
    }

    [ServerRpc(RequireOwnership = false)]
    public void LoseHealth(float amount_to_lose)
    {
        LoseHealthRPC(amount_to_lose);
    }

    [ObserversRpc]
    private void LoseHealthRPC(float amount_to_lose)
    {
        health = health - amount_to_lose;
    }


    public NetworkConnection Connection => base.Owner;

    public Transform CameraTransform => _camera.transform;
}