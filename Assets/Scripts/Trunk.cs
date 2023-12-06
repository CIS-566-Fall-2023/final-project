using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
//using System.Numerics;
using UnityEngine;

public class Trunk : MonoBehaviour
{
    public Material m_OutputMaterial;

    // You can perform actions with the array in your script
    void Start()
    {
    }

    void Update(){
        //Get relative location
        Matrix4x4 worldToLocalMatrix = transform.worldToLocalMatrix * transform.parent.localToWorldMatrix;
        m_OutputMaterial.SetMatrix("_ParentWorldToLocal", worldToLocalMatrix);
    }
}
