using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerator))]
public class MapCustomInspector : Editor
{
    // Public attributes

    SerializedProperty
        MinXProp,
        MinYProp,
        MaxXProp,
        MaxYProp;

    SerializedProperty lineColor,
    lineThickness,
    lineMaterial;

    public void OnEnable()
    {
        
        MinXProp = serializedObject.FindProperty("MinX");
        MinYProp = serializedObject.FindProperty("MinY");
        MaxXProp = serializedObject.FindProperty("MaxX");
        MaxYProp = serializedObject.FindProperty("MaxY");

        lineColor = serializedObject.FindProperty("LineColor");
        lineThickness = serializedObject.FindProperty("LineThickness");
        lineMaterial = serializedObject.FindProperty("LineMaterial");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        Texture banner = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Images/mapLogo.jpg", typeof(Texture));
        GUILayout.Box(banner, GUILayout.Width(400), GUILayout.Height(400));

        EditorGUILayout.LabelField("Map Bounds", EditorStyles.boldLabel);
        MinXProp.intValue = EditorGUILayout.IntField("Min X", MinXProp.intValue);
        MinYProp.intValue = EditorGUILayout.IntField("Min Y", MinYProp.intValue);
        MaxXProp.intValue = EditorGUILayout.IntField("Max X", MaxXProp.intValue);
        MaxYProp.intValue = EditorGUILayout.IntField("Max Y", MaxYProp.intValue);

        EditorGUILayout.Space();


        EditorGUILayout.LabelField("Line Properties", EditorStyles.boldLabel);
        lineColor.colorValue = EditorGUILayout.ColorField("Line Color", lineColor.colorValue);
        lineThickness.floatValue = EditorGUILayout.FloatField("Line Thickness", lineThickness.floatValue);
        lineMaterial.objectReferenceValue = (Material) EditorGUILayout.ObjectField("Line Material", lineMaterial.objectReferenceValue, typeof(Material));

        serializedObject.ApplyModifiedProperties();
    }
}
