using Planetile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cell : MonoBehaviour, IWFCCell
{
    [SerializeField]
    WFCType type;

    [SerializeField]
    IWFCTile tile;
    [SerializeField]
    Item item;
    [SerializeField]
    int edgeNum = 3;
    [SerializeField]
    Cell[] neighbours = new Cell[3];
    public WFCType Type => type;
    public IWFCTile Tile => tile;

    public IWFCItem Item => item;

    public bool IsPlaced => item != null;   

    public int EdgeNum => edgeNum;

    public IWFCCell[] GetAdjacentCells()
    {
        return neighbours;
    }

    public IWFCCell[] GetAdjacentCellsInTile(IWFCTile tile)
    {
        throw new System.NotImplementedException();
    }

    public Vector3 GetWorldPos()
    {
        return transform.position;
    }
    //public void PlaceItem(IWFCItem _item) { Item __item = _item as Item; if (__item != null) PlaceItem(__item); }

    public void PlaceItem(IWFCItem _item)
    {
        Item __item = _item as Item;
        if (this.item == null || this.item.ItemName != __item.ItemName)
        {
            this.type = __item.Type;
            this.item = Instantiate(__item, this.transform);
        }
    }
}
