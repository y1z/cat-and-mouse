using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class SphereDrawer : MonoBehaviour
{
    [Tooltip("controls where to sphere is drawn ")]
    public Vector3 widgetPosition;

    [Tooltip("controls the size of the sphere")]
    public float widgetSize;

    [Tooltip("controls color of the sphere")]
    public Color widgetColor;

    [Tooltip("Controls how long to draw the object")]
    public float duration;

    [Tooltip("keeps track of how long it's been drawing")]
    public float timeDrawing = 0.0f;

    [Tooltip("Tells the script if it can draw or not")]
    public bool canDraw = false;

    [Tooltip("Tells the script how many time per second to draw")] [Range(1.0f, 60.0f)]
    public float howManyTimesPerSecondToDraw = 5;

    // controls how much the coroutine waits for 
    private WaitForSeconds _howMuchToWaitFor;

    private GameObject _objToDraw;

    // keeps track of where to store the object when not in use
    private Vector3 storagePosition;

    private Coroutine currentCoroutine;

    public static readonly Color GIZMO_COLOR_DEFAULT = Color.red;

    private void Start()
    {
        _objToDraw = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        _objToDraw.GetComponent<SphereCollider>().isTrigger = true;
        storagePosition = Vector3.left * 1337.0f;
        _objToDraw.transform.position = storagePosition;
        currentCoroutine = null;
    }

    public void StartDraw(float _duration, Vector3 position, float size = 1.0f, Color? color = null)
    {
        canDraw = true;
        duration = _duration;
        widgetPosition = position;
        if (color == null)
        {
            color = GIZMO_COLOR_DEFAULT;
        }

        float waitTimeValue = Mathf.Clamp(1.0f / howManyTimesPerSecondToDraw, float.Epsilon, _duration);
        _howMuchToWaitFor = new WaitForSeconds(waitTimeValue);
        widgetColor = color.Value;
        widgetSize = size;
        timeDrawing = 0.0f;
        if (currentCoroutine == null)
        {
            currentCoroutine = StartCoroutine(drawSphere());
        }
    }

    IEnumerator drawSphere()
    {
        Vector3 scale = _objToDraw.transform.localScale;
        Vector3 original_scale = new Vector3(scale.x, scale.y, scale.z);
        while (duration > timeDrawing)
        {
            _objToDraw.transform.position = widgetPosition;
            _objToDraw.transform.localScale = new Vector3(original_scale.x * widgetSize, original_scale.y * widgetSize,
                original_scale.z * widgetSize);


            yield return _howMuchToWaitFor;
        }

        //Debug.Log(Utility.StringUtil.addColorToString("This string is Green", Color.green),this);
        _objToDraw.transform.localScale = original_scale;
        _objToDraw.transform.position = storagePosition;
        endDraw();
    }

    private void Update()
    {
        if (!canDraw)
        {
            return;
        }


        timeDrawing += Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        if (!canDraw && duration > timeDrawing)
            return;
        
        Color initial_color = Gizmos.color;
        Gizmos.color = widgetColor;
        //Gizmos.DrawWireSphere();
        Gizmos.DrawSphere(widgetPosition, widgetSize);

        Gizmos.color = initial_color;
        timeDrawing += Time.deltaTime;
    }

    private void endDraw()
    {
        canDraw = false;
        timeDrawing = 0.0f;
        currentCoroutine = null;
    }
}