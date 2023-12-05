using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class HexagonTile : MonoBehaviour
{
    public GameObject cellPrefab;
    public int ID;
    public List<int> connectedTiles; //For Record
    public List<Cell> Cells;
    public List<Cell>[] CellsOnTileEdges;
    public Vector3 position;
    public Vector3 normal;
    public Quaternion rotation;

    public void SetupTile(Vector3 pos, List<sCorner> corners, sTile self)
    {

        position = pos;
        transform.position = pos;
        ID = self.id;
        connectedTiles = new List<int>();
        for (int i = 0; i < self.adjTiles.Count; i++)
            connectedTiles.Add(self.adjTiles[i].id);

        SetupMesh(corners);

    }
    public void SetupMesh(List<sCorner> corners)
    {
        Vector3[] vertices = new Vector3[corners.Count];
        for (int i = 0; i < corners.Count; i++)
        {
            vertices[i] = corners[i].position - transform.position;
        }
        int[] indices;
        if (corners.Count == 6)
            indices = new int[12];
        else
            indices = new int[9];

        if (corners.Count == 6)
        {
            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 2;

            indices[3] = 0;
            indices[4] = 2;
            indices[5] = 3;

            indices[6] = 0;
            indices[7] = 3;
            indices[8] = 5;

            indices[9] = 5;
            indices[10] = 3;
            indices[11] = 4;

        }
        else
        {
            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 2;

            indices[3] = 0;
            indices[4] = 2;
            indices[5] = 3;

            indices[6] = 0;
            indices[7] = 3;
            indices[8] = 4;
        }

        normal = Vector3.Cross(vertices[1] - vertices[0], vertices[1] - vertices[3]).normalized;
        rotation = Quaternion.LookRotation(normal);

        Mesh mesh = new Mesh();
        if (!transform.GetComponent<MeshFilter>() || !transform.GetComponent<MeshRenderer>())
        {
            transform.gameObject.AddComponent<MeshFilter>();
            transform.gameObject.AddComponent<MeshRenderer>();
        }
        transform.GetComponent<MeshFilter>().mesh = mesh;
        //transform.GetComponent<MeshCollider>().sharedMesh = transform.GetComponent<MeshFilter>().mesh;
        mesh.name = "NewAddedMesh";

        mesh.vertices = vertices;
        mesh.triangles = indices;

        mesh.RecalculateNormals();
        mesh.Optimize();
    }
    /// <summary>
    /// cash cell positions and directions
    /// </summary>
    Dictionary<int, List<System.Tuple<Vector3, bool>>> hexgonCellDict = new Dictionary<int, List<System.Tuple<Vector3, bool>>>();
    /// <summary>
    /// cash cell neighbor indices
    /// </summary>
    Dictionary<int, int[]> cellAdjs = new Dictionary<int, int[]>();

    public void SetupCellsInEquilateralTriangle(ref List<System.Tuple<Vector3, bool>> listOut, Vector3 center, bool bottomFlat, int remaining)
    {
        if (remaining == 0)
        {
            listOut.Add(new System.Tuple<Vector3, bool>(center, bottomFlat));
        }
        else
        {
            Vector3 pos1 = new Vector3(center.x + (float)remaining * 0.5f, center.y, center.z + (float)remaining * (bottomFlat ? -0.2886751345948129f : 0.2886751345948129f));
            Vector3 pos2 = new Vector3(center.x - (float)remaining * 0.5f, center.y, center.z + (float)remaining * (bottomFlat ? -0.2886751345948129f : 0.2886751345948129f));
            Vector3 pos3 = new Vector3(center.x, center.y, center.z + (float)remaining * (bottomFlat ? 0.5773502691896258f : -0.5773502691896258f));
            if (remaining == 1)
            {
                listOut.Add(new System.Tuple<Vector3, bool>(center, !bottomFlat));
                listOut.Add(new System.Tuple<Vector3, bool>(pos1, bottomFlat));
                listOut.Add(new System.Tuple<Vector3, bool>(pos2, bottomFlat));
                listOut.Add(new System.Tuple<Vector3, bool>(pos3, bottomFlat));
            }
            else
            {
                SetupCellsInEquilateralTriangle(ref listOut, center, !bottomFlat, remaining - 1);
                SetupCellsInEquilateralTriangle(ref listOut, pos1, bottomFlat, remaining - 1);
                SetupCellsInEquilateralTriangle(ref listOut, pos2, bottomFlat, remaining - 1);
                SetupCellsInEquilateralTriangle(ref listOut, pos3, bottomFlat, remaining - 1);
            }
        }
    }
    public void SetupLocalCellsPositionHexagon(int cellDensity)
    {
        var list = new List<System.Tuple<Vector3, bool>>();
        SetupCellsInEquilateralTriangle(ref list, new Vector3((float)cellDensity * 0.5f, 0f, (float)cellDensity * 0.2886751345948129f), true, cellDensity - 1);
        SetupCellsInEquilateralTriangle(ref list, new Vector3(-(float)cellDensity * 0.5f, 0f, (float)cellDensity * 0.2886751345948129f), true, cellDensity - 1);
        SetupCellsInEquilateralTriangle(ref list, new Vector3((float)cellDensity * 0.5f, 0f, -(float)cellDensity * 0.2886751345948129f), false, cellDensity - 1);
        SetupCellsInEquilateralTriangle(ref list, new Vector3(-(float)cellDensity * 0.5f, 0f, -(float)cellDensity * 0.2886751345948129f), false, cellDensity - 1);
        SetupCellsInEquilateralTriangle(ref list, new Vector3(0f, 0f, (float)cellDensity * 0.5773502691896258f), false, cellDensity - 1);
        SetupCellsInEquilateralTriangle(ref list, new Vector3(0f, 0f, -(float)cellDensity * 0.5773502691896258f), true, cellDensity - 1);
        hexgonCellDict.Add(cellDensity, list);
    }

    public void SetupCells(int cellDensity, List<sCorner> corners)
    {
        Vector3 center = new Vector3(0, 0, 0);
        foreach (var corner in corners)
        {
            center += corner.position;
        }
        center /= corners.Count;
        if (cellDensity <= 0 || (corners.Count != 5 && corners.Count != 6))
        {
            return;
        }
        CellsOnTileEdges = new List<Cell>[corners.Count];
        if (corners.Count == 6)
        {
            if (cellDensity <= 1)
            {
                Debug.LogError($"cell density must larger than 1");
            }
            else
            {
                var _rotation = Quaternion.LookRotation(Vector3.Normalize((corners[0].position + corners[1].position) * 0.5f - transform.position), normal);
                float edgeLength = Vector3.Magnitude(transform.position - corners[0].position) / cellDensity;
                if (!hexgonCellDict.ContainsKey(cellDensity))
                    SetupLocalCellsPositionHexagon(cellDensity);
                var cellProps = hexgonCellDict[cellDensity];
                for (int i = 0; i < cellProps.Count; ++i)
                {
                    var cellPos = cellProps.ElementAt(i).Item1;
                    GameObject cellGO = Instantiate(cellPrefab);
                    var cell = cellGO.GetOrAddComponent<Cell>();
                    cell.BottomFlat = cellProps.ElementAt(i).Item2;
                    Cells.Add(cell);


                    // TODO: Fix position and rotation.
                    Vector3 newPos = cellPos * edgeLength;
                    newPos = _rotation * newPos;
                    cellGO.transform.position = newPos + center;
                    cellGO.transform.parent = transform;
                    cellGO.name = i.ToString();
                }
            }
        }
        else
        {
            // TODO: Îå±ßÐÎ
            for (int i = 0; i < corners.Count; i++)
            {
                Vector3 p1 = corners[i].position;
                Vector3 p2 = corners[(i + 1) % corners.Count].position;

                for (int j = 0; j < cellDensity+1; j++)
                {
                    for (int k = 0; k < cellDensity+1 - j; k++)
                    {
                        float alpha = j / ((float)cellDensity + 1);
                        float beta = k / ((float)cellDensity + 1);

                        Vector3 point = (1 - alpha - beta) * center + alpha * p1 + beta * p2;
                        //cellPosList.Add(point);
                    }
                }
            }
        }
    }
}
