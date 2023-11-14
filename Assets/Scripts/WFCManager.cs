using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.VersionControl;
using UnityEngine;

namespace Planetile
{
    using Wave = Dictionary<Item, float>;
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

        /// <summary>
        /// Try to fill a tile's cells. 
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="startingCells"></param>
        public void FillOneTile(Tile tile, Cell[] startingCells)
        {
            while (startingCells.Length > 0)
            {
                foreach (var cell in startingCells)
                {
                    CollapseCell(cell);
                }
                var tmp = new HashSet<Cell>();
                for (int i = 0; startingCells[i]; i++)
                {
                    foreach (var cell in startingCells[i].GetAdjacentCells())
                    {
                        if (tile.HasCell(cell) && !cell.IsPlaced)
                        {
                            tmp.Add(cell);
                        }
                    }
                }
                startingCells = tmp.ToArray();
            }
        }
        public void CollapseCell(Cell cell)
        {
            var wave = GenerateWave(cell);
            if (wave.Count == 0)
                Debug.LogWarning("No item is placed.", cell);

            float totalWeight = 0f;
            foreach (var w in wave.Values)
            {
                totalWeight += w;
            }
            float randomValue = Random.Range(0f, totalWeight);
            foreach (var pair in wave)
            {
                randomValue -= pair.Value;
                if (randomValue <= 0)
                {
                    cell.PlaceItem(pair.Key);
                }
            }
        }
        Wave GenerateWave(Cell cell)
        {
            Wave ret = new Wave();
            var neighbors = cell.GetAdjacentCells();
            foreach (var item in itemPool)
            {
                float entropy = item.Entropy(neighbors);
                if (entropy > 0) ret.Add(item, entropy);
            }
            return ret;
        }
    }
}
