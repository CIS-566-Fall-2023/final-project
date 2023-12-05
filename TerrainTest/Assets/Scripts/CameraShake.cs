using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraShake : MonoBehaviour
{

    // shake
    Vector3 originPosition;
    Quaternion originRotation;
    Quaternion startRotation;
    float shake_decay;
    float shake_intensity;

    public static CameraShake Instance { get; private set; }

    protected void Awake()
    {
        Instance = this as CameraShake;
    }

    private void Start()
    {
        startRotation = transform.rotation;
    }
    private void Update()
    {
        if (shake_intensity > 0)
        {
            Vector3 move = new Vector3(0, Random.Range(-1.0f, 1.0f), 0);
            transform.position = originPosition + move * shake_intensity;
            transform.rotation = new Quaternion(
                            originRotation.x + Random.Range(-shake_intensity, shake_intensity) * 0.12f,
                            originRotation.y,
                            originRotation.z + Random.Range(-shake_intensity, shake_intensity) * 0.12f,
                            originRotation.w );
            shake_intensity -= shake_decay;
        }
        else
        {
            transform.rotation = startRotation;
        }
    }

    public void Shake()
    {
        originPosition = transform.position;
        originRotation = transform.rotation;
        shake_intensity = 0.09f;
        shake_decay = 0.0008f;
    }
}
