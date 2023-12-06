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
        if (cell.pentagonDirection == -1)
        {
            itemOptions = new string[WFCManager.Instance.HexagonItemNum];
            for (int i = 0; i < WFCManager.Instance.HexagonItemNum; i++)
            {
                itemOptions[i] = WFCManager.Instance.GetHexagonItem(i).ItemName;
            }
        }
        else
        {
            itemOptions = new string[WFCManager.Instance.PentagonItemNum];
            for (int i = 0; i < WFCManager.Instance.PentagonItemNum; i++)
            {
                itemOptions[i] = WFCManager.Instance.GetPentagonItem(i).ItemName;
            }
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
                if (cell.pentagonDirection == -1)
                {
                    cell.PlaceItem(WFCManager.Instance.GetHexagonItem(selectedItem));
                }
                else
                    cell.PlaceItem(WFCManager.Instance.GetPentagonItem(selectedItem));
            }
            if (GUILayout.Button("Clear"))
            {
                cell.CleanUp();
            }
        }
    }
}
