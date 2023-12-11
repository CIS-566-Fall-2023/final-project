using BinaryPartition;
using Geom;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

namespace Navigation
{
    public class Wanderer : MonoBehaviour
    {
        private Vector2 _position;
        private float _rotationAngle;
        private Graph _navGraph;

        private int _prevVertex;
        private int _nextVertex;
        private ICurve _currCurve;
        private float _curveT;

        private bool _inRoom = false;

        public GameObject particleSys;

        private const float _speed = 10f;

        public void Initialize(Graph navGraph)
        {
            Instantiate(particleSys, gameObject.transform);
            _navGraph = navGraph;
            _prevVertex = navGraph.GetRandomEdge().ToVertex;
            var edge = navGraph.GetNextEdge(_prevVertex, _prevVertex);
            _currCurve = edge.Curve;
            _nextVertex = edge.ToVertex;
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
                FindNextCurve();
            }

            _curveT += deltaT;
            _position = _currCurve.Point(_curveT);
            _rotationAngle = _currCurve.TangentAngle(_curveT);
            gameObject.transform.SetLocalPositionAndRotation(new Vector3(_position.x / 4, 49.5f, _position.y / 4),
                Quaternion.Euler(0f, Mathf.Rad2Deg * _rotationAngle, 0f));
        }

        private void FindNextCurve()
        {
            // if (_navGraph.GetVertex(_nextVertex).tag == VertexTag.Room)
            // {
            //     _inRoom = true;
            //     _currCurve = GenerateRoomCurve();
            // }
            var edge = _navGraph.GetNextEdge(_prevVertex, _nextVertex);
            _prevVertex = _nextVertex;
            _nextVertex = edge.ToVertex;
            _currCurve = edge.Curve;
        }

        // private void Move(ICurve curve, float t)
        // {
        //     _position = curve.Point(t);
        //     _rotationAngle = curve.TangentAngle(t);
        // }

        ICurve GenerateRoomCurve(Vector2 entrance, Vector2 exit)
        {
            return new LineCurve(entrance, exit);
        }

        // private IEnumerator EnterRoom(EdgeInfo edge)
        // {
        //     
        //     var startPoint = currEdge.Curve.Point(1);
        //     var exit = path.Pop();
        //     var endPoint = exit.Curve.Point(0);
        //
        //     var roomVertex = _navGraph._vertices[edge.ToVertex].region;
        //     
        //     var controlPoints = new Vector2[4];
        //     controlPoints[0] = startPoint;
        //     controlPoints[1] = roomVertex.RandPoint();
        //     controlPoints[2] = roomVertex.RandPoint();
        //     controlPoints[3] = endPoint;
        //
        //     var bezier = new BezierCurve(controlPoints);
        //     while (Time.time - lerpStartTime < lerpDurationRoom)
        //     {
        //         float t = (Time.time - lerpStartTime) / lerpDuration;
        //         Move(bezier, t);
        //         yield return null;
        //     }
        //     Move(bezier, 1);
        //     yield return null;
        // }
    }
}