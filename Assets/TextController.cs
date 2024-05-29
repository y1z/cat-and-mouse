using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public sealed class TextController : MonoBehaviour
{
    [SerializeField] private TextMeshPro _meshPro;
    // Start is called before the first frame update
    void Start()
    {
        if (_meshPro == null)
        {
            _meshPro = GetComponent<TextMeshPro>();
            Debug.Assert(_meshPro != null,"_meshPro should not be null", this );
        }

    }

    public void changeText(string new_text)
    {
        _meshPro.text = new_text;
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
