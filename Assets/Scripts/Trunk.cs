using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
//using System.Numerics;
using UnityEngine;

public class Trunk : MonoBehaviour
{
    [SerializeField]
    private Knod[] m_Knods;
    [SerializeField]
    public float m_Radius;
    // You can perform actions with the array in your script
    void Start()
    {
        if (m_Knods != null)
        {
            foreach (Knod data in m_Knods)
            {
                Debug.Log(data.GetHeight(0.5f));
            }
        }
    }
    public Vector3 GetKnodPosition(float d, int idx){
        if(idx >= m_Knods.Length){
            return new Vector3();
        }
        Knod curKnod = m_Knods[idx];
        float h = curKnod.GetHeight(d);
        float ori = curKnod.GetOrientation();
        
        Vector3 fwd = new Vector3(m_Radius,0,h);
        Quaternion orientation = Quaternion.Euler(0,ori,0);
        return orientation * fwd;
    }
}
