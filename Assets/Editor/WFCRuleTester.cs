using UnityEngine;
using UnityEditor;

using Planetile;

public class TemporaryCellClass : IWFCCell
{
    public bool IsPentagon => false;
    WFCType type = WFCType.Null;
    public TemporaryCellClass(WFCType type) { this.type = type; }
    public WFCType Type => type;

    public IWFCTile Tile => throw new System.NotImplementedException();

    public IWFCItem Item => throw new System.NotImplementedException();

    public bool IsPlaced => throw new System.NotImplementedException();

    public int EdgeNum => throw new System.NotImplementedException();

    public IWFCCell[] GetAdjacentCells()
    {
        throw new System.NotImplementedException();
    }

    public IWFCCell[] GetAdjacentCellsInTile(IWFCTile tile)
    {
        throw new System.NotImplementedException();
    }

    public Vector3 GetWorldPos()
    {
        throw new System.NotImplementedException();
    }

    public void PlaceItem(IWFCItem item)
    {
        throw new System.NotImplementedException();
    }
}


public class WFCRuleTester : EditorWindow
{
    string inputText = "Input rule.";
    WFCRule rule = new WFCRule();
    WFCType type0 = WFCType.Null;
    WFCType type1 = WFCType.Null;
    WFCType type2 = WFCType.Null;
    string testResult = "";

    [MenuItem("Window/WFC Rule Tester")]
    public static void ShowWindow()
    {
        GetWindow<WFCRuleTester>("WFC Rule Tester");
    }

    void OnGUI()
    {
        using (new EditorGUILayout.HorizontalScope())
        {
            inputText = EditorGUILayout.TextArea(inputText);

            if (GUILayout.Button("Parse Rule"))
            {
                rule.rule = inputText;
                rule.InitializeRPN();
                testResult = "";
            }
        }
        if (rule.Initialized)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                type0 = (WFCType)EditorGUILayout.EnumFlagsField(type0);
                type1 = (WFCType)EditorGUILayout.EnumFlagsField(type1);
                type2 = (WFCType)EditorGUILayout.EnumFlagsField(type2);
                if (GUILayout.Button("Test"))
                {
                    var neighbors = new TemporaryCellClass[3] {
                    new TemporaryCellClass(type0),
                    new TemporaryCellClass(type1),
                    new TemporaryCellClass(type2),
                };
                    if (rule.SatisfyRule(neighbors))
                    {
                        testResult = "True";
                    }
                    else
                    {
                        testResult = "False";
                    }
                }
            }
            GUILayout.Label(testResult);
        }
    }
}