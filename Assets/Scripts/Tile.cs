using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Planetile
{
    public class Tile : MonoBehaviour
    {
        [SerializeField]
        Cell[] cells;

        public Cell this[int index]
        {
            get
            {
                return cells[index];
            }
        }
        void OnEnable()
        {
            //cells = new Cell[TILE_CELL_NUM];
        }
    }
}