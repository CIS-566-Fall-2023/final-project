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
    public int PentagonRotationFlag = -1;
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

        // ������������εĶ���
        float baseLength = 0.5f; // �ױ߳���
        float halfBase = baseLength / 2; // �ױ�һ��ĳ���
        float topAngle = 72f; // ����
        float topAngleRad = topAngle * Mathf.Deg2Rad; // ����ת��Ϊ����

        // �����
        float height = (baseLength / 2) / Mathf.Tan(topAngleRad / 2);

        // �ױߵ���������
        Vector3 baseVertex1 = transform.position + transform.right * halfBase;
        Vector3 baseVertex2 = transform.position - transform.right * halfBase;

        // ����
        Vector3 topVertex = transform.position + transform.up * height;

        // ����������
        Gizmos.DrawLine(baseVertex1, baseVertex2);
        Gizmos.DrawLine(baseVertex2, topVertex);
        Gizmos.DrawLine(topVertex, baseVertex1);
    }
}
