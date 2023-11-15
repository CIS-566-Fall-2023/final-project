using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct Layer
{
    public List<GameObject> meshes;
    public Material material;
}

[CreateAssetMenu(fileName = "New Biome Config", menuName = "Biome Config")]
public class BiomeConfig : ScriptableObject
{
    public Layer near;
    public Layer mid;
    public Layer far;
}

