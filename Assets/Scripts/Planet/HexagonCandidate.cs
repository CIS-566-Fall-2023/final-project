using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexagonCandidate : MonoBehaviour
{
    void Start()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        // 顶点
        Vector3[] vertices = new Vector3[7]; // 6个外部顶点 + 1个中心点
        int[] triangles = new int[6 * 3]; // 6个三角形，每个三角形3个顶点
        Vector2[] uvs = new Vector2[7]; // UV坐标

        // 设置中心点
        vertices[0] = Vector3.zero;
        uvs[0] = new Vector2(0.5f, 1f); // 中心点的UV

        // 计算外部顶点
        for (int i = 0; i < 6; i++)
        {
            float angleDeg = 60 * i;
            float angleRad = Mathf.Deg2Rad * angleDeg;
            vertices[i + 1] = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0);

            // 计算UV坐标
            //uvs[i + 1] = new Vector2(
            //    0.5f + 0.5f * Mathf.Cos(angleRad),
            //    0.5f + 0.5f * Mathf.Sin(angleRad)
            //);
            uvs[i + 1] = new Vector2(1f, 0f);
        }

        // 创建三角形
        for (int i = 0; i < 6; i++)
        {
            int triangleIndex = i * 3;
            triangles[triangleIndex] = 0; // 中心点
            triangles[triangleIndex + 1] = i + 1;
            triangles[triangleIndex + 2] = i < 5 ? i + 2 : 1;
        }

        // 应用顶点、三角形和UV数据
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        mesh.RecalculateNormals();
    }
}
