using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Planetile
{
    public interface IWFCTile
    {
        int Index { get; }
        /// <summary>
        /// Hexgon: 6, Pentagon: 5
        /// </summary>
        int EdgeNum { get; }
        /// <summary>
        /// Get the adjacency index of a tile.
        /// </summary>
        /// <param name="tile"></param>
        /// <returns>If not adjacent, return -1. </returns>
        int Adjacency(IWFCTile tile);
        IEnumerable<IWFCCell> Cells { get; }
        //IWFCTile[] GetAdjacentTiles();
        IEnumerable<IWFCCell> GetCellsOnEdge(int edge);
    }
}