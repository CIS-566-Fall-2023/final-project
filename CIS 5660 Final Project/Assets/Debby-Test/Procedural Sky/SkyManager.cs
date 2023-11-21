using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// execute always - execute methods in class while we are not running game/still in editor
[ExecuteAlways]
public class SkyManager : MonoBehaviour
{
    //Variables 
    [SerializeField, Range(0, 24)] private float TimeOfDay;

    private void Update()
    {
        if (Application.isPlaying)
        {
            // get player's position
            Vector3 playerPos = GameObject.Find("Player").transform.position;

            //(Replace with a reference to the game time)
            TimeOfDay += Time.deltaTime;
            TimeOfDay %= 24; //Modulus to ensure always between 0-24
            // update sky color 
            Material skyColor = GetComponent<Renderer>().sharedMaterial; 
            skyColor.SetTextureOffset("_MainTex", new Vector2((TimeOfDay - 12) * .05f, 0));
        }
        else
        {
            // update sky color 
            Material skyColor = GetComponent<Renderer>().sharedMaterial;
            skyColor.SetTextureOffset("_MainTex", new Vector2((TimeOfDay - 12) * .05f, 0));

        }
    }
}