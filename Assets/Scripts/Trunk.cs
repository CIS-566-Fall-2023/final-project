using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
//using System.Numerics;
using UnityEngine;

public class Trunk : MonoBehaviour
{
    private int m_TextureWidth = 256;
    private int m_TextureHeight = 256;
    [SerializeField] 
    private Material m_OutputMaterial;
    [SerializeField]
    private Knot[] m_Knots;
    [SerializeField]
    private float m_MaxHeight = 100.0f;
    [SerializeField]
    private float m_MinRadius = 1.0f;
    [SerializeField]
    private float m_MaxRadius = 2.0f;

    [SerializeField] 
    private Texture2D m_ColorMap;
    private Texture2D m_KnotHeightMap;
    private Texture2D m_KnotOrientationMap;
    private Texture2D m_KnotStateMap;
    
    public bool UpdateEveryFrame = false;

    // You can perform actions with the array in your script
    void Start()
    {
        GenerateKnotMaps();
        UpdateTexture();
    }

    void Update(){
        //Get relative location
        Matrix4x4 worldToLocalMatrix = transform.worldToLocalMatrix * transform.parent.localToWorldMatrix;
        m_OutputMaterial.SetMatrix("_ParentWorldToLocal", worldToLocalMatrix);
        if(UpdateEveryFrame)
        {
            GenerateKnotMaps();
            UpdateTexture();
        }
    }
    private int GetKnotIndex(int y){
        return (y * m_Knots.Length) / m_TextureHeight;
    }
    private Color CalculateHeightMap(Knot knot, float d){
        //float zRatio = m_MaxHeight / m_MinRadius;
        float r = knot.GetHeight(0);//z0
        float g = knot.GetHeight(d) - r;//z+
        float b = r - knot.GetHeight(d);//z-
        return new Color(r,g,b);
    }
    private Color CalculateOrientationMap(Knot knot, float d){
        float r = (knot.GetOrientation()/360);//w0
        r = r - Mathf.Floor(r); //fract(r)
        float g = 0;//wccw
        float b = 0;//wcw
        return new Color(r,g,b);
    }
    //*add branchradius
    private Color CalculateStateMap(Knot knot, float d){
        float r = knot.IsDead(d)? 0:1;//alive
        float g = knot.m_TimeOfDeath;//time of death
        //float b = 0;//nothing
        float b = knot.GetRadius();
        return new Color(r,g,b);
    }
    private void GenerateKnotMaps(){
        // Create a new texture
        m_KnotHeightMap = new Texture2D(m_TextureWidth, m_TextureHeight);
        m_KnotOrientationMap = new Texture2D(m_TextureWidth, m_TextureHeight);
        m_KnotStateMap = new Texture2D(m_TextureWidth, m_TextureHeight);
        if(m_Knots.Length < 1){
            Debug.Log("No knot");
            return;
        }

        // Loop through each pixel and set its color based on your custom algorithm or type
        for (int x = 0; x < m_TextureWidth; x++)
        {
            for (int y = 0; y < m_TextureHeight; y++)
            {
                int idx = GetKnotIndex(y);
                float d = ((float)x + 0.5f)/(float)m_TextureWidth;
                Color heightMapPixel = CalculateHeightMap(m_Knots[idx],d);
                Color orientationMapPixel = CalculateOrientationMap(m_Knots[idx],d);
                Color stateMapPixel = CalculateStateMap(m_Knots[idx],d);
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
    private void UpdateTexture(){
        if(m_OutputMaterial){
            // float _MaxHeight;
            // float _AnimationTime;
            // float _MinRadius;
            // float _MaxRadius;
            // float _KnotCount;
            // sampler2D _ColorMap;
            // sampler2D _HeightMap;
            // sampler2D _OrientationMap;
            // sampler2D _StateMap;
            // sampler2D _PithRadiusMap;
            m_OutputMaterial.SetFloat("_MaxHeight", m_MaxHeight);
            m_OutputMaterial.SetFloat("_MinRadius", m_MinRadius);
            m_OutputMaterial.SetFloat("_MaxRadius", m_MaxRadius);
            m_OutputMaterial.SetFloat("_KnotCount", m_Knots.Length);
            m_OutputMaterial.SetTexture("_ColorMap", m_ColorMap);
            m_OutputMaterial.SetTexture("_HeightMap", m_KnotHeightMap);
            m_OutputMaterial.SetTexture("_OrientationMap",m_KnotOrientationMap);
            m_OutputMaterial.SetTexture("_StateMap", m_KnotStateMap);
            // m_OutputMaterial.SetTexture("_HeightTex", m_KnotHeightMap);
            // m_OutputMaterial.SetTexture("_OrientationTex", m_KnotOrientationMap);
            // m_OutputMaterial.SetTexture("_StateTex",m_KnotStateMap);
        }
    }
}
