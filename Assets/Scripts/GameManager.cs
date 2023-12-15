using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Planetile
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;
        public GameObject UIObj;
        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType(typeof(GameManager)) as GameManager;
                    if (instance == null)
                    {
                        GameObject go = GameObject.Find("GameManager");
                        if (go == null)
                        {
                            go = new GameObject("GameManager");
                            go.tag = "GameController";
                            DontDestroyOnLoad(go);
                        }
                        instance = go.AddComponent<GameManager>();
                    }
                }
                return instance;
            }
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                ShowUI();
                Time.timeScale = 0;
            }
        }

        private void ShowUI()
        {
            if(UIObj != null)
            {
                UIObj.SetActive(true);
            }
        }
    }
}
