using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointLightController : MonoBehaviour
{
    private GameObject skyDome;
    private SkyManager skyManager;
    private Light pointLight;

    void Start()
    {
        // Cache the Light component
        pointLight = GetComponent<Light>();

        // Find the SkyDome GameObject and get the SkyManager component
        skyDome = GameObject.Find("SkyDome");
        if (skyDome != null)
        {
            skyManager = skyDome.GetComponent<SkyManager>();
        }

        // Deactivate the light by default
        pointLight.enabled = false;
    }

    void Update()
    {
        // Ensure that the SkyManager is attached and available
        if (skyManager != null)
        {
            // Check if the TimeOfDay is between 22 and 4
            if (skyManager.TimeOfDay > 22 || skyManager.TimeOfDay < 4)
            {
                // Activate the light
                pointLight.enabled = true;
            }
            else
            {
                // Deactivate the light
                pointLight.enabled = false;
            }
        }
    }
}