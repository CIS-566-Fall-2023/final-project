using System;
using System.Collections.Generic;
using System.Linq;
using BinaryPartition;
using Generation;
using Generation.Tower;
using Geom;
using GraphBuilder;
using Navigation;
using Unity.Profiling;
using UnityEngine;

namespace MyDebug
{
    public class DebugRenderer : MonoBehaviour
    {
        private List<IDebugDrawable> _drawables = new();

        private List<ICurve> _walls;
        private Graph _graph;
        
        private int _vertexId = 0;
        private int _edgeId = 0;
        private int _incidentEdgeCount = 0;
        
        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("Running Debug Renderer");
            BuildingGenerator generator = new BuildingGenerator();
            generator.GenerateBuilding();
            _walls = generator.GetWalls().ToList();
            _graph = generator.Builder.ToGraph();

            foreach (var curve in _graph.Curves())
            {
                _drawables.Add(new DebugCurve(curve, Color.green));
            }
            
            foreach (var curve in _walls)
            {
                _drawables.Add(new DebugCurve(curve, Color.blue));
            }
        }

        // Update is called once per frame
        void Update()
        {
            foreach (var drawable in _drawables)
            {
                drawable.Draw();
            }
            //
            // if (Input.GetKeyDown("q"))
            // {
            //     _vertexId++;
            //     _incidentEdgeCount = _graph.AdjList[_vertexId].Count;
            //     _edgeId = 0;
            // }
            //
            // if (Input.GetKeyDown("w") && _incidentEdgeCount != 0)
            // {
            //     _edgeId = (_edgeId + 1) % _incidentEdgeCount;
            // }
            //
            // var vertex = _graph.Vertices[_vertexId];
            // new DebugRect(vertex.region.CenterPoint, Color.black).Draw();
            //
            // if (_incidentEdgeCount != 0)
            // {
            //     var edge = _graph.AdjList[_vertexId][_edgeId];
            //     new DebugRect(_graph.Vertices[edge.ToVertex].region.CenterPoint, Color.white).Draw();
            //     new DebugCurve(edge.Curve, Color.red).Draw();
            // }
            
            
            
            
            //new DebugRect(vertex.region.CenterPoint, Color.white).Draw();
        }
    }
}
