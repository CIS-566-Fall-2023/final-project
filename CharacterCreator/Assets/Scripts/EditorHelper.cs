#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class FlingMenu
{
    [MenuItem("GameObject/SDF Collection", false, 10)]
    private static void CreateSDFCollection(MenuCommand menuCommand)
    {
        GameObject sdfObject = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/SDFCollection.prefab", typeof(GameObject)) as GameObject;

        // Create a custom game object
        GameObject go = GameObject.Instantiate(sdfObject);
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }

    [MenuItem("CustomMenu/Print the count of selected gameobjects")]
    private static void PrintNumberOfSelectedGameObjects()
    {
        Debug.Log(Selection.gameObjects.Length);
    }
}
#endif