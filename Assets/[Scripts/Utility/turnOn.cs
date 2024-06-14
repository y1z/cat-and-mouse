using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Object;

public class turnOn : NetworkBehaviour 
{
    public List<Transform> objectToActivate = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        foreach (var obj in objectToActivate)
        {
            obj.gameObject.SetActive(true);
        }
    }

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        
        foreach (var obj in objectToActivate)
        {
            obj.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
