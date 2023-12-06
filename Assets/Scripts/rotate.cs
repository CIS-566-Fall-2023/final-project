using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class rotate : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0, 50, 0); // Rotation speed in degrees per second
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
