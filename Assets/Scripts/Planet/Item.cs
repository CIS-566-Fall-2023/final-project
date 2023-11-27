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

        public string ItemName => itemName;

        [SerializeField]
        WFCType type;

        [SerializeField]
        WFCRule[] rules;

        /// <summary>
        /// Available meshes. Randomly select one when being placed.
        /// </summary>
        [SerializeField]
        List<Mesh> meshes;

        public WFCType Type { get { return type; } }

        public WFCRule[] Rules => throw new NotImplementedException();

        public float Entropy(IWFCCell cell)
        {
            var neighbors = cell.GetAdjacentCells();
            float ret = 1f;
            foreach (var rule in rules)
            {
                rule.ApplyRule(ref ret, neighbors);
            }
            return ret;
        }
    }
}