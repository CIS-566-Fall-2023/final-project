using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionOnlyFollow : MonoBehaviour
{
    public Transform parentTransform; // Assign the parent object in the inspector

    void Update()
    {
        if (parentTransform != null)
        {
            // Update position to match the parent, but keep local rotation unchanged
            transform.position = new Vector3(parentTransform.position.x, transform.position.y, parentTransform.position.z);

        }
    }
}

