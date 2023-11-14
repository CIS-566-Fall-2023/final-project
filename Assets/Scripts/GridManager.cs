using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Planetile
{
    public class GridManager : MonoBehaviour
    {
        private static GridManager instance;
        public static GridManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType(typeof(GridManager)) as GridManager;
                    if (instance == null)
                    {
                        GameObject go = GameObject.Find("GameManager");
                        if (go == null)
                        {
                            go = new GameObject("GameManager");
                            go.tag = "GameController";
                            DontDestroyOnLoad(go);
                        }
                        instance = go.AddComponent<GridManager>();
                    }
                }
                return instance;
            }
        }
    }
}
