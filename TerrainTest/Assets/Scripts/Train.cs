using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    public float viewRange;
    public float speed;
    public float shakeInterval;

    private float m_shakeTimer;
    public static Train Instance { get; private set; }

    protected void Awake()
    {
        Instance = this as Train;
    }

    private void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);
        m_shakeTimer += Time.deltaTime;
        if(m_shakeTimer >= shakeInterval)
        {
            CameraShake.Instance.Shake();
            m_shakeTimer = 0;
        }
    }

    public float GetLeftBound()
    {
        return transform.position.x - viewRange * 0.5f;
    }
}
