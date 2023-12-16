using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// scriptable object - enables us to share one preset among multiple scenes w one file
[System.Serializable]
// enables us to create instance of the class as a file in unity editor when we right click
[CreateAssetMenu(fileName = "Lighting Preset", menuName = "Scriptables/Lighting Preset", order = 1)]
public class LightingPreset : ScriptableObject
{
    // ambient color, directional light, and fog color 
    public Gradient AmbientColor;
    public Gradient DirectionalColor;
    public Gradient FogColor;
}