using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]public class Knot
{
    public AnimationCurve m_HeightMap;
    public float m_Orientation;
    public float m_TimeOfDeath;

    public float GetHeight(float d){
        return m_HeightMap.Evaluate(d);
    }

    public float GetOrientation(){
        return m_Orientation;
    }

    public bool IsDead(float d){
        return m_TimeOfDeath < d;
    }

    public float GetTimeSinceDeath(float t){
        if(t < m_TimeOfDeath){
            return 0;
        }else{
            return t - m_TimeOfDeath;
        }
    }
}
