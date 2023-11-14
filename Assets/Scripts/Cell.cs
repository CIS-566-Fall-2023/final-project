using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Planetile
{
    public enum CellType
    {
        Ocean = 0x01,
        Desert = 0x01 << 1,
        Tree = 0x01 << 2,
        House = 0x01 << 3,
        PentagonOcean = 0x01 << 4,
        PentagonDesert = 0x01 << 5,
        PentagonTree = 0x01 << 6,
        PentagonHouse = 0x01 << 7,
        Null = 0xFFFF,
    }
    public class Cell : MonoBehaviour
    {
        [SerializeField]
        Tile tile;
        [SerializeField]
        CellType type;
        public CellType Type
        {
            get { return type; }
        }
        public Tile Tile
        {
            get { return tile; }
        }

        public bool IsPlaced { get; private set; }
        public void PlaceItem(Item item)
        {
            throw new System.NotImplementedException();
        }

        public Cell[] GetAdjacentCells()
        {
            throw new System.NotImplementedException();
        }
        public Cell[] GetAdjacentCellsInTile(Tile tile)
        {
            throw new System.NotImplementedException();
        }
        public bool IsAdjacnet(Cell cell)
        {
            throw new System.NotImplementedException();
        }
        public Vector3 GetWorldPos()
        {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// Get the distance between two cells
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public float Distance(Cell cell)
        {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// Get the direction from one cell to another cell. [0-2pi).
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public float Direction(Cell cell)
        {
            throw new System.NotImplementedException();
        }
    }
}