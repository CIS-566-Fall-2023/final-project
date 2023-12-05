using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform leftDoor;
    public Transform rightDoor;

    float translation;
    bool isSliding = false;

    void Update()
    {
        if (isSliding)
        {
            Debug.Log("isSliding");

            leftDoor.Translate(Vector3.right * Time.deltaTime);
            rightDoor.Translate(-Vector3.right * Time.deltaTime);
            
            translation += Time.deltaTime;
            if (translation > 2.5)
            {
                isSliding = false;
                translation = 0;
            }
        }
        
    }

    public void Open()
    {
        isSliding = true;
    }
}
