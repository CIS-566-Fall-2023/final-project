using System;
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
        [Tooltip("Identifier used for Rule. Make sure this is unique for each Prefab.")]
        string itemName;

        [SerializeField]
        WFCType type;

        [SerializeField]
        WFCRule rule;

        /// <summary>
        /// Available meshes. Randomly select one when being placed.
        /// </summary>
        [SerializeField]
        List<Mesh> meshes;

        public WFCType Type { get { return type; } }
        public WFCRule Rule { get { return rule; } }

        public float Entropy(IWFCCell cell)
        {
            throw new System.NotImplementedException();
        }
    }
}