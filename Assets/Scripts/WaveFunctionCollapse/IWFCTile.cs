using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Planetile
{
    public interface IWFCTile
    {
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
        IWFCTile[] GetAdjacentTiles();
        IWFCCell[] GetCellsOnEdge(int edge);
    }
}