using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
//using System.Numerics;
using UnityEngine;

public class Trunk : MonoBehaviour
{
    [SerializeField] 
    private Material m_OutputMaterial;
    [SerializeField]
    private Knod[] m_Knods;
    [SerializeField]
    public float m_Radius;

    public int m_TextureWidth = 256;
    public int m_TextureHeight = 256;
    public Texture2D m_KnotHeightMap;
    public Texture2D m_KnotOrientationMap;
    public Texture2D m_KnotStateMap;
    // You can perform actions with the array in your script
    void Start()
    {
        GenerateKnotMaps();
        ApplyTexture();
    }
    private int GetKnotIndex(int y){
        return (y * m_Knods.Length) / m_TextureHeight;
    }
    private Color CalculateHeightMap(Knod knot, float d){
        float r = knot.GetHeight(0);//z0
        float g = knot.GetHeight(d) - r;//z+
        float b = r - knot.GetHeight(d);//z-
        return new Color(r,g,b);
    }
    private Color CalculateOrientationMap(Knod knot, float d){
        float r = knot.GetOrientation();//w0
        float g = 0;//wccw
        float b = 0;//wcw
        return new Color(r,g,b);
    }
    private Color CalculateStateMap(Knod knot, float d){
        float r = knot.IsDead(d)?0:1;//alive
        float g = knot.m_TimeOfDeath;//time of death
        float b = 0;//nothing
        return new Color(r,g,b);
    }
    private void GenerateKnotMaps(){
        // Create a new texture
        m_KnotHeightMap = new Texture2D(m_TextureWidth, m_TextureHeight);
        m_KnotOrientationMap = new Texture2D(m_TextureWidth, m_TextureHeight);
        m_KnotStateMap = new Texture2D(m_TextureWidth, m_TextureHeight);
        if(m_Knods.Length < 1){
            Debug.Log("No knod");
            return;
        }

        // Loop through each pixel and set its color based on your custom algorithm or type
        for (int x = 0; x < m_TextureWidth; x++)
        {
            for (int y = 0; y < m_TextureHeight; y++)
            {
                int idx = GetKnotIndex(y);
                float d = ((float)x + 0.5f)/(float)m_TextureWidth;
                Color heightMapPixel = CalculateHeightMap(m_Knods[idx],d);
                Color orientationMapPixel = CalculateOrientationMap(m_Knods[idx],d);
                Color stateMapPixel = CalculateStateMap(m_Knods[idx],d);
                m_KnotHeightMap.SetPixel(x,y,heightMapPixel);
                m_KnotOrientationMap.SetPixel(x,y,orientationMapPixel);
                m_KnotStateMap.SetPixel(x,y,stateMapPixel);
            }
        }

        // Apply changes to the texture
        m_KnotHeightMap.Apply();
        m_KnotOrientationMap.Apply();
        m_KnotStateMap.Apply();
    }
    private void ApplyTexture(){
        if(m_OutputMaterial){
            m_OutputMaterial.SetTexture("_MainTex", m_KnotHeightMap);
            // m_OutputMaterial.SetTexture("_HeightTex", m_KnotHeightMap);
            // m_OutputMaterial.SetTexture("_OrientationTex", m_KnotOrientationMap);
            // m_OutputMaterial.SetTexture("_StateTex",m_KnotStateMap);
        }
    }
}
