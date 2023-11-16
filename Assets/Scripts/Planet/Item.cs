using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Planetile
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [DisallowMultipleComponent]
    public class Item : MonoBehaviour, IWFCItem
    {
        [SerializeField]
        WFCType type;

        /// <summary>
        /// Available meshes. Randomly select one when being placed.
        /// </summary>
        [SerializeField]
        List<Mesh> meshes;

        public WFCType Type { get { return type; } }

        public float Entropy(IWFCCell[] cells)
        {
            throw new System.NotImplementedException();
        }
    }
}