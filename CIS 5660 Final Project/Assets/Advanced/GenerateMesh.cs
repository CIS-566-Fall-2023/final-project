using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProceduralToolkit;
using Unity.Mathematics;

[RequireComponent(typeof(MeshFilter))]
public class GenerateMesh : MonoBehaviour {

    private MeshFilter meshFilter;
    public ComputeShader noiseComputeShader;

    public Vector3 TerrainSize { get; set; }
    public float CellSize { get; set; }
    public float NoiseScale { get; set; }

    public Gradient Gradient { get; set; }

    public Vector2 NoiseOffset { get; set; }

    public UnityEngine.Vector2Int xyIndex { get; set; }

    private static bool usePerlinNoise = true;
    public static bool UsePerlinNoise { get { return usePerlinNoise; } set { usePerlinNoise = value; } }

    public struct Data
    {
        public float x;
        public float y;
        public Data(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public void Generate() {
        meshFilter = GetComponent<MeshFilter>();

        MeshDraft draft = TerrainDraft2(TerrainSize, CellSize, NoiseOffset, NoiseScale, Gradient, noiseComputeShader);
        draft.Move(Vector3.left * TerrainSize.x / 2 + Vector3.back * TerrainSize.z / 2);
        meshFilter.mesh = draft.ToMesh();

        MeshCollider meshCollider = GetComponent<MeshCollider>();
        if (meshCollider)
            meshCollider.sharedMesh = meshFilter.mesh;
    }

    private MeshDraft TerrainDraft2(Vector3 terrainSize, float cellSize, Vector2 noiseOffset, float noiseScale,
        Gradient gradient, ComputeShader computeShader)
    {
        int xSegments = Mathf.FloorToInt(terrainSize.x / cellSize);
        int zSegments = Mathf.FloorToInt(terrainSize.z / cellSize);

        float xStep = terrainSize.x / xSegments;
        float zStep = terrainSize.z / zSegments;
        int vertexCount = 6 * xSegments * zSegments;
        MeshDraft draft = new MeshDraft
        {
            name = "Terrain",
            vertices = new List<Vector3>(vertexCount),
            triangles = new List<int>(vertexCount),
            normals = new List<Vector3>(vertexCount),
            colors = new List<Color>(vertexCount)
        };

        // Populate the draft with initial data
        for (int i = 0; i < vertexCount; i++)
        {
            draft.vertices.Add(Vector3.zero);
            draft.triangles.Add(0);
            draft.normals.Add(Vector3.zero);
            draft.colors.Add(Color.black);
        }

        // Create an array for position data
        int totalSegments = xSegments * zSegments;
        Data[] positionData = new Data[totalSegments * 4];

        // Populate position data
        int j = 0;
        for (int x = 0; x < xSegments; x++)
        {
            for (int z = 0; z < zSegments; z++)
            {
                float noiseX = noiseScale * x / xSegments + noiseOffset.x;
                float noiseZ = noiseScale * z / zSegments + noiseOffset.y;

                float noiseX1 = noiseScale * (x + 1) / xSegments + noiseOffset.x;
                float noiseZ1 = noiseScale * (z + 1) / zSegments + noiseOffset.y;

                positionData[j] = new Data { x = noiseX, y = noiseZ };
                positionData[j + 1] = new Data { x = noiseX, y = noiseZ1 };
                positionData[j + 2] = new Data { x = noiseX1, y = noiseZ };
                positionData[j + 3] = new Data { x = noiseX1, y = noiseZ1 };

                j = j + 4;
            }
        }

        // Call TestMe to get the heights
        float[] heights = TestMe2(computeShader, positionData);

        // Generate mesh using the heights array
        int index = 0;
        for (int x = 0; x < xSegments; x++)
        {
            for (int z = 0; z < zSegments; z++)
            {
                //Debug.Log("generating xseg " + x + ", zseg " + z);
                int index0 = 6 * (x + z * xSegments);
                int index1 = index0 + 1;
                int index2 = index0 + 2;
                int index3 = index0 + 3;
                int index4 = index0 + 4;
                int index5 = index0 + 5;

                float height00 = heights[index];
                float height01 = heights[index + 1];
                float height10 = heights[index + 2];
                float height11 = heights[index + 3];
                index += 4;

                Vector3 vertex00 = new Vector3(x * xStep, height00 * terrainSize.y, z * zStep);
                Vector3 vertex01 = new Vector3(x * xStep, height01 * terrainSize.y, (z + 1) * zStep);
                Vector3 vertex10 = new Vector3((x + 1) * xStep, height10 * terrainSize.y, z * zStep);
                Vector3 vertex11 = new Vector3((x + 1) * xStep, height11 * terrainSize.y, (z + 1) * zStep);

                draft.vertices[index0] = vertex00;
                draft.vertices[index1] = vertex01;
                draft.vertices[index2] = vertex11;
                draft.vertices[index3] = vertex00;
                draft.vertices[index4] = vertex11;
                draft.vertices[index5] = vertex10;

                draft.colors[index0] = gradient.Evaluate(height00);
                draft.colors[index1] = gradient.Evaluate(height01);
                draft.colors[index2] = gradient.Evaluate(height11);
                draft.colors[index3] = gradient.Evaluate(height00);
                draft.colors[index4] = gradient.Evaluate(height11);
                draft.colors[index5] = gradient.Evaluate(height10);

                Vector3 normal000111 = Vector3.Cross(vertex01 - vertex00, vertex11 - vertex00).normalized;
                Vector3 normal001011 = Vector3.Cross(vertex11 - vertex00, vertex10 - vertex00).normalized;

                draft.normals[index0] = normal000111;
                draft.normals[index1] = normal000111;
                draft.normals[index2] = normal000111;
                draft.normals[index3] = normal001011;
                draft.normals[index4] = normal001011;
                draft.normals[index5] = normal001011;

                draft.triangles[index0] = index0;
                draft.triangles[index1] = index1;
                draft.triangles[index2] = index2;
                draft.triangles[index3] = index3;
                draft.triangles[index4] = index4;
                draft.triangles[index5] = index5;
            }
        }

        return draft;
    }

    private float[] TestMe2(ComputeShader compShader, Data[] positionData)
    {
        Debug.Log("Sending positionData[0].x = " + positionData[0].x + ", positionData[0].y = " + positionData[0].y + " at [" + xyIndex.x + ", " + xyIndex.y + "]");
        int kernelIndex = 0;
        int threadGroupSize = 100; // For example, you might define 64 threads per group in x dimension

        ComputeBuffer positionBuffer = new ComputeBuffer(positionData.Length, sizeof(float) * 2);
        ComputeBuffer resultBuffer = new ComputeBuffer(positionData.Length, sizeof(float));
        float[] resultData = new float[positionData.Length];

        positionBuffer.SetData(positionData);
        compShader.SetBuffer(kernelIndex, "positionData", positionBuffer);
        compShader.SetBuffer(kernelIndex, "resultData", resultBuffer);

        // Calculate the number of thread groups to dispatch
        int threadGroups = (positionData.Length + threadGroupSize - 1) / threadGroupSize;
        compShader.Dispatch(kernelIndex, threadGroups, 1, 1);

        resultBuffer.GetData(resultData);

        positionBuffer.Release();
        resultBuffer.Release();

        Debug.Log("Returning height[0] = " + resultData[0] + " at [" + xyIndex.x + ", " + xyIndex.y + "]");

        return resultData;
    }

    //int kernelHandle = compShader.FindKernel("CSMain");

    //// Create compute buffers
    //ComputeBuffer inputBuffer = new ComputeBuffer(positionData.Length, sizeof(float) * 2); // Size of Data struct
    //ComputeBuffer resultBuffer = new ComputeBuffer(positionData.Length, sizeof(float));

    //// Set data to buffers
    //inputBuffer.SetData(positionData);
    //compShader.SetBuffer(kernelHandle, "InputData", inputBuffer);
    //compShader.SetBuffer(kernelHandle, "ResultBuffer", resultBuffer);

    //// Dispatch compute shader
    //compShader.Dispatch(kernelHandle, positionData.Length / 64, 1, 1); // Adjust based on your thread configuration and data size

    //// Retrieve data
    //float[] heights = new float[positionData.Length];
    //resultBuffer.GetData(heights);

    //// Release buffers
    //inputBuffer.Release();
    //resultBuffer.Release();

    //return heights;


    //private void TestMe(ComputeShader compShader)
    //{
    //    int kernelHandle = compShader.FindKernel("CSMain");

    //    ComputeBuffer resultBuffer = new ComputeBuffer(1, sizeof(float), ComputeBufferType.Default);

    //    noiseComputeShader.SetBuffer(kernelHandle, "ResultBuffer", resultBuffer);

    //    noiseComputeShader.Dispatch(kernelHandle, 1, 1, 1);

    //    float[] data = new float[1];
    //    resultBuffer.GetData(data);
    //    Debug.Log("Compute Shader Result: " + data[0]);

    //    resultBuffer.Release();
    //}

    //private float GetHeight(int x, int z, int xSegments, int zSegments, Vector2 noiseOffset, float noiseScale, ComputeShader noiseComputeShader)
    //{
    //    float noiseX = noiseScale * x / xSegments + noiseOffset.x;
    //    float noiseZ = noiseScale * z / zSegments + noiseOffset.y;

    //    if (usePerlinNoise)
    //    {
    //        //TestMe(noiseComputeShader);
    //        return Mathf.PerlinNoise(noiseX, noiseZ);
    //    }
    //    else
    //    {
    //        return TerrainController.noisePixels[(int)noiseX % TerrainController.noisePixels.Length][(int)noiseZ % TerrainController.noisePixels[0].Length];
    //    }
    //}

    //return DispatchComputeShader(noiseX, noiseZ, noiseComputeShader);

    //private float GetHeight(int x, int z, int xSegments, int zSegments, Vector2 noiseOffset, float noiseScale, ComputeShader noiseComputeShader)
    //{
    //    float noiseX = noiseScale * x / xSegments + noiseOffset.x;
    //    float noiseZ = noiseScale * z / zSegments + noiseOffset.y;
    //    if (usePerlinNoise)
    //        return Mathf.PerlinNoise(noiseX, noiseZ);
    //    else
    //        return TerrainController.noisePixels[(int)noiseX % TerrainController.noisePixels.Length][(int)noiseZ % TerrainController.noisePixels[0].Length];
    //}

    private float DispatchComputeShader(float noiseX, float noiseZ, ComputeShader computeShader)
    {
        int kernelHandle = computeShader.FindKernel("CSMain");

        // Create the buffer to store the result
        ComputeBuffer resultBuffer = new ComputeBuffer(1, sizeof(float), ComputeBufferType.Default);

        // Set the buffer on the compute shader
        computeShader.SetBuffer(kernelHandle, "ResultBuffer", resultBuffer);

        // Dispatch the compute shader
        computeShader.Dispatch(kernelHandle, 1, 1, 1);

        // Read data back from the buffer
        float[] data = new float[1];
        resultBuffer.GetData(data);
        Debug.Log("Compute Shader Result: " + data[0]);

        // Release the buffer
        resultBuffer.Release();
        return Mathf.PerlinNoise(noiseX, noiseZ);


        //int kernelHandle = computeShader.FindKernel("CSMain");

        //ComputeBuffer inputDataBuffer = new ComputeBuffer(1, sizeof(float) * 2);
        //Vector2 inputData = new Vector2(noiseX, noiseZ);

        //inputDataBuffer.SetData(new Vector2[] { inputData });
        //computeShader.SetBuffer(kernelHandle, "InputData", inputDataBuffer);

        //ComputeBuffer outputBuffer = new ComputeBuffer(1, sizeof(float));
        //computeShader.SetBuffer(kernelHandle, "Result", outputBuffer);

        //computeShader.Dispatch(kernelHandle, 1, 1, 1);

        //// Retrieve data
        //float[] result = new float[1];
        //outputBuffer.GetData(result);

        //// Cleanup
        //inputDataBuffer.Release();
        //outputBuffer.Release();

        //return result[0]; // We expect only one result as we dispatched only one thread
    }
}