using System.Runtime.CompilerServices;
using UnityEngine;

public class SDFCollection : MonoBehaviour
{
    #region Shader Properties
    private const int MAX_SDF_OBJECTS = 256;
    private static readonly int SDFTypeShaderPropertyID = Shader.PropertyToID("SDFType");
    private static readonly int SDFPositionsShaderPropertyID= Shader.PropertyToID("SDFPositions");
    private static readonly int SDFSizeShaderPropertyID = Shader.PropertyToID("SDFSizes");
    private static readonly int SDFBlendOperationShaderPropertyID = Shader.PropertyToID("SDFBlendOperation");
    private static readonly int SDFBlendFactorShaderPropertyID = Shader.PropertyToID("SDFBlendFactor");
    private static readonly int SDFCountShaderPropertyID = Shader.PropertyToID("SDFCount");
    #endregion

    private SDFObject[] sdfObjects = new SDFObject[MAX_SDF_OBJECTS];
    int numSDFObjects;
    [SerializeField] private SDFObject sdfObjectPrefab;

    private Renderer renderer;
    private MaterialPropertyBlock materialPropertyBlock;

    private Vector4[] sdfPositions = new Vector4[MAX_SDF_OBJECTS];
    private float[] sdfTypes = new float[MAX_SDF_OBJECTS];
    private float[] sdfSizes = new float[MAX_SDF_OBJECTS];
    private float[] sdfBlendOperations = new float[MAX_SDF_OBJECTS];
    private float[] sdfBlends = new float[MAX_SDF_OBJECTS];

    private void OnEnable()
    {
        renderer = GetComponent<Renderer>();
        materialPropertyBlock = new MaterialPropertyBlock();
    }

    private void Update()
    {
        for (int i = 0; i < numSDFObjects; i++)
        {
            SDFObject sdf = sdfObjects[i];
            sdfTypes[i] = (int)sdf.Type;
            sdfPositions[i] = sdf.transform.position;
            sdfSizes[i] = sdf.Size;
            sdfBlendOperations[i] = (int)sdf.BlendOperation;
            sdfBlends[i] = sdf.BlendFactor;
        }

        materialPropertyBlock.SetFloatArray(SDFTypeShaderPropertyID, sdfTypes);
        materialPropertyBlock.SetVectorArray(SDFPositionsShaderPropertyID, sdfPositions);
        materialPropertyBlock.SetFloatArray(SDFSizeShaderPropertyID, sdfSizes);
        materialPropertyBlock.SetFloatArray(SDFBlendOperationShaderPropertyID, sdfBlendOperations);
        materialPropertyBlock.SetFloatArray(SDFBlendFactorShaderPropertyID, sdfBlends);
        materialPropertyBlock.SetInt(SDFCountShaderPropertyID, numSDFObjects);
        renderer.SetPropertyBlock(materialPropertyBlock);
    }

    private void OnDisable()
    {
        materialPropertyBlock.Clear();
        materialPropertyBlock = null;
        renderer.SetPropertyBlock(null);
    }

    #region HELPERS
    [ContextMenu("Add SDF Object")]
    private void AddSDFObject()
    {
        if (numSDFObjects == MAX_SDF_OBJECTS)
        {
            // Can't add any more to this SDFCollection!
            return;
        }

        SDFObject sdfObject = Instantiate(sdfObjectPrefab, transform);
        sdfObjects[numSDFObjects] = sdfObject;
        numSDFObjects++;
    }
    #endregion
}
