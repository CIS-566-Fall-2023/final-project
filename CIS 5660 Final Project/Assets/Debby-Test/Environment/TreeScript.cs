using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScript : MonoBehaviour
{
    [SerializeField]
    private int treeType;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 randScale; 

        // randomize scale based on tree type 
        switch (treeType) {
            case 0: 
                // pine tree 
                randScale = new Vector3(Random.Range(100.0f, 200.0f), Random.Range(100.0f, 400.0f), Random.Range(100.0f, 200.0f));
                transform.localScale = randScale;
                break;
            case 1:  
                // mushroom tree 
                randScale = new Vector3(Random.Range(50.0f, 100.0f), Random.Range(50.0f, 100.0f), Random.Range(50.0f, 100.0f));
                transform.localScale = randScale;
                break;
            case 2: // swirl
                randScale = new Vector3(Random.Range(30.0f, 70.0f), Random.Range(30.0f, 100.0f), Random.Range(30.0f, 70.0f));
                transform.localScale = randScale;
                break;
            case 3: // palm 
                randScale = new Vector3(Random.Range(30.0f, 70.0f), Random.Range(30.0f, 150.0f), Random.Range(30.0f, 70.0f));
                transform.localScale = randScale;
                break; 
             
        }

    }    

    // Update is called once per frame
    void Update()
    {

    }

}
