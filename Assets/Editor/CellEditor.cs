using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Planetile;
using System;

[CustomEditor(typeof(Cell))]
public class CellEditor : Editor
{
    Cell cell;
    string[] itemOptions;
    int selectedItem = 0;
    void OnEnable()
    {
        cell = (Cell)target;
        itemOptions = new string[WFCManager.Instance.ItemNum];
        for (int i = 0; i < WFCManager.Instance.ItemNum; i++)
        {
            itemOptions[i] = WFCManager.Instance[i].ItemName;
        }
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Place one item"))
        {
            WFCManager.Instance.Collapse(cell);
        }
        using (new EditorGUILayout.HorizontalScope())
        {
            selectedItem = EditorGUILayout.Popup(selectedItem, itemOptions);
            if (GUILayout.Button("Place"))
            {
                cell.PlaceItem(WFCManager.Instance[selectedItem]);
            }
            if (GUILayout.Button("Clear"))
            {
                cell.CleanUp();
            }
        }
    }
}
