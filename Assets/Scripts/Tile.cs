using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Planetile
{
    public class Tile : MonoBehaviour
    {
        [SerializeField]
        bool isHexagon = true;
        [SerializeField]
        Cell[] cells /*= new Cell[TILE_CELL_NUM]*/;

        public bool IsHexagon { get { return IsHexagon; } }
        public IEnumerable<Cell> Cells { get { return cells; } }
        public bool HasCell(Cell cell)
        {
            throw new System.NotImplementedException();
        }
        public Tile[] GetAdjacentTiles()
        {
            throw new System.NotImplementedException();
        }
    }
}