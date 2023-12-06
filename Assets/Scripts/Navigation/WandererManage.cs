using System;
using System.Collections.Generic;
using BinaryPartition;
using Geom;
using GraphBuilder;
using UnityEngine;
using MyDebug;
using UnityEditor;

namespace Navigation {
    public class WandererManager : MonoBehaviour {

        private List<IDebugDrawable> _drawables = new();
        private List<Wanderer> wanderers = new List<Wanderer>();
        private Graph navGraph;
        private PathFinder pathFinder;
        private bool isInitialized = false;
        public Sprite sprite;

        public void Initialize(Graph graph) {
            navGraph = graph;
            isInitialized = true;
        }

        void Start() {

            if (!isInitialized) {
                return;
            }
            this.navGraph = navGraph;
            pathFinder = new PathFinder(navGraph);
            for (int i = 0; i < 1; i++) {
                var start = navGraph.GetRandomEdge();
                var end = navGraph.GetRandomEdge();
                string gameObjectName = "Wanderer_" + i.ToString();
                var wander = new GameObject(gameObjectName).AddComponent<Wanderer>();
                wander.Initialize(navGraph, start, end, pathFinder);
                
                FootprintTrail fpt = wander.gameObject.AddComponent<FootprintTrail>();
                fpt.sprite = sprite;

                wanderers.Add(wander);
                _drawables.Add(new DebugSquare() {position = wander.Position});
            }
        }

        void Update() {
            if (!isInitialized) {
                return;
            }
            _drawables = new();
            foreach (var wanderer in wanderers) {
                _drawables.Add(new DebugSquare() {position = wanderer.Position});
            }
            foreach (var drawable in _drawables) {
                drawable.Draw();
            }
            }
        }

        
    }


