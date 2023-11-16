using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Planetile
{
    public enum WFCType
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
    public interface IWFCCell
    {
        WFCType Type { get; }
        IWFCTile Tile { get; }
        IWFCItem Item { get; }
        /// <summary>
        /// Any item placed on this cell?
        /// </summary>
        bool IsPlaced { get; }

        /// <summary>
        /// Place one item and change this cell's type to item's.
        /// </summary>
        /// <param name="item"></param>
        void PlaceItem(Item item);
        int EdgeNum { get; }

        IWFCCell[] GetAdjacentCells();
        IWFCCell[] GetAdjacentCellsInTile(IWFCTile tile);
        /// <summary>
        /// Get the adjacency index of a cell.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns>If not adjacent, return -1. </returns>
        int Adjacency(IWFCCell cell);
        Vector3 GetWorldPos();
        ///// <summary>
        ///// Get the distance between two cells
        ///// </summary>
        ///// <param name="cell"></param>
        ///// <returns></returns>
        //float Distance(IWFCCell cell);
        ///// <summary>
        ///// Get the direction from one cell to another cell. [0-2pi).
        ///// </summary>
        ///// <param name="cell"></param>
        ///// <returns></returns>
        //float Direction(IWFCCell cell);
    }
}