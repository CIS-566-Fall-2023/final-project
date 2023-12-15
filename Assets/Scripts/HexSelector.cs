using Planetile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class HexSelector : MonoBehaviour
{
    public Material hexHighlight;
    public Material originalMaterial;
    public Transform CameraTrans;
    private HexagonTile selected;

    private GameObject lastHoveredHex;
    [SerializeField] GameObject preHex;
    public List<GameObject> Items1 = new List<GameObject>();
    public List<GameObject> Items2 = new List<GameObject>();
    public List<GameObject> Items3 = new List<GameObject>();
    public List<GameObject> Items4 = new List<GameObject>();
    public List<GameObject> Items5 = new List<GameObject>();
    public List<GameObject> Items6 = new List<GameObject>();
    public List<GameObject> Items7 = new List<GameObject>();
    public List<GameObject> Items8 = new List<GameObject>();
    public List<GameObject> Items9 = new List<GameObject>();
    public List<GameObject> Items10 = new List<GameObject>();
    public List<GameObject> Items11 = new List<GameObject>();
    public List<GameObject> Items12 = new List<GameObject>();

    private HexagonTile _hex;
    bool hasInit = false;
    // Start is called before the first frame update
    void Start()
    {
        //preHex.transform.parent = CameraTrans;

        //preHex.AddComponent<HexagonTile>();
        _hex = preHex.GetComponent<HexagonTile>();
        RefreshHex();
    }

    private int SimpleHash(int input)
    {
        int Modulus = 4;
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

    private void RefreshHex()
    {
        int t1 = SimpleHash(1 + Random.Range(0, 1000));
        int t2 = SimpleHash(2 + Random.Range(0, 1000));
        int t3 = SimpleHash(3 + Random.Range(0, 1000));
        int t4 = SimpleHash(4 + Random.Range(0, 1000));
        int t5 = SimpleHash(5 + Random.Range(0, 1000));
        int t6 = SimpleHash(6 + Random.Range(0, 1000));

        WFCType type1 = (WFCType)(0x01 << t1);
        WFCType type2 = (WFCType)(0x01 << t2);
        WFCType type3 = (WFCType)(0x01 << t3);
        WFCType type4 = (WFCType)(0x01 << t4);
        WFCType type5 = (WFCType)(0x01 << t5);
        WFCType type6 = (WFCType)(0x01 << t6);

        _hex.cellData.CellsOnTileEdge0[0].Type = type1;
        ActiveItem(Items1, t1);
        _hex.cellData.CellsOnTileEdge0[1].Type = type1;
        ActiveItem(Items2, t1);

        _hex.cellData.CellsOnTileEdge1[0].Type = type2;
        ActiveItem(Items3, t2);
        _hex.cellData.CellsOnTileEdge1[1].Type = type2;
        ActiveItem(Items4, t2);

        _hex.cellData.CellsOnTileEdge2[0].Type = type3;
        ActiveItem(Items5, t3);
        _hex.cellData.CellsOnTileEdge2[1].Type = type3;
        ActiveItem(Items6, t3);

        _hex.cellData.CellsOnTileEdge3[0].Type = type4;
        ActiveItem(Items7, t4);
        _hex.cellData.CellsOnTileEdge3[1].Type = type4;
        ActiveItem(Items8, t4);

        _hex.cellData.CellsOnTileEdge4[0].Type = type5;
        ActiveItem(Items9, t5);
        _hex.cellData.CellsOnTileEdge4[1].Type = type5;
        ActiveItem(Items10, t5);

        _hex.cellData.CellsOnTileEdge5[0].Type = type6;
        ActiveItem(Items11, t6);
        _hex.cellData.CellsOnTileEdge5[1].Type = type6;
        ActiveItem(Items12, t6);
    }

    public void ActiveItem(List<GameObject> Items, int i)
    {
        foreach (GameObject item in Items)
        {
            item.SetActive(false);
        }
        Items[i].SetActive(true);
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            GameObject obj = hit.collider.gameObject;
            HexagonTile hex = obj.GetComponent<HexagonTile>();

            if (hex != null && hex.corners.Count == 6 && lastHoveredHex != obj)
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

                if (hex != null && hex.corners.Count == 6 && !hex.hasSelected)
                {
                    selected = hex;
                    //selected.GetComponent<MeshRenderer>().material = hexSelect;
                    selected.hasSelected = true;
                    selected.cellData.CellsOnTileEdge0[0].Type = _hex.cellData.CellsOnTileEdge0[0].Type;
                    selected.cellData.CellsOnTileEdge0[1].Type = _hex.cellData.CellsOnTileEdge0[1].Type;
                    selected.cellData.CellsOnTileEdge1[0].Type = _hex.cellData.CellsOnTileEdge1[0].Type;
                    selected.cellData.CellsOnTileEdge1[1].Type = _hex.cellData.CellsOnTileEdge1[1].Type;
                    selected.cellData.CellsOnTileEdge2[0].Type = _hex.cellData.CellsOnTileEdge2[0].Type;
                    selected.cellData.CellsOnTileEdge2[1].Type = _hex.cellData.CellsOnTileEdge2[1].Type;
                    selected.cellData.CellsOnTileEdge3[0].Type = _hex.cellData.CellsOnTileEdge3[0].Type;
                    selected.cellData.CellsOnTileEdge3[1].Type = _hex.cellData.CellsOnTileEdge3[1].Type;
                    selected.cellData.CellsOnTileEdge4[0].Type = _hex.cellData.CellsOnTileEdge4[0].Type;
                    selected.cellData.CellsOnTileEdge4[1].Type = _hex.cellData.CellsOnTileEdge4[1].Type;
                    selected.cellData.CellsOnTileEdge5[0].Type = _hex.cellData.CellsOnTileEdge5[0].Type;
                    selected.cellData.CellsOnTileEdge5[1].Type = _hex.cellData.CellsOnTileEdge5[1].Type;

                    WFCManager.Instance.FillOneTile(selected);

                    RefreshHex();
                }
            }
        }
    }
}
