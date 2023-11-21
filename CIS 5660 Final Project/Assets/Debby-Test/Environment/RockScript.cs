using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // randomize scale btwn 1 and 3  
        Vector3 randScale = new Vector3(Random.Range(1.0f, 3.0f), Random.Range(1.0f, 3.0f), Random.Range(1.0f, 3.0f));
        transform.localScale = randScale;
    }

    // Update is called once per frame
    void Update()
    {

    }

}
