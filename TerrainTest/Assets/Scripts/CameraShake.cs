using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    // shake
    Vector3 punchVec = new Vector3(0.03f, 0.05f, 0);
    float duration = 0.8f;
    Quaternion startRotation;
    [HideInInspector] public Tween tween;

    public static CameraShake Instance { get; private set; }

    protected void Awake()
    {
        Instance = this as CameraShake;
        startRotation = transform.rotation;
        tween = transform.DOShakeRotation(1.5f, 0.5f, 2, 20, false, ShakeRandomnessMode.Harmonic).SetLoops(-1, LoopType.Yoyo);
        tween.Pause();
    }

    public void Shake()
    {       
        transform.DOPunchPosition(punchVec, duration, 1, 0, false);
    }
}
