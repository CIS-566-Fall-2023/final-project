using Planetile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cell : MonoBehaviour, IWFCCell
{
    public int pentagonDirection = -1;
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
        if (pentagonDirection == -1)
        {
            verts[0] = transform.localPosition;
            verts[0].z += bottomFlat ? 0.5773502691896258f : -0.5773502691896258f/* * transform.lossyScale.x*/;
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
        else
        {
            switch (pentagonDirection)
            {
                default:
                case 0:
                    Gizmos.color = Color.red;
                    break;
                case 1:
                    Gizmos.color = Color.yellow;
                    break;
                case 2:
                    Gizmos.color = Color.blue;
                    break;
                case 3:
                    Gizmos.color = Color.white;
                    break;
                case 4:
                    Gizmos.color = Color.magenta;
                    break;
            }
            Vector3 fwd = new Vector3(0f, 0f, 1f);

            var rot = Quaternion.Euler(0f, -72f * pentagonDirection, 0f);
            fwd = rot * fwd;
            if (!bottomFlat) { fwd = -fwd; };
            var right = Vector3.Normalize(Vector3.Cross(fwd, Vector3.up));
            verts[0] = transform.position;
            verts[1] = verts[0] + fwd * 0.3f;

            verts[0] = transform.localPosition + fwd * 0.5773502691896258f;
            verts[1] = transform.localPosition - fwd * 0.2886751345948129f + right * 0.5f;
            verts[2] = transform.localPosition - fwd * 0.2886751345948129f - right * 0.5f;

            Gizmos.matrix = transform.parent.localToWorldMatrix;
            Gizmos.DrawLine(verts[0], verts[1]);
            Gizmos.DrawLine(verts[1], verts[2]);
            Gizmos.DrawLine(verts[2], verts[0]);

            //Gizmos.DrawLine(verts[0], verts[1]);
        }
    }
}
