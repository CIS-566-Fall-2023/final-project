using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{

    // shake
    Vector3 punchVec = new Vector3(0.03f, 0.1f, 0);
    float duration = 0.5f;
    Quaternion startRotation;

    public static CameraShake Instance { get; private set; }

    protected void Awake()
    {
        Instance = this as CameraShake;
    }

    private void Start()
    {
        startRotation = transform.rotation;
        transform.DOShakeRotation(2.0f, 0.5f, 2, 20, false, ShakeRandomnessMode.Harmonic).SetLoops(-1, LoopType.Yoyo);
    }

    public void Shake()
    {       
        transform.DOPunchPosition(punchVec, duration, 5, 0, false);
    }
}
