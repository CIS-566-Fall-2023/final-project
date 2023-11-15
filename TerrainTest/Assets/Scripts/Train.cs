using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    public float viewRange;
    public float speed;
    public static Train Instance { get; private set; }

    protected virtual void Awake()
    {
        Instance = this as Train;
    }

    private void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);
    }

    public float GetLeftBound()
    {
        return transform.position.x - viewRange * 0.5f;
    }
}
