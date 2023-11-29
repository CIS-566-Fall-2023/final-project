using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BiomeSky", menuName = "ScriptableObjects/Biome Sky", order = 1)]
public class SkyColorScriptableObject : ScriptableObject
{
    public Color skyColor;
    public Color horizonColor;
    public Color groundColor;
}
