using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScript : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        // randomize scale  
        Vector3 randScale = new Vector3(Random.Range(1.0f, 5.0f), Random.Range(1.0f, 10f), Random.Range(1.0f, 5.0f));
        transform.localScale = randScale;
    }

    // Update is called once per frame
    void Update()
    {

    }

}
