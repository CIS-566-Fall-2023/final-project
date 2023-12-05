using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Planetile;
using System;

[CustomEditor(typeof(HexagonTile))]
public class TileEditor : Editor
{
    HexagonTile tile;
    void OnEnable()
    {
        tile = (HexagonTile)target;
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Fill"))
            {
                WFCManager.Instance.FillOneTile(tile);
            }
            if (GUILayout.Button("Clear"))
            {
                foreach(var cell in tile.cellData.Cells)
                {
                    cell.CleanUp();
                }
            }
        }
    }
}
