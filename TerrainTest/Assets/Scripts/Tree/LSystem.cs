using UnityEngine;
using System.Collections.Generic;

public class LSystem : MonoBehaviour
{
    [SerializeField]
    private TextAsset file;
    [SerializeField]
    private int segmentAxialSamples = 3;
    [SerializeField]
    private int segmentRadialSamples = 3;
    [SerializeField]
    private float segmentWidth;
    [SerializeField]
    private float segmentHeight;
    [SerializeField]
    private float leafSize;
    [SerializeField]
    private int leafAxialDensity = 1;
    [SerializeField]
    private int leafRadialDensity = 1;
    [SerializeField]
    private bool useFoliage;
    [SerializeField]
    private bool narrowBranches = true;
    [SerializeField]
    private Material trunkMaterial;
    [SerializeField]
    private Material leafMaterial;

    void Start()
    {
        string axiom;
        float angle;
        int derivations;
        Dictionary<string, List<Production>> productions;
        Parse(
            file.text,
            out axiom,
            out angle,
            out derivations,
            out productions);

        string moduleString;
        Derive(
            axiom,
            angle,
            derivations,
            productions,
            out moduleString);

        GameObject leaves, trunk;
        LSystemInterpreter.Interpret(
            segmentAxialSamples,
            segmentRadialSamples,
            segmentWidth,
            segmentHeight,
            leafSize,
            leafAxialDensity,
            leafRadialDensity,
            useFoliage,
            narrowBranches,
            leafMaterial,
            trunkMaterial,
            angle,
            moduleString,
            out leaves,
            out trunk);

        leaves.transform.parent = transform;
        leaves.transform.localPosition = Vector3.zero;
        trunk.transform.parent = transform;
        trunk.transform.localPosition = Vector3.zero;

        UpdateColliderBounds(trunk);
    }

    void UpdateColliderBounds(GameObject trunk)
    {
        // Calculate AABB
        Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        for (int i = 0; i < trunk.transform.childCount; i++)
        {
            Transform chunk = trunk.transform.GetChild(i);
            min.x = Mathf.Min(min.x, chunk.GetComponent<Renderer>().bounds.min.x);
            min.y = Mathf.Min(min.y, chunk.GetComponent<Renderer>().bounds.min.y);
            min.z = Mathf.Min(min.z, chunk.GetComponent<Renderer>().bounds.min.z);
            max.x = Mathf.Max(max.x, chunk.GetComponent<Renderer>().bounds.max.x);
            max.y = Mathf.Max(max.y, chunk.GetComponent<Renderer>().bounds.max.y);
            max.z = Mathf.Max(max.z, chunk.GetComponent<Renderer>().bounds.max.z);
        }

        Bounds bounds = new Bounds();
        bounds.SetMinMax(min, max);

        BoxCollider collider = gameObject.GetComponent<BoxCollider>();
        if (collider == null)
            return;
        collider.center = bounds.center - transform.position;
        collider.size = 2 * bounds.extents;
    }

    public void Parse(string content, out string axiom, out float angle, out int derivations, out Dictionary<string, List<Production>> productions)
    {
        axiom = "";
        angle = 0;
        derivations = 0;
        productions = new Dictionary<string, List<Production>>();
        var lines = content.Split('\n');
        foreach (string rawLine in lines)
        {
            string line = rawLine.Trim();
            if (line.Length == 0)
                continue;
            else if (line.Length == 1 && line[0] == '\r')
                continue;
            else if (line[0] == '/' && line[1] == '/')
                continue;
            string value;
            if (line.IndexOf("axiom") != -1)
            {
                value = line.Substring(line.IndexOf("=") + 1);
                value = value.Trim();
                axiom = value;
            }
            else if (line.IndexOf("angle") != -1)
            {
                value = line.Substring(line.IndexOf("=") + 1);
                value = value.Trim();
                angle = float.Parse(value);
            }
            else if (line.IndexOf("number of derivations") != -1)
            {
                value = line.Substring(line.IndexOf("=") + 1);
                value = value.Trim();
                derivations = int.Parse(value);
            }
            else
            {
                string[] tokens = line.Split('=');
                if (tokens.Length != 2)
                    continue;
                string predecessor = tokens[0].Trim();
                tokens = tokens[1].Trim().Split(')');
                string probabilityString = tokens[0].Substring(1);
                string successor = tokens[1];
                float probability = float.Parse(probabilityString);
                if (!productions.ContainsKey(predecessor))
                    productions[predecessor] = new List<Production>();
                productions[predecessor].Add(new Production(predecessor, successor, probability));
            }
        }
    }

    public void Derive(string axiom, float angle, int derivations, Dictionary<string, List<Production>> productions, out string moduleString)
    {
        moduleString = axiom;
        for (int i = 0; i < Mathf.Max(1, derivations); i++)
        {
            string newModuleString = "";
            for (int j = 0; j < moduleString.Length; j++)
            {
                string module = moduleString[j] + "";
                if (!productions.ContainsKey(module))
                {
                    newModuleString += module;
                    continue;
                }
                var production = Match(module, productions);
                newModuleString += production.successor;
            }
            moduleString = newModuleString;
        }
    }

    public Production Match(string module, Dictionary<string, List<Production>> productions)
    {
        if (!productions.ContainsKey(module))
            return null;
        List<Production> matches = productions[module];
        if (matches.Count == 1)
            return matches[0];
        float chance = UnityEngine.Random.value, accProbability = 0;
        foreach (var match in matches)
        {
            accProbability += match.probability;
            if (accProbability <= chance)
                return match;
        }
        return null;
    }

    public bool CheckProbabilities(Dictionary<string, List<Production>> productions)
    {
        foreach (var matches in productions.Values)
        {
            if (matches.Count == 1 && matches[0].probability != 1)
                return false;
            float accProbabilities = 0;
            foreach (var match in matches)
                accProbabilities += match.probability;
            if (accProbabilities != 1)
                return false;
        }
        return true;
    }
}
