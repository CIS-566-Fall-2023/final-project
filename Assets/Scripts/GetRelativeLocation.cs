using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetRelativeLocation : MonoBehaviour
{
    protected Renderer m_Renderer;
    protected MaterialPropertyBlock m_PropBlock;
    // Start is called before the first frame update
    void Start()
    {
        // Assuming you have a reference to the Renderer
        m_Renderer = GetComponent<Renderer>();
        m_PropBlock = new MaterialPropertyBlock();
        
    }

    // Update is called once per frame
    void Update()
    {
        Matrix4x4 worldToLocalMatrix = transform.parent == null ? Matrix4x4.identity : transform.parent.worldToLocalMatrix;

        GetComponent<Renderer>().GetPropertyBlock(m_PropBlock);
        m_PropBlock.SetMatrix("_ParentWorldToLocal", worldToLocalMatrix);
        GetComponent<Renderer>().SetPropertyBlock(m_PropBlock);
    }
}
