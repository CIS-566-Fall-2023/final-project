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

        // ����
        Vector3[] vertices = new Vector3[7]; // 6���ⲿ���� + 1�����ĵ�
        int[] triangles = new int[6 * 3]; // 6�������Σ�ÿ��������3������
        Vector2[] uvs = new Vector2[7]; // UV����

        // �������ĵ�
        vertices[0] = Vector3.zero;
        uvs[0] = new Vector2(0.5f, 1f); // ���ĵ��UV

        // �����ⲿ����
        for (int i = 0; i < 6; i++)
        {
            float angleDeg = 60 * i;
            float angleRad = Mathf.Deg2Rad * angleDeg;
            vertices[i + 1] = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0);

            // ����UV����
            //uvs[i + 1] = new Vector2(
            //    0.5f + 0.5f * Mathf.Cos(angleRad),
            //    0.5f + 0.5f * Mathf.Sin(angleRad)
            //);
            uvs[i + 1] = new Vector2(1f, 0f);
        }

        // ����������
        for (int i = 0; i < 6; i++)
        {
            int triangleIndex = i * 3;
            triangles[triangleIndex] = 0; // ���ĵ�
            triangles[triangleIndex + 1] = i + 1;
            triangles[triangleIndex + 2] = i < 5 ? i + 2 : 1;
        }

        // Ӧ�ö��㡢�����κ�UV����
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        mesh.RecalculateNormals();
    }
}
