using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    public float viewRange;
    public float speed;
    public float shakeInterval;

    private float m_shakeTimer;
    private bool m_playing;

    public static Train Instance { get; private set; }

    protected void Awake()
    {
        Instance = this as Train;
        m_playing = false;
    }

    private void Update()
    {
        if (!m_playing || !ObjectManager.Instance.initializationFinished) return;

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

    public void Play()
    {
        if (m_playing) return;
        m_playing = true;
        CameraShake.Instance.tween.Play();
    }

    public void Pause()
    {
        if (!m_playing) return;
        m_playing = false;
        CameraShake.Instance.tween.Pause();
    }
}
