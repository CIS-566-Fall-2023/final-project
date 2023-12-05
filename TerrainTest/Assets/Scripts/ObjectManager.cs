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

    public static ObjectManager Instance { get; private set; }
    protected void Awake()
    {
        Instance = this as ObjectManager;
        m_currPoolID = 0;
        m_pools = new Dictionary<int, List<GameObject>>();
        m_objsTransfrom = GameObject.Find("ObjectManager").transform;
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

}
