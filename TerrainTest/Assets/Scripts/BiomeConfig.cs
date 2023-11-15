using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct Layer
{
    public int width;
    public List<GameObject> meshes;
    public Material material;
}

[CreateAssetMenu(fileName = "New Biome Config", menuName = "Biome Config")]
public class BiomeConfig : ScriptableObject
{
    public bool blendNearMid;
    public float midNearRatio;
    public bool blendMidFar;
    public float farMidRatio;
    public Layer near;
    public Layer mid;
    public Layer far;
}


