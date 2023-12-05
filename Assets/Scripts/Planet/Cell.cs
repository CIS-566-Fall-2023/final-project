using Planetile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cell : MonoBehaviour, IWFCCell
{
    /// <summary>
    /// the direction of the triangle
    /// </summary>
    public bool bottomFlat = false;
    [SerializeField]
    WFCType type;
    [SerializeField]
    IWFCTile tile;
    [SerializeField]
    public Item item;
    [SerializeField]
    int edgeNum = 3;
    [SerializeField]
    public Cell[] neighbors = new Cell[3];
    public WFCType Type => type;
    public IWFCTile Tile => tile;

    public IWFCItem Item => item;

    public bool IsPlaced => item != null;   

    public int EdgeNum => edgeNum;

    public IWFCCell[] GetAdjacentCells()
    {
        return neighbors;
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
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position, 0.02f);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        var verts = new Vector3[3];
        verts[0] = transform.localPosition;
        verts[0].z += bottomFlat? 0.5773502691896258f : -0.5773502691896258f/* * transform.lossyScale.x*/;
        verts[1] = transform.localPosition;
        verts[1].x += 0.5f;
        verts[1].z += bottomFlat ? -0.2886751345948129f : 0.2886751345948129f/* * transform.lossyScale.z*/;
        verts[2] = transform.localPosition;
        verts[2].x -= 0.5f;
        verts[2].z += bottomFlat ? -0.2886751345948129f : 0.2886751345948129f/* * transform.lossyScale.z*/;
        //Gizmos.matrix = Matrix4x4.Rotate(transform.rotation);
        Gizmos.matrix = transform.parent.localToWorldMatrix;
        Gizmos.DrawLine(verts[0], verts[1]);
        Gizmos.DrawLine(verts[1], verts[2]);
        Gizmos.DrawLine(verts[2], verts[0]);
    }
}
