using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Planetile
{
    [Serializable]
    struct ItemAndType
    {
        CellType type;
        List<Item> items;
    }
    public class WFCManager : MonoBehaviour
    {
        private static WFCManager instance;
        public static WFCManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType(typeof(WFCManager)) as WFCManager;
                    if (instance == null)
                    {
                        GameObject go = GameObject.Find("GameManager");
                        if (go == null)
                        {
                            go = new GameObject("GameManager");
                            go.tag = "GameController";
                            DontDestroyOnLoad(go);
                        }
                        instance = go.AddComponent<WFCManager>();
                    }
                }
                return instance;
            }
        }

        [SerializeField]
        List<ItemAndType> itemPool;

    }
}
