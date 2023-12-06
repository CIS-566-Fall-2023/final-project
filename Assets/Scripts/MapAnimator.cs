using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAnimator : MonoBehaviour
{
    public MapGenerator mapGenerator;

    // This gets called at the end of the map unfold animation
    void unfolded()
    {
        mapGenerator.startDrawing = true;
    }
}
