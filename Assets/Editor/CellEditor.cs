using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Planetile;

[CustomEditor(typeof(Cell))]
public class CellEditor : Editor
{
    Cell cell;
    void OnEnable()
    {
        cell = (Cell)target;
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Place one item"))
        {
            WFCManager.Instance.Collapse(cell);
        }
    }
}
