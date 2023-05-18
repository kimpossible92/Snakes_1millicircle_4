using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class MMDebug
{
    //public static MMConsole _console;

    public static RaycastHit2D RayCast(Vector2 rayOriginPoint, Vector2 rayDirection, float rayDistance, LayerMask mask, Color color, bool drawGizmo = false)
    {
        if (drawGizmo)
        {
            Debug.DrawRay(rayOriginPoint, rayDirection * rayDistance, color);
        }
        return Physics2D.Raycast(rayOriginPoint, rayDirection, rayDistance, mask);
    }
    public static bool Raycast3DBoolean(Vector3 rayOriginPoint, Vector3 rayDirection, float rayDistance, LayerMask mask, Color color, bool drawGizmo = false)
    {
        if (drawGizmo)
        {
            Debug.DrawRay(rayOriginPoint, rayDirection * rayDistance, color);
        }
        RaycastHit hit;
        return Physics.Raycast(rayOriginPoint, rayDirection, out hit, rayDistance, mask);
    }
    public static RaycastHit Raycast3D(Vector3 rayOriginPoint, Vector3 rayDirection, float rayDistance, LayerMask mask, Color color, bool drawGizmo = false)
    {
        if (drawGizmo)
        {
            Debug.DrawRay(rayOriginPoint, rayDirection * rayDistance, color);
        }
        RaycastHit hit;
        Physics.Raycast(rayOriginPoint, rayDirection, out hit, rayDistance, mask);
        return hit;
    }
}
