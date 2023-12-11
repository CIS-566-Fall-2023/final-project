using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerator))]
public class MapCustomInspector : Editor
{
    // Public attributes

    SerializedProperty
        minX,
        minY,
        maxX,
        maxY,
        scale;

    SerializedProperty 
    //lineColor,
    //lineThickness,
    //lineMaterial,
    lines,
    //canvas,
    //font,
    textPrefab,
    //footprintSprite,
    footprintParticle,
    lineMat1, 
    lineMat2,
    lineMat3,
    lineMat4;

    public void OnEnable()
    {
        minX = serializedObject.FindProperty("MinX");
        minY = serializedObject.FindProperty("MinY");
        maxX = serializedObject.FindProperty("MaxX");
        maxY = serializedObject.FindProperty("MaxY");

        scale = serializedObject.FindProperty("Scale");

        // lineColor = serializedObject.FindProperty("LineColor");
        // lineThickness = serializedObject.FindProperty("LineThickness");
        // lineMaterial = serializedObject.FindProperty("LineMaterial");
        lines = serializedObject.FindProperty("Lines");
        // canvas = serializedObject.FindProperty("Canvas");
        // font = serializedObject.FindProperty("Font");
        textPrefab = serializedObject.FindProperty("TextPrefab");

        lineMat1 = serializedObject.FindProperty("lineMat1");
        lineMat2 = serializedObject.FindProperty("lineMat2");
        lineMat3 = serializedObject.FindProperty("lineMat3");
        lineMat4 = serializedObject.FindProperty("lineMat4");

        //footprintSprite = serializedObject.FindProperty("footprintSprite");

        footprintParticle = serializedObject.FindProperty("footprintParticle");

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        Texture banner = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Images/mapLogo.jpg", typeof(Texture));
        GUILayout.Box(banner, GUILayout.Width(400), GUILayout.Height(400));

        EditorGUILayout.LabelField("Map Bounds", EditorStyles.boldLabel);
        minX.intValue = EditorGUILayout.IntField("Min X", minX.intValue);
        minY.intValue = EditorGUILayout.IntField("Min Y", minY.intValue);
        maxX.intValue = EditorGUILayout.IntField("Max X", maxX.intValue);
        maxY.intValue = EditorGUILayout.IntField("Max Y", maxY.intValue);
        
        scale.floatValue = EditorGUILayout.FloatField("Scale", scale.floatValue);

        EditorGUILayout.Space();


        // EditorGUILayout.LabelField("Line Properties", EditorStyles.boldLabel);
        // lineColor.colorValue = EditorGUILayout.ColorField("Line Color", lineColor.colorValue);
        // lineThickness.floatValue = EditorGUILayout.FloatField("Line Thickness", lineThickness.floatValue);
        // lineMaterial.objectReferenceValue = (Material) EditorGUILayout.ObjectField("Line Material", lineMaterial.objectReferenceValue, typeof(Material));
        //
        // lines.objectReferenceValue = EditorGUILayout.ObjectField("Lines", lines.objectReferenceValue, typeof(GameObject));
        //
        // canvas.objectReferenceValue = EditorGUILayout.ObjectField("Canvas", canvas.objectReferenceValue, typeof(Canvas));
        //
        // font.objectReferenceValue = EditorGUILayout.ObjectField("Font", font.objectReferenceValue, typeof(Font));

        textPrefab.objectReferenceValue = EditorGUILayout.ObjectField("TextPrefab", textPrefab.objectReferenceValue, typeof(GameObject));

        //footprintSprite.objectReferenceValue = EditorGUILayout.ObjectField("footprintSprite", footprintSprite.objectReferenceValue, typeof(Sprite));

        footprintParticle.objectReferenceValue = EditorGUILayout.ObjectField("footprintParticle", footprintParticle.objectReferenceValue, typeof(GameObject));

        lineMat1.objectReferenceValue = EditorGUILayout.ObjectField("lineMat1", lineMat1.objectReferenceValue, typeof(Material));
        lineMat2.objectReferenceValue = EditorGUILayout.ObjectField("lineMat2", lineMat2.objectReferenceValue, typeof(Material));
        lineMat3.objectReferenceValue = EditorGUILayout.ObjectField("lineMat3", lineMat3.objectReferenceValue, typeof(Material));
        lineMat4.objectReferenceValue = EditorGUILayout.ObjectField("lineMat4", lineMat4.objectReferenceValue, typeof(Material));
        

        serializedObject.ApplyModifiedProperties();
    }
}
