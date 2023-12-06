using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

namespace Planetile
{

    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [DisallowMultipleComponent]
    public class Item : MonoBehaviour, IWFCItem
    {
        [SerializeField]
        [Tooltip("Identifier used for Rule. Make sure this is unique for each Prefab.")]
        string itemName;

        public string ItemName => itemName;

        [SerializeField]
        WFCType type;

        [SerializeField]
        WFCRule[] rules;

        /// <summary>
        /// Available meshes. Randomly select one when being placed.
        /// </summary>
        [SerializeField]
        Mesh mesh;

        Vector3[] vertices;
        int[] triangles;
        Vector2[] uvs;

        public bool isPentagon = false;

        public WFCType Type { get { return type; } }

        public WFCRule[] Rules => rules;

        private void Awake()
        {
            mesh = GetComponent<MeshFilter>().mesh;
        }

        void Start()
        {
            if (isPentagon) MakePentagonTriangleMeshData();
            else MakeMeshData();
            CreateMesh();
        }

        void MakePentagonTriangleMeshData()
        {
            vertices = new Vector3[] { new Vector3(0, 0, 0), new Vector3(5, 0, 6.88f), new Vector3(10, 0, 0) };
            triangles = new int[] { 0, 1, 2 };
            uvs = new Vector2[] { new Vector2(0, 0), new Vector2(0.5f, 1), new Vector2(1, 0) };
        }

        void MakeMeshData()
        {
            vertices = new Vector3[] { new Vector3(0, 0, 0), new Vector3(5, 0, 8.66f), new Vector3(10, 0, 0) };
            triangles = new int[] { 0, 1, 2 };
            uvs = new Vector2[] { new Vector2(0, 0), new Vector2(0.5f, 1), new Vector2(1, 0) };
        }

        void CreateMesh()
        {
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
        }

        public float Entropy(IWFCCell cell)
        {
            var neighbors = cell.GetAdjacentCells();
            float ret = 1f;
            bool success = false;
            foreach (var rule in rules)
            {
                success = success || rule.ApplyRule(ref ret, neighbors);
            }
            if (success) return ret;
            else return 0f;
        }
    }
}