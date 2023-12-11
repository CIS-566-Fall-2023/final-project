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

        private List<Wanderer> wanderers = new List<Wanderer>();
        private Graph navGraph;
        // private PathFinder pathFinder;
        private bool isInitialized = false;
        public GameObject particleSys;

        public void Initialize(Graph graph) {
            navGraph = graph;
            isInitialized = true;
        }

        void Start(){

            if (!isInitialized){
                return;
            }
            // pathFinder = new PathFinder(navGraph);
            for (int i = 0; i < 5; i++){
                var start = navGraph.GetRandomEdge();
                var end = navGraph.GetRandomEdge();
                string wandererName = "Wanderer_" + i.ToString();
                var wander = new GameObject(wandererName).AddComponent<Wanderer>();
                //float scale = .25f;
                //wander.transform.localScale.Scale(new Vector3(scale, scale, scale));
                wander.particleSys = particleSys;
                wander.Initialize(navGraph);
                //wander.Initialize(navGraph, start, end, pathFinder);

                wanderers.Add(wander);
                //_drawables.Add(new DebugSquare() {position = wander.Position});
            }
        }

        //void Update() {
        //    if (!isInitialized) {
        //        return;
        //    }
        //    //_drawables = new();
        //    foreach (var wanderer in wanderers) {
        //        _drawables.Add(new DebugSquare() {position = wanderer.Position});
        //    }
        //    foreach (var drawable in _drawables) {
        //        drawable.Draw();
        //    }
        //    }
        }

        
    }


