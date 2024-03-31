using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CollectableManager : MonoBehaviour
{
    
    public static CollectableManager instance;
    
    private List<CollectableBase> _collectables = new List<CollectableBase>();

    private int totalCollected = 0;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }


    private void Start()
    {
       GetEveryCollectable(); 
    }

    // Update is called once per frame
    void Update()
    {

        foreach (var collectable in _collectables)
        {
            if (collectable.isCollected)
            {
                collectable.gameObject.SetActive(false);
                collectable.isCollected = false;
                totalCollected += 1;
            }
            
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("total collected = " + totalCollected);
        }
#endif
        
    }


    public void GetEveryCollectable()
    {
       GameObject[] objects = GameObject.FindGameObjectsWithTag("Collectable");

       foreach (var obj in objects)
       {
           if (obj.TryGetComponent(out CollectableBase collectable))
           {
               _collectables.Add(collectable);
           }

       }
        
       Debug.Log("Collectables found = " + _collectables.Count);
    }
}
