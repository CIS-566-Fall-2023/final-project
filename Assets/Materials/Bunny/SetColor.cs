using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetColor : MonoBehaviour
{
    [SerializeField]
    public Color Color;
    // Start is called before the first frame update
    void Start()
    {
        // Get all Renderer components in the children
        Renderer[] childRenderers = GetComponentsInChildren<Renderer>();

        // Loop through each child Renderer
        foreach (Renderer rend in childRenderers)
        {
            // Change the color of the material
            rend.material.SetColor("_WoodColor", Color);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
