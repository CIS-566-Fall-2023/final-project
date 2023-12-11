using System.Collections;
using Geom;
using UnityEngine;
using System.Collections.Generic;
using System.Numerics;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Navigation
{
    public class Wanderer : MonoBehaviour
    {
        private Vector2 _position;
        private float _rotationAngle;
        private Graph _navGraph;

        private int _pathEnd;
        private ICurve _currCurve;
        private float _curveT;

        private readonly Queue<ICurve> _path = new();
        
        public GameObject particleSys;

        private const float _speed = 30f;

        public void Initialize(Graph navGraph)
        {
            Instantiate(particleSys, gameObject.transform);
            _navGraph = navGraph;
            var start = navGraph.GetRandomEdge().ToVertex;
            var edge = navGraph.GetNextEdge(start);
            _currCurve = edge.Curve;
            _pathEnd = edge.ToVertex;
            _curveT = 0;
        }

        void Update()
        {
            float dist = _speed * Time.deltaTime;
            float deltaT = dist / _currCurve.Length();
            while (_curveT + deltaT > 1)
            {
                dist -= (1 - _curveT) * _currCurve.Length();
                deltaT = dist / _currCurve.Length();
                _curveT = 0;
                UpdateCurve();
            }

            _curveT += deltaT;
            _position = _currCurve.Point(_curveT);
            _rotationAngle = _currCurve.TangentAngle(_curveT);
            gameObject.transform.SetLocalPositionAndRotation(new Vector3(_position.x / 4, 49.5f, _position.y / 4),
                Quaternion.Euler(0f, Mathf.Rad2Deg * _rotationAngle, 0f));
        }

        private void UpdateCurve()
        {
            if (_path.Count == 0)
            {
                var source = _pathEnd;
                var (path, target) = PathFinder.RandomPath(_navGraph, source);
                foreach (var curve in path)
                {
                    _path.Enqueue(curve);
                }
                _pathEnd = target;
                // var nextEdge = _navGraph.GetNextEdge(_pathEnd);
                // if (_navGraph.GetVertex(_pathEnd).tag == VertexTag.Room)
                // {
                //     _path.Enqueue(GenerateRoomCurve(_currCurve.Point(1), nextEdge.Curve.Point(0)));
                // }
                // _pathEnd = nextEdge.ToVertex;
                // _path.Enqueue(nextEdge.Curve);
            }
            _currCurve = _path.Dequeue();
        }
    }
}