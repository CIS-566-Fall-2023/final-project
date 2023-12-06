using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.Animations;

public class Interactivity : MonoBehaviour
{
    public Camera mainCamera;
    public TMP_InputField input1;

    public Animator mapAnimator;

    bool rotatedCamera = false;

    // Update is called once per frame
    void Update()
    {
        if (!rotatedCamera && verifyInput())
        {
            input1.gameObject.SetActive(false);
            mainCamera.transform.Rotate(90, 0, 0);
            mainCamera.transform.position = new Vector3(0.0f, 84.1f, 0f);
     
            mapAnimator.SetBool("unfoldMap", true);
            rotatedCamera = true;
        }
    }

        bool verifyInput()
    {
        if(input1.text.ToLower() == "i") 
           // + "solemnly swear that i am up to no good")
        {
            return true;
        }
        else
        {
            return false;
        }   
    }
}
