using System;
using System.Collections.Generic;
using BinaryPartition;
using Geom;
using GraphBuilder;
using Navigation;
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
            Builder builder = new();
            
            PartitionRunner runner = new PartitionRunner(builder, new Rectangle
            {
                Min = new Vector2(-100, -50), Max = new Vector2(100, 50)
            });
            runner.Run();

            var navGraph = runner.Builder.ToGraph();

            foreach (var curve in navGraph.Curves())
            {
                var lineCurve = (LineCurve) curve;
                _drawables.Add(new DebugSegment() {P0 = lineCurve.P0, P1 = lineCurve.P1, Color = Color.green});
            }

            foreach (var rectangle in navGraph.Rectangles())
            {
                _drawables.Add(new DebugRect {Rectangle = rectangle, Color = Color.blue});
            }

            // foreach (var divider in runner.Dividers)
            // {
            //     foreach (var edgeId in divider.GetEdges())
            //     {
            //         var lineCurve = (LineCurve) runner.Builder.GetCurve(edgeId);
            //         _drawables.Add(new DebugSegment() {P0 = lineCurve.P0, P1 = lineCurve.P1, Color = Color.green});
            //     }
            // }

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
