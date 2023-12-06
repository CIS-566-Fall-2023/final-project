using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;


public class ObjectManager : MonoBehaviour
{
    private Dictionary<int, List<GameObject>> m_pools;
    private int m_currPoolID;
    [HideInInspector] public bool initializationFinished = false;
    private Transform m_objsTransfrom;

    private Transform m_tilesTransfrom;
    private Dictionary<int, List<GameObject>> m_tilePools;
    private int m_currTilePoolID;

    public static ObjectManager Instance { get; private set; }

    protected void Awake()
    {
        Instance = this as ObjectManager;
        m_currPoolID = 0;
        m_pools = new Dictionary<int, List<GameObject>>();
        m_tilePools = new Dictionary<int, List<GameObject>>();
        m_objsTransfrom = GameObject.Find("ObjectManager").transform;
        m_tilesTransfrom = GameObject.Find("TilePool").transform;
    }

    public int InitializeObjectPool(int numOfObjs, GameObject prefab)
    {
        List<GameObject> pool = new List<GameObject>();
        for (int i = 0; i < numOfObjs; i++)
        {
            GameObject obj = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            obj.transform.SetParent(m_objsTransfrom);
            obj.SetActive(false);
            pool.Add(obj);
        }
        m_pools[m_currPoolID] = pool;
        m_currPoolID++;
        return m_currPoolID - 1;
    }

    public void InitializeObjectPoolOneLayer(Layer layer, int tilesPerBiome)
    {
        if (layer.hasTree)
        {
            int treePoolSize = layer.treeNumberPerGrid * tilesPerBiome * 2;
            int poolID = InitializeObjectPool(treePoolSize, layer.tree);
            layer.treePoolID = poolID;
        }

        if (layer.hasStone)
        {
            int poolSize = layer.stoneNumberPerGrid * tilesPerBiome * 2;
            int poolID = InitializeObjectPool(poolSize, layer.stone);
            layer.stonePoolID = poolID;
        }
    }

    public GameObject GetObjectFromPool(int id)
    {
        List<GameObject> pool = m_pools[id];
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                return pool[i];
            }
        }
        return null;
    }

    public void InitializeTilePoolOneLayer(Layer layer, int tilesPerBiome, float size_adjust)
    {
        List<GameObject> pool = new List<GameObject>();
        int numOfObjs = (layer.width + 1) * tilesPerBiome;

        for (int i = 0; i < numOfObjs; i++)
        {
            GameObject prefab = layer.meshes[i % layer.meshes.Count];
            GameObject obj = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            obj.GetComponent<MeshRenderer>().material = layer.material;
            obj.transform.localScale *= size_adjust;
            obj.transform.Rotate(Vector3.up, 30);
            obj.transform.SetParent(m_tilesTransfrom, true);
            obj.SetActive(false);
            pool.Add(obj);
        }
        m_tilePools[m_currTilePoolID] = pool;
        layer.tilePoolID = m_currTilePoolID;
        m_currTilePoolID++;
    }

    public GameObject GetTileFromPool(int id)
    {
        List<GameObject> pool = m_tilePools[id];
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                return pool[i];
            }
        }
        return null;
    }

    public void BackToObjectPool(Transform t)
    {
        t.SetParent(m_objsTransfrom);
        t.gameObject.SetActive(false);
    }

    public void BackToTilePool(GameObject tile)
    {
        tile.transform.SetParent(m_tilesTransfrom);
        tile.SetActive(false);
    }
}
