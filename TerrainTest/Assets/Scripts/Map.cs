using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public int layers = 3;
    public int length = 6;
    public float hexSize = 0.8f;
    public int tilesPerBiome = 10;
    public List<BiomeConfig> biomes;

    private List<List<GameObject>> m_hexTiles;
    private int m_currBiome;
    private Vector2 m_headPos;
    private int m_tileCount = 0;

    private const float half_sqr3 = 0.866f;
    private const float size_adjust = 1.13f;

    private void Start()
    {
        m_currBiome = 0;
        GenerateGrid();
    }

    private void Update()
    {
        UpdateGrid();

        if(m_tileCount >= tilesPerBiome)
        {
            m_tileCount = 0;
            m_currBiome++;
            if (m_currBiome >= biomes.Count)
            {
                m_currBiome = 0;
            }
        }
    }

    public void GenerateGrid()
    {
        m_hexTiles = new List<List<GameObject>>();
        m_headPos.x = -0.5f * (length - 1) * hexSize * half_sqr3;
        m_headPos.y = 0.5f * (layers - 1) * hexSize * half_sqr3;


        for (int z = 0; z < layers; z++)
        {
            m_hexTiles.Add(new List<GameObject>());
            for (int x = 0; x < length; x++)
            {
                SpawnHexTile(z, x);
            }
        }

        m_tileCount = length;
    }

    public void SpawnHexTile(int z, int x)
    {
        float pos_z = m_headPos.y - z * hexSize * half_sqr3;
        float pos_x; 
        if (z % 2 == 0) pos_x = m_headPos.x + x * hexSize * half_sqr3;
        else pos_x = m_headPos.x - 0.5f * hexSize * half_sqr3 + x * hexSize * half_sqr3;

        Layer layer = GetBiomeLayer(z);
        GameObject tile = layer.meshes[UnityEngine.Random.Range(0, layer.meshes.Count)];
        var spawnedTile = Instantiate(tile, new Vector3(pos_x, 0, pos_z), Quaternion.identity);
        spawnedTile.GetComponent<MeshRenderer>().material = layer.material;

        spawnedTile.transform.localScale *= size_adjust;
        spawnedTile.transform.Rotate(Vector3.up, 30 + UnityEngine.Random.Range(0, 6) * 60);
        spawnedTile.transform.SetParent(this.transform, true);

        m_hexTiles[z].Add(spawnedTile);

        SpawnObjectsForLayer(layer, spawnedTile.transform);
    }

    public Layer GetBiomeLayer(int z)
    {
        // Far & far mid boundary
        if (z < biomes[m_currBiome].far.width - 1)
        {
            return biomes[m_currBiome].far;
        }
        if (z < biomes[m_currBiome].far.width)
        {
            if(biomes[m_currBiome].blendMidFar 
                && UnityEngine.Random.Range(0.0f, 1.0f) > biomes[m_currBiome].farMidRatio)
            {
                return biomes[m_currBiome].mid;
            }
            return biomes[m_currBiome].far;
        }

        // Mid & mid near boundary
        if (z < biomes[m_currBiome].far.width + biomes[m_currBiome].mid.width - 1)
        {
            return biomes[m_currBiome].mid;
        }
        if (z < biomes[m_currBiome].far.width + biomes[m_currBiome].mid.width)
        {
            if (biomes[m_currBiome].blendNearMid
                && UnityEngine.Random.Range(0.0f, 1.0f) > biomes[m_currBiome].midNearRatio)
            {
                return biomes[m_currBiome].near;
            }
            return biomes[m_currBiome].mid;
        }
 
        // Near
        return biomes[m_currBiome].near;         
    }

    public void UpdateGrid()
    {
        if(m_hexTiles[0][0].transform.position.x <= Train.Instance.GetLeftBound())
        {
            m_headPos.x = m_hexTiles[0][1].transform.position.x;
            GameObject tile;
            for (int z = 0; z < layers; z++)
            {
                tile = m_hexTiles[z][0];
                m_hexTiles[z].RemoveAt(0);
                Destroy(tile);
                SpawnHexTile(z, length - 1);
            }
            m_tileCount++;
        }
    }

    void SpawnObjectsForLayer(Layer layer, Transform parent)
    {
        if(layer.hasTree)
        {
            SpawnObjectsOnGrid(layer.treeNumberPerGrid, layer.tree, parent);
        }
        if (layer.hasStone)
        {
            SpawnObjectsOnGrid(layer.stoneNumberPerGrid, layer.stone, parent);
        }
    }

    void SpawnObjectsOnGrid(int numberPerGrid, GameObject objectToSpawn, Transform parent)
    {
        for (int i = 0; i < numberPerGrid; i++)
        {
            float randomAngle = UnityEngine.Random.Range(0f, 2f * Mathf.PI);
            float spawnRadius = hexSize * half_sqr3 * 0.5f * UnityEngine.Random.Range(0f, 1f);
            float randomX = parent.position.x + spawnRadius * Mathf.Cos(randomAngle);
            float randomZ = parent.position.z + spawnRadius * Mathf.Sin(randomAngle);

            float terrainHeight = GetMeshTerrainHeight(new Vector3(randomX, 0f, randomZ));
            if (terrainHeight > 0.2) terrainHeight = 0;
            Vector3 spawnPosition = new Vector3(randomX, terrainHeight, randomZ);

            GameObject newObj = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
            newObj.transform.Rotate(Vector3.up, UnityEngine.Random.Range(0, 360));
            newObj.transform.SetParent(parent, true);
        }
    }

    float GetMeshTerrainHeight(Vector3 position)
    {
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(position.x, 1000f, position.z), Vector3.down, out hit, Mathf.Infinity))
        {
            return hit.point.y;
        }
        else
        {
            return 0f;
        }
    }

}
