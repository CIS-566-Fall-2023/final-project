using Planetile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexSelector : MonoBehaviour
{
    public Material hexHighlight;
    public Material hexSelect;
    public Material originalMaterial;
    public Transform CameraTrans;
    private HexagonTile selected;

    private GameObject lastHoveredHex;
    [SerializeField] private List<GameObject> prefabHexList;
    [SerializeField] private List<Transform> preTransList;
    [SerializeField] private List<GameObject> preHexList;
    private List<HexagonTile> hexList;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < prefabHexList.Count; i++)
        {
            preHexList.Add(Instantiate(prefabHexList[i]));
        }

        for(int i = 0; i < preTransList.Count; i++)
        {
            preHexList[i].transform.position = preTransList[i].position;
            preHexList[i].transform.parent = CameraTrans;
        }

        foreach(GameObject obj in preHexList)
        {
            obj.AddComponent<HexagonTile>();
            hexList.Add(obj.GetComponent<HexagonTile>());
        }

        for (int i = 0; i < hexList.Count; i++)
        {
            WFCType type1 = (WFCType)SimpleHash(1 + Random.Range(0, 1000) * i);
            WFCType type2 = (WFCType)SimpleHash(2 + Random.Range(0, 1000) * i);
            WFCType type3 = (WFCType)SimpleHash(3 + Random.Range(0, 1000) * i);
            WFCType type4 = (WFCType)SimpleHash(4 + Random.Range(0, 1000) * i);
            WFCType type5 = (WFCType)SimpleHash(5 + Random.Range(0, 1000) * i);
            WFCType type6 = (WFCType)SimpleHash(6 + Random.Range(0, 1000) * i);
            WFCType type7 = (WFCType)SimpleHash(7 + Random.Range(0, 1000) * i);
            WFCType type8 = (WFCType)SimpleHash(8 + Random.Range(0, 1000) * i);
            WFCType type9 = (WFCType)SimpleHash(9 + Random.Range(0, 1000) * i);
            WFCType type10 = (WFCType)SimpleHash(0 + Random.Range(0, 1000) * i);
            WFCType type11 = (WFCType)SimpleHash(11 + Random.Range(0, 1000) * i);
            WFCType type12 = (WFCType)SimpleHash(12 + Random.Range(0, 1000) * i);

            hexList[i].cellData.CellsOnTileEdge0[0].Type = type1;
            hexList[i].cellData.CellsOnTileEdge0[1].Type = type2;
            hexList[i].cellData.CellsOnTileEdge1[0].Type = type3;
            hexList[i].cellData.CellsOnTileEdge1[1].Type = type4;
            hexList[i].cellData.CellsOnTileEdge2[0].Type = type5;
            hexList[i].cellData.CellsOnTileEdge2[1].Type = type6;
            hexList[i].cellData.CellsOnTileEdge3[0].Type = type7;
            hexList[i].cellData.CellsOnTileEdge3[1].Type = type8;
            hexList[i].cellData.CellsOnTileEdge4[0].Type = type9;
            hexList[i].cellData.CellsOnTileEdge4[1].Type = type10;
            hexList[i].cellData.CellsOnTileEdge5[0].Type = type11;
            hexList[i].cellData.CellsOnTileEdge5[1].Type = type12;
        }
    }

    private int SimpleHash(int input)
    {
        int Modulus = 8;
        int Multiplier = 48271;
        int Increment = 214748367;

        input = (Multiplier * input + Increment) % Modulus;
        return input;
    }

    public HexagonTile GetSelected()
    {
        return selected;
    }

    // Update is called once per frame

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            GameObject obj = hit.collider.gameObject;
            HexagonTile hex = obj.GetComponent<HexagonTile>();

            if (hex != null && lastHoveredHex != obj)
            {
                if (lastHoveredHex != null && !lastHoveredHex.GetComponent<HexagonTile>().hasSelected)
                {
                    lastHoveredHex.GetComponent<MeshRenderer>().material = originalMaterial;
                }

                lastHoveredHex = obj;
                if (!hex.hasSelected)
                {
                    hex.GetComponent<MeshRenderer>().material = hexHighlight;
                }
            }
        }
        else
        {
            if (lastHoveredHex != null && !lastHoveredHex.GetComponent<HexagonTile>().hasSelected)
            {
                lastHoveredHex.GetComponent<MeshRenderer>().material = originalMaterial;
                lastHoveredHex = null;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit2;
            if (Physics.Raycast(ray2, out hit2, Mathf.Infinity))
            {
                GameObject obj = hit.collider.gameObject;

                HexagonTile hex = obj.GetComponent<HexagonTile>();

                if (hex != null && !hex.hasSelected)
                {
                    selected = hex;
                    selected.GetComponent<MeshRenderer>().material = hexSelect;
                    selected.hasSelected = true;
                    selected.cellData.CellsOnTileEdge0[0].Type = hexList[0].cellData.CellsOnTileEdge0[0].Type;
                    selected.cellData.CellsOnTileEdge0[1].Type = hexList[0].cellData.CellsOnTileEdge0[1].Type;
                    selected.cellData.CellsOnTileEdge1[0].Type = hexList[0].cellData.CellsOnTileEdge1[0].Type;
                    selected.cellData.CellsOnTileEdge1[1].Type = hexList[0].cellData.CellsOnTileEdge1[1].Type;
                    selected.cellData.CellsOnTileEdge2[0].Type = hexList[0].cellData.CellsOnTileEdge2[0].Type;
                    selected.cellData.CellsOnTileEdge2[1].Type = hexList[0].cellData.CellsOnTileEdge2[1].Type;
                    selected.cellData.CellsOnTileEdge3[0].Type = hexList[0].cellData.CellsOnTileEdge3[0].Type;
                    selected.cellData.CellsOnTileEdge3[1].Type = hexList[0].cellData.CellsOnTileEdge3[1].Type;
                    selected.cellData.CellsOnTileEdge4[0].Type = hexList[0].cellData.CellsOnTileEdge4[0].Type;
                    selected.cellData.CellsOnTileEdge4[1].Type = hexList[0].cellData.CellsOnTileEdge4[1].Type;
                    selected.cellData.CellsOnTileEdge5[0].Type = hexList[0].cellData.CellsOnTileEdge5[0].Type;
                    selected.cellData.CellsOnTileEdge5[1].Type = hexList[0].cellData.CellsOnTileEdge5[1].Type;

                    WFCManager.Instance.FillOneTile(selected);

                    hexList.RemoveAt(0);
                    preHexList.RemoveAt(0);
                    GameObject newHex = Instantiate(prefabHexList[Random.Range(0, 3)]);
                    preHexList.Add(newHex);
                    HexagonTile hexTile = newHex.AddComponent<HexagonTile>();
                    hexList.Add(hexTile);

                    WFCType type1 = (WFCType)SimpleHash(1 + Random.Range(0, 1000));
                    WFCType type2 = (WFCType)SimpleHash(2 + Random.Range(0, 1000));
                    WFCType type3 = (WFCType)SimpleHash(3 + Random.Range(0, 1000));
                    WFCType type4 = (WFCType)SimpleHash(4 + Random.Range(0, 1000));
                    WFCType type5 = (WFCType)SimpleHash(5 + Random.Range(0, 1000));
                    WFCType type6 = (WFCType)SimpleHash(6 + Random.Range(0, 1000));
                    WFCType type7 = (WFCType)SimpleHash(7 + Random.Range(0, 1000));
                    WFCType type8 = (WFCType)SimpleHash(8 + Random.Range(0, 1000));
                    WFCType type9 = (WFCType)SimpleHash(9 + Random.Range(0, 1000));
                    WFCType type10 = (WFCType)SimpleHash(0 + Random.Range(0, 1000));
                    WFCType type11 = (WFCType)SimpleHash(11 + Random.Range(0, 1000));
                    WFCType type12 = (WFCType)SimpleHash(12 + Random.Range(0, 1000));

                    hexTile.cellData.CellsOnTileEdge0[0].Type = type1;
                    hexTile.cellData.CellsOnTileEdge0[1].Type = type2;
                    hexTile.cellData.CellsOnTileEdge1[0].Type = type3;
                    hexTile.cellData.CellsOnTileEdge1[1].Type = type4;
                    hexTile.cellData.CellsOnTileEdge2[0].Type = type5;
                    hexTile.cellData.CellsOnTileEdge2[1].Type = type6;
                    hexTile.cellData.CellsOnTileEdge3[0].Type = type7;
                    hexTile.cellData.CellsOnTileEdge3[1].Type = type8;
                    hexTile.cellData.CellsOnTileEdge4[0].Type = type9;
                    hexTile.cellData.CellsOnTileEdge4[1].Type = type10;
                    hexTile.cellData.CellsOnTileEdge5[0].Type = type11;
                    hexTile.cellData.CellsOnTileEdge5[1].Type = type12;

                    for (int i = 0; i < preTransList.Count; i++)
                    {
                        preHexList[i].transform.position = preTransList[i].position;
                        preHexList[i].transform.parent = CameraTrans;
                    }
                }
            }
        }
    }
}
