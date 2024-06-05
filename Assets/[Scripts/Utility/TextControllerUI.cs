using TMPro;
using UnityEngine;

/**
 * Helps control text in UI space (NOT for world space)
 */
public class TextControllerUI : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI _meshPro;

    // Start is called before the first frame update
    void Start()
    {
        if (_meshPro == null)
        {
            _meshPro = GetComponent<TextMeshProUGUI>();
            Debug.AssertFormat(_meshPro != null, "_meshPro should not be null", this);
        }
    }

    public void changeText(string new_text)
    {
        _meshPro.text = new_text;
    }
}