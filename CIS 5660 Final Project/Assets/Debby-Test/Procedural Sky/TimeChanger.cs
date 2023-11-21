using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeChanger : MonoBehaviour
{
    // skybox material
    [SerializeField] private Material skybox;
    private float _elapsedTime = 0f;
    private float _timeScale = 2.5f;
    private static readonly int Rotation = Shader.PropertyToID("_Rotation");
    private static readonly int Exposure = Shader.PropertyToID("_Exposure");

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // increase elapsed time 
        _elapsedTime += Time.deltaTime;
        // set rotation and exposure 
        skybox.SetFloat(Rotation, _elapsedTime + _timeScale);
        // repeat btwn 0.15f and 1.f else it would get too dark
        skybox.SetFloat(Exposure, Mathf.Clamp(Mathf.Sin(_elapsedTime), 0.15f, 1f));

    }
}
