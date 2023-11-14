using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Planetile
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [DisallowMultipleComponent]
    public class Item : MonoBehaviour, IRule
    {
        [SerializeField]
        CellType type;

        /// <summary>
        /// Available meshes. Randomly select one when being placed.
        /// </summary>
        [SerializeField]
        List<Mesh> meshes;

        public CellType Type { get { return type; } }

        public float Entropy(Cell[] cells)
        {
            throw new System.NotImplementedException();
        }
    }
}