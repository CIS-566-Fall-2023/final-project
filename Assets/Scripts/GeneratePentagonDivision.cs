using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePentagonDivision : MonoBehaviour
{
    [SerializeField]
    List<Vector3> positions = new List<Vector3>();
    void Start()
    {
        float a = 1f;
        float b = a * Mathf.Tan(54f * Mathf.Deg2Rad);
        float c = a / Mathf.Cos(54f * Mathf.Deg2Rad);
        var verts = new Vector3[5];
        var rot = Quaternion.Euler(0f, -72f, 0f);
        verts[0] = new Vector3(a, 0f, -b);
        verts[1] = rot * verts[0];
        verts[2] = rot * verts[1];
        verts[3] = rot * verts[2];
        verts[4] = rot * verts[3];
        DivideATriangle(Vector3.zero, verts[0], verts[1], ref positions, 1);
        DivideATriangle(Vector3.zero, verts[1], verts[2], ref positions, 2);
        DivideATriangle(Vector3.zero, verts[2], verts[3], ref positions, 3);
        DivideATriangle(Vector3.zero, verts[3], verts[4], ref positions, 4);
        DivideATriangle(Vector3.zero, verts[4], verts[0], ref positions, 0);
    }
    void DivideATriangle(Vector3 vert0, Vector3 vert1, Vector3 vert2, ref List<Vector3> positions, int pentagonDir)
    {
        // 计算每条边的中点
        Vector3 midPoint01 = (vert0 + vert1) / 2;
        Vector3 midPoint12 = (vert1 + vert2) / 2;
        Vector3 midPoint20 = (vert2 + vert0) / 2;

        // 添加四个小三角形的重心到列表中
        // 原三角形的三个顶点和三个中点形成四个小三角形

        // 小三角形1：vert0, midPoint01, midPoint20
        positions.Add((vert0 + midPoint01 + midPoint20) / 3);
        {
            var go = new GameObject("cell" + (positions.Count - 1).ToString());
            go.transform.position = positions[positions.Count - 1];
            go.transform.parent = transform;
            var cell = go.AddComponent<Cell>();
            cell.bottomFlat = true;
            cell.pentagonDirection = pentagonDir;
        }


        // 小三角形2：vert1, midPoint01, midPoint12
        positions.Add((vert1 + midPoint01 + midPoint12) / 3);
        {
            var go = new GameObject("cell" + (positions.Count - 1).ToString());
            go.transform.position = positions[positions.Count - 1];
            go.transform.parent = transform;
            var cell = go.AddComponent<Cell>();
            cell.bottomFlat = true;
            cell.pentagonDirection = pentagonDir;
        }

        // 小三角形3：vert2, midPoint12, midPoint20
        positions.Add((vert2 + midPoint12 + midPoint20) / 3);
        {
            var go = new GameObject("cell" + (positions.Count - 1).ToString());
            go.transform.position = positions[positions.Count - 1];
            go.transform.parent = transform;
            var cell = go.AddComponent<Cell>();
            cell.bottomFlat = true;
            cell.pentagonDirection = pentagonDir;
        }

        // 小三角形4：midPoint01, midPoint12, midPoint20
        positions.Add((midPoint01 + midPoint12 + midPoint20) / 3);

        {
            var go = new GameObject("cell" + (positions.Count - 1).ToString());
            go.transform.position = positions[positions.Count - 1];
            go.transform.parent = transform;
            var cell = go.AddComponent<Cell>();
            cell.bottomFlat = false;
            cell.pentagonDirection = pentagonDir;
        }
    }
}
