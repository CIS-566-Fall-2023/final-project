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

    public void OnEnable()
    {
        MinXProp = serializedObject.FindProperty("MinX");
        MinYProp = serializedObject.FindProperty("MinY");
        MaxXProp = serializedObject.FindProperty("MaxX");
        MaxYProp = serializedObject.FindProperty("MaxY");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        MinXProp.intValue = EditorGUILayout.IntField("Min X", MinXProp.intValue);
        MinYProp.intValue = EditorGUILayout.IntField("Min Y", MinYProp.intValue);
        MaxXProp.intValue = EditorGUILayout.IntField("Max X", MaxXProp.intValue);
        MaxYProp.intValue = EditorGUILayout.IntField("Max Y", MaxYProp.intValue);

        serializedObject.ApplyModifiedProperties();
    }
}
