using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (FieldOfView))]
public class FieldOfViewEditor : Editor
{
    void OnSceneGUI()
    {
        FieldOfView _fov = (FieldOfView)target;
        Handles.color = Color.yellow;
        Handles.DrawWireArc(_fov.transform.position, Vector3.up, Vector3.forward, 360, _fov.radius);
        Vector3 _viewAngleA = _fov.ConvertAngle(-_fov.angle / 2, false);
        Vector3 _viewAngleB = _fov.ConvertAngle(_fov.angle / 2, false);

        Handles.DrawLine(_fov.transform.position, _fov.transform.position + _viewAngleA * _fov.radius);
        Handles.DrawLine(_fov.transform.position, _fov.transform.position + _viewAngleB * _fov.radius);

        Handles.color = Color.red;
        if (_fov.target != null)
            Handles.DrawLine(_fov.transform.position, _fov.target.position);

    }
}