using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class SphereDrawer : MonoBehaviour
{
    [Tooltip("controls where to sphere is drawn ")]
    public Vector3 gizmoPosition;

    [Tooltip("controls the size of the sphere")]
    public float gizmoSize;

    [Tooltip("controls color of the sphere")]
    public Color gizmoColor;

    [Tooltip("Controls how long to draw the object")]
    public float duration;

    [Tooltip("keeps track of how long it's been drawing")]
    public float timeDrawing = 0.0f;

    public bool canDraw = false;

    private GameObject _objToDraw;

    public static readonly Color GIZMO_COLOR_DEFAULT = Color.red;

    private void Start()
    {
        _objToDraw = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    }

    public void StartDraw(float _duration, Vector3 position, float size = 1.0f, Color? color = null)
    {
        canDraw = true;
        gizmoPosition = position;
        duration = _duration;

        if (color == null)
        {
            color = GIZMO_COLOR_DEFAULT;
        }

        gizmoColor = color.Value;
        gizmoSize = size;
    }


    private void OnDrawGizmos()
    {
        if (!canDraw && duration > timeDrawing)
            return;

        Debug.Log($" duration ");

        Color initial_color = Gizmos.color;
        Gizmos.color = gizmoColor;
        //Gizmos.DrawWireSphere();
        Gizmos.DrawSphere(gizmoPosition, gizmoSize);

        Gizmos.color = initial_color;
        timeDrawing += Time.deltaTime;
    }

    public void endDraw()
    {
        canDraw = false;
        timeDrawing = 0.0f;
    }
}