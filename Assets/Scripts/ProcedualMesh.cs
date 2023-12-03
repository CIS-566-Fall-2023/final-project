using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class ProcedualMesh : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    // Start is called before the first frame update
    void Start()
    {
        MakeMeshData();
        CreateMesh();
    }

    void MakeMeshData()
    {
        vertices = new Vector3[] { new Vector3(0, 0, 0), new Vector3(5, 0, 8.66f), new Vector3(10, 0, 0) };
        triangles = new int[] { 0, 1, 2 };
    }

    void CreateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }
}
