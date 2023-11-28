using System;
using System.Collections.Generic;
using BinaryPartition;
using UnityEngine;

namespace MyDebug
{
    public class DebugRenderer : MonoBehaviour
    {
        private List<IDebugDrawable> _drawables = new();
        
        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("Running Debug Renderer");
            BinaryRoom room = new BinaryRoom(new Rectangle
            {
                Min = new Vector2(-100, -50), Max = new Vector2(100, 50)
            });
            room.RandomSplit();
            foreach (var rect in room.GetRects())
            {
                _drawables.Add(new DebugRect {Rectangle = rect, Color = Color.blue});
            }

            foreach (var (a, b) in room.SplitDivider.GetSegments())
            {
                _drawables.Add(new DebugSegment { P0 = a, P1 = b , Color = Color.green});
            }
        }

        // Update is called once per frame
        void Update()
        {
            foreach (var drawable in _drawables)
            {
                drawable.Draw();
            }
        }
    }
}
