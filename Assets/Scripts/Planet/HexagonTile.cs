using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using Planetile;
using static UnityEngine.UI.GridLayoutGroup;

public class HexagonTile : MonoBehaviour, IWFCTile
{
    public GameObject hexagonCellsPrefab;
    public GameObject pentagonCellsPrefab;
    public HardcodedCells cellData;
    public bool hasSelected = false;
    public int ID;
    public List<int> connectedTiles; //For Record
    public Vector3 position;
    public Vector3 normal;
    //public Quaternion rotation;
    public Vector3 center;
    public List<sCorner> corners;

    public void SetupTile(Vector3 pos, List<sCorner> corners, sTile self)
    {
        //this.corners = corners;
        position = pos;
        transform.position = pos;
        ID = self.id;
        connectedTiles = new List<int>();
        for (int i = 0; i < self.adjTiles.Count; i++)
            //for (int i = self.adjTiles.Count-1; i >= 0; i--)
            connectedTiles.Add(self.adjTiles[i].id);

        SetupMesh(corners);

    }
    public void SetupMesh(List<sCorner> corners)
    {
        this.corners = corners;
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

        center = new Vector3(0, 0, 0);
        foreach (var corner in corners)
        {
            center += corner.position;
        }
        center /= corners.Count;
        //normal = Vector3.Cross(vertices[1] - vertices[0], vertices[1] - vertices[3]).normalized;

        normal = Vector3.Normalize(Vector3.Cross(corners[0].position - corners[1].position, corners[0].position - corners[2].position));

        Mesh mesh = new Mesh();
        if (!transform.GetComponent<MeshFilter>() || !transform.GetComponent<MeshRenderer>())
        {
            transform.gameObject.AddComponent<MeshFilter>();
            transform.gameObject.AddComponent<MeshRenderer>();
        }
        transform.GetComponent<MeshFilter>().mesh = mesh;
        transform.GetComponent<MeshCollider>().sharedMesh = transform.GetComponent<MeshFilter>().mesh;
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

    public void SetupCells()
    {
        var lengths = new float[6];
        for (int i = 0; i < corners.Count; i++)
        {
            if (i == corners.Count - 1)
                lengths[i] = Vector3.Magnitude(corners[i].position - corners[0].position);
            else
                lengths[i] = Vector3.Magnitude(corners[i].position - corners[i + 1].position);
        }
        Debug.Log($"{lengths[0]}, {lengths[1]}, , {lengths[2]}, {lengths[3]}, {lengths[4]}, {lengths[5]}");
        if (corners.Count == 6)
        {
            //var forwardPos = SphereCreation.Instance.tiles[connectedTiles[0]].center;
            var forwardPos = (corners[0].position + corners[1].position) * 0.5f;
            var forwardDir = Vector3.Normalize(forwardPos - center);
            var _rotation = Quaternion.LookRotation(forwardDir, normal);
            float edgeLength = Vector3.Magnitude(center - corners[0].position) / 2;
            GameObject newTileGO = Instantiate(hexagonCellsPrefab);
            newTileGO.transform.parent = transform;
            cellData = newTileGO.GetComponent<HardcodedCells>();
            newTileGO.transform.position = center;
            newTileGO.transform.localScale = new Vector3(edgeLength, edgeLength, edgeLength);
            newTileGO.transform.rotation = _rotation;
            //newTileGO.transform.forward = forwardDir;
            //newTileGO.transform.up = normal;
        }
        else
        {
            float edgeLength = Vector3.Magnitude(corners[0].position - corners[1].position) * 0.5f;
            GameObject newTileGO = Instantiate(pentagonCellsPrefab);
            newTileGO.transform.parent = transform;
            cellData = newTileGO.GetComponent<HardcodedCells>();
            //oops
            WFCType type1 = (WFCType)(0x01 << (SimpleHash(1 + Random.Range(0, 1000)) + 4));
            WFCType type2 = (WFCType)(0x01 << (SimpleHash(2 + Random.Range(0, 1000)) + 4));
            WFCType type3 = (WFCType)(0x01 << (SimpleHash(3 + Random.Range(0, 1000)) + 4));
            WFCType type4 = (WFCType)(0x01 << (SimpleHash(4 + Random.Range(0, 1000)) + 4));
            WFCType type5 = (WFCType)(0x01 << (SimpleHash(5 + Random.Range(0, 1000)) + 4));

            cellData.CellsOnTileEdge0[0].Type = type1;
            cellData.CellsOnTileEdge0[1].Type = type1;
            cellData.CellsOnTileEdge1[0].Type = type2;
            cellData.CellsOnTileEdge1[1].Type = type2;
            cellData.CellsOnTileEdge2[0].Type = type3;
            cellData.CellsOnTileEdge2[1].Type = type3;
            cellData.CellsOnTileEdge3[0].Type = type4;
            cellData.CellsOnTileEdge3[1].Type = type4;
            cellData.CellsOnTileEdge4[0].Type = type5;
            cellData.CellsOnTileEdge4[1].Type = type5;

            newTileGO.transform.position = center;
            newTileGO.transform.localScale = new Vector3(edgeLength, edgeLength, edgeLength);

            var forwardPos = corners[0].position;
            var forwardDir = Vector3.Normalize(forwardPos - center);
            //newTileGO.transform.forward = -forwardDir;
            //newTileGO.transform.up = normal;
            newTileGO.transform.rotation = Quaternion.LookRotation(forwardDir, normal);
        }
    }

    private int SimpleHash(int input)
    {
        int Modulus = 4; 
        int Multiplier = 48271;
        int Increment = 214748367;

        input = (Multiplier * input + Increment) % Modulus;
        return input;
    }

    public void ConnectCellsWithOtherTile()
    {
        for (int i = 0; i < connectedTiles.Count; ++i)
        {
            var neighborTile = SphereCreation.Instance.tiles[connectedTiles[i]];
            int otherEdge = neighborTile.Adjacency(this);
            foreach (var cell in cellData.CellsOnTileEdge(i))
            {
                for (int j = 0; j < cell.neighbors.Length; j++)
                {
                    if (cell.neighbors[j] == null)
                    {
                        var otherCells = neighborTile.cellData.CellsOnTileEdge(i);
                        //cell.neighbors[j] = otherCells[0];
                        cell.neighborInOtherTile = otherCells[0];
                        for (int k = 1; k < cell.neighbors.Length; k++)
                            if (Vector3.Magnitude(otherCells[k].transform.position - cell.transform.position) <
                                Vector3.Magnitude(cell.neighbors[j].transform.position - cell.transform.position))
                            {
                                //cell.neighbors[j] = otherCells[k];
                                cell.neighborInOtherTile = otherCells[k];
                            }
                        break;
                    }
                }
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (corners != null)
        {
            Gizmos.matrix = Matrix4x4.identity;
            Gizmos.color = Color.yellow;
            foreach (var corner in corners)
            {
                Gizmos.DrawSphere(corner.position, 0.01f);
            }
            Gizmos.DrawLine(center, center + normal);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(center, center + Vector3.Normalize(corners[0].position - center));
        }
    }
    public int Index => ID;
    public int EdgeNum
    {
        get
        {
            if (corners != null) return corners.Count;
            else return 6;
        }
    }
    public int Adjacency(IWFCTile tile)
    {
        for (int i = 0; i < connectedTiles.Count; i++)
        {
            if (connectedTiles[i] == tile.Index) return i;
        }
        return -1;
    }


    IEnumerable<IWFCCell> IWFCTile.Cells => cellData.Cells;

    public IEnumerable<IWFCCell> GetCellsOnEdge(int edge)
    {
        return cellData.CellsOnTileEdge(edge);
    }
}
