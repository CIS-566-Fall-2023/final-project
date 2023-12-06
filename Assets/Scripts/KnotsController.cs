using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnotsController : MonoBehaviour
{
    private int m_TextureWidth = 256;
    private int m_TextureHeight = 256;

    private Texture2D m_KnotHeightMap;
    private Texture2D m_KnotOrientationMap;
    private Texture2D m_KnotStateMap;

    [SerializeField]
    private float m_MaxHeight = 100.0f;
    [SerializeField]
    private float m_MinRadius = 1.0f;
    [SerializeField]
    private float m_MaxRadius = 2.0f;

    public Knot[] m_Knots;
    public Material[] m_Materials;

    public bool UpdateEveryFrame = false;
    // Start is called before the first frame update
    void Start()
    {
        m_KnotHeightMap = new Texture2D(m_TextureWidth, m_TextureHeight);
        m_KnotOrientationMap = new Texture2D(m_TextureWidth, m_TextureHeight);
        m_KnotStateMap = new Texture2D(m_TextureWidth, m_TextureHeight);
        GenerateKnotMaps();
        UpdateTexture();
    }

    // Update is called once per frame
    void Update()
    {
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
    private void UpdateTexture()
    {
        foreach(Material material in m_Materials)
        {
            material.SetFloat("_MaxHeight", m_MaxHeight);
            material.SetFloat("_MinRadius", m_MinRadius);
            material.SetFloat("_MaxRadius", m_MaxRadius);
            material.SetFloat("_KnotCount", m_Knots.Length);
            material.SetTexture("_HeightMap", m_KnotHeightMap);
            material.SetTexture("_OrientationMap",m_KnotOrientationMap);
            material.SetTexture("_StateMap", m_KnotStateMap);
        }
    }
}
