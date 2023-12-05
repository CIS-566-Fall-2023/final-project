using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.VersionControl;
using UnityEngine;

namespace Planetile
{
    using Wave = SortedDictionary<float, System.Tuple<IWFCCell, Item>>;
    public class WFCManager : MonoBehaviour
    {
        private static WFCManager instance;
        public static WFCManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType(typeof(WFCManager)) as WFCManager;
                    if (instance == null)
                    {
                        GameObject go = GameObject.Find("GameManager");
                        if (go == null)
                        {
                            go = new GameObject("GameManager");
                            go.tag = "GameController";
                            DontDestroyOnLoad(go);
                        }
                        instance = go.AddComponent<WFCManager>();
                    }
                }
                return instance;
            }
        }

        [SerializeField]
        List<Item> itemPool;
        [SerializeField]
        internal Vector3 hexagonItemLocalScale = Vector3.one;
        [SerializeField]
        internal Vector3 hexagonItemPositionOffset = Vector3.zero;

        public int ItemNum => itemPool.Count;
        public Item this[int index] => itemPool[index];

        /// <summary>
        /// Try to fill a tile's cells. Starting from cells on edges. 
        /// </summary>
        /// <param name="tile"></param>
        public void FillOneTile(IWFCTile tile)
        {
            var startingCells = new HashSet<IWFCCell>();
            for (int i = 0; i < tile.EdgeNum; ++i)
            {
                startingCells.UnionWith(tile.GetCellsOnEdge(i));
            }
            while (startingCells.Count > 0)
            {
                var cell = Collapse(startingCells);
                if (cell != null)
                {
                    startingCells.Remove(cell);
                    foreach (var i in cell.GetAdjacentCellsInTile(tile))
                    {
                        if (i.IsPlaced)
                        {
                            startingCells.Add(i);
                        }
                    }
                }
                else
                {
                    Debug.LogError("Failed to fill the tile.", tile as Object);
                }
            }
        }
        /// <summary>
        /// place an item to a cell
        /// </summary>
        /// <param name="cell"></param>
        public bool Collapse(IWFCCell cell)
        {
            var wave = new SortedDictionary<float, IWFCItem>();
            foreach (var item in itemPool)
            {
                // if it's null type, we can use any kind of item.
                if (cell.Type != WFCType.Null && cell.Type != item.Type) continue;
                float entropy = item.Entropy(cell);
                if (entropy > 0)
                {
                    wave.Add(entropy, item);
                }
            }
            if (wave.Count == 0)
            {
                Debug.LogWarning($"nothing is placed to {cell}");
                return true;
            }
            float totalWeight = 0f;
            foreach (var w in wave.Keys)
            {
                totalWeight += w;
            }
            float randomValue = Random.Range(0f, totalWeight);
            foreach (var pair in wave.Reverse())
            {
                randomValue -= pair.Key;
                if (randomValue <= 0)
                {
                    cell.PlaceItem(pair.Value);
                    Debug.Log($"{pair.Value} is placed to {cell}");
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// In a bunch of cells, fill a cell with an item.
        /// </summary>
        /// <param name="cells"></param>
        /// <returns></returns>
        public IWFCCell Collapse(IEnumerable<IWFCCell> cells)
        {
            if (itemPool != null && itemPool.Count > 0)
            {
                var waves = new Wave();
                foreach (var cell in cells)
                {
                    int count = 0;
                    foreach (var item in itemPool)
                    {
                        // if it's null type, we can use any kind of item.
                        if (cell.Type != WFCType.Null && cell.Type != item.Type) continue;
                        float entropy = item.Entropy(cell);
                        if (entropy > 0)
                        {
                            waves.Add(entropy, new System.Tuple<IWFCCell, Item>(cell, item));
                            count++;
                        }
                    }
                    if (count == 0)
                        Debug.LogWarning("No item is placed.");
                }

                float totalWeight = 0f;
                foreach (var w in waves.Keys)
                {
                    totalWeight += w;
                }
                float randomValue = Random.Range(0f, totalWeight);
                foreach (var pair in waves.Reverse())
                {
                    randomValue -= pair.Key;
                    if (randomValue <= 0)
                    {
                        pair.Value.Item1.PlaceItem(pair.Value.Item2);
                        return pair.Value.Item1;
                    }
                }
                return null;
            }
            return null;
        }
    }
}
