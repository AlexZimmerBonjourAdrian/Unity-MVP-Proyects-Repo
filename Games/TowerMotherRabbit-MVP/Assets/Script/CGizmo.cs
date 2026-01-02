using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGizmo : MonoBehaviour
{
    public Color GizmoColor = Color.red;
    public float GizmoRadius = 1f;

    void OnDrawGizmos()
    {
        Gizmos.color = GizmoColor;
        Gizmos.DrawSphere(transform.position, GizmoRadius);
    }
}
