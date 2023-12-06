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
    [SerializeField] GameObject preHex;
    private HexagonTile hex;
    // Start is called before the first frame update
    void Start()
    {
        //preHex.transform.parent = CameraTrans;
        
        //preHex.AddComponent<HexagonTile>();
        hex = preHex.GetComponent<HexagonTile>();

        WFCType type1 = (WFCType)(0x01 << SimpleHash(1 + Random.Range(0, 1000)));
        WFCType type2 = (WFCType)(0x01 << SimpleHash(2 + Random.Range(0, 1000)));
        WFCType type3 = (WFCType)(0x01 << SimpleHash(3 + Random.Range(0, 1000)));
        WFCType type4 = (WFCType)(0x01 << SimpleHash(4 + Random.Range(0, 1000)));
        WFCType type5 = (WFCType)(0x01 << SimpleHash(5 + Random.Range(0, 1000)));
        WFCType type6 = (WFCType)(0x01 << SimpleHash(6 + Random.Range(0, 1000)));

        hex.cellData.CellsOnTileEdge0[0].Type = type1;
        hex.cellData.CellsOnTileEdge0[1].Type = type1;
        hex.cellData.CellsOnTileEdge1[0].Type = type2;
        hex.cellData.CellsOnTileEdge1[1].Type = type2;
        hex.cellData.CellsOnTileEdge2[0].Type = type3;
        hex.cellData.CellsOnTileEdge2[1].Type = type3;
        hex.cellData.CellsOnTileEdge3[0].Type = type4;
        hex.cellData.CellsOnTileEdge3[1].Type = type4;
        hex.cellData.CellsOnTileEdge4[0].Type = type5;
        hex.cellData.CellsOnTileEdge4[1].Type = type5;
        hex.cellData.CellsOnTileEdge5[0].Type = type6;
        hex.cellData.CellsOnTileEdge5[1].Type = type6;
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
                    selected.cellData.CellsOnTileEdge0[0].Type = hex.cellData.CellsOnTileEdge0[0].Type;
                    selected.cellData.CellsOnTileEdge0[1].Type = hex.cellData.CellsOnTileEdge0[1].Type;
                    selected.cellData.CellsOnTileEdge1[0].Type = hex.cellData.CellsOnTileEdge1[0].Type;
                    selected.cellData.CellsOnTileEdge1[1].Type = hex.cellData.CellsOnTileEdge1[1].Type;
                    selected.cellData.CellsOnTileEdge2[0].Type = hex.cellData.CellsOnTileEdge2[0].Type;
                    selected.cellData.CellsOnTileEdge2[1].Type = hex.cellData.CellsOnTileEdge2[1].Type;
                    selected.cellData.CellsOnTileEdge3[0].Type = hex.cellData.CellsOnTileEdge3[0].Type;
                    selected.cellData.CellsOnTileEdge3[1].Type = hex.cellData.CellsOnTileEdge3[1].Type;
                    selected.cellData.CellsOnTileEdge4[0].Type = hex.cellData.CellsOnTileEdge4[0].Type;
                    selected.cellData.CellsOnTileEdge4[1].Type = hex.cellData.CellsOnTileEdge4[1].Type;
                    selected.cellData.CellsOnTileEdge5[0].Type = hex.cellData.CellsOnTileEdge5[0].Type;
                    selected.cellData.CellsOnTileEdge5[1].Type = hex.cellData.CellsOnTileEdge5[1].Type;

                    WFCManager.Instance.FillOneTile(selected);

                    WFCType type1 = (WFCType)(0x01 << SimpleHash(1 + Random.Range(0, 1000)));
                    WFCType type2 = (WFCType)(0x01 << SimpleHash(2 + Random.Range(0, 1000)));
                    WFCType type3 = (WFCType)(0x01 << SimpleHash(3 + Random.Range(0, 1000)));
                    WFCType type4 = (WFCType)(0x01 << SimpleHash(4 + Random.Range(0, 1000)));
                    WFCType type5 = (WFCType)(0x01 << SimpleHash(5 + Random.Range(0, 1000)));
                    WFCType type6 = (WFCType)(0x01 << SimpleHash(6 + Random.Range(0, 1000)));

                    hex.cellData.CellsOnTileEdge0[0].Type = type1;
                    hex.cellData.CellsOnTileEdge0[1].Type = type1;
                    hex.cellData.CellsOnTileEdge1[0].Type = type2;
                    hex.cellData.CellsOnTileEdge1[1].Type = type2;
                    hex.cellData.CellsOnTileEdge2[0].Type = type3;
                    hex.cellData.CellsOnTileEdge2[1].Type = type3;
                    hex.cellData.CellsOnTileEdge3[0].Type = type4;
                    hex.cellData.CellsOnTileEdge3[1].Type = type4;
                    hex.cellData.CellsOnTileEdge4[0].Type = type5;
                    hex.cellData.CellsOnTileEdge4[1].Type = type5;
                    hex.cellData.CellsOnTileEdge5[0].Type = type6;
                    hex.cellData.CellsOnTileEdge5[1].Type = type6;
                }
            }
        }
    }
}
