using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(MeshRenderer))]
public class SDFCollection : MonoBehaviour
{
    #region Shader Properties
    private const int MAX_SDF_OBJECTS = 256;
    private static readonly int SDFTypeShaderPropertyID = Shader.PropertyToID("SDFType");
    private static readonly int SDFTransformMatricesPropertyID = Shader.PropertyToID("SDFTransformMatrices");
    private static readonly int SDFSizeShaderPropertyID = Shader.PropertyToID("SDFSizes");
    private static readonly int SDFBlendOperationShaderPropertyID = Shader.PropertyToID("SDFBlendOperation");
    private static readonly int SDFBlendFactorShaderPropertyID = Shader.PropertyToID("SDFBlendFactor");
    private static readonly int SDFCountShaderPropertyID = Shader.PropertyToID("SDFCount");
    #endregion

    private SDFObject[] sdfObjects;
    int numSDFObjects;
    [SerializeField] private SDFObject sdfObjectPrefab;

    private Renderer renderer;
    private MaterialPropertyBlock materialPropertyBlock;

    private Matrix4x4[] sdfTransformMatrices = new Matrix4x4[MAX_SDF_OBJECTS];
    private float[] sdfTypes = new float[MAX_SDF_OBJECTS];
    private float[] sdfSizes = new float[MAX_SDF_OBJECTS];
    private float[] sdfBlendOperations = new float[MAX_SDF_OBJECTS];
    private float[] sdfBlends = new float[MAX_SDF_OBJECTS];

    private bool hasInitialized = false;

    private void OnEnable()
    {
        Initialize();
    }

    private void OnTransformChildrenChanged()
    {
        ValidateTransformChildrenChange();
    }

    private void OnValidate()
    {
        hasInitialized = false;
        Initialize();
    }

    private void Initialize()
    {
        if (hasInitialized)
        {
            return;
        }

        sdfObjects = new SDFObject[MAX_SDF_OBJECTS];
        sdfTransformMatrices = new Matrix4x4[MAX_SDF_OBJECTS];
        sdfTypes = new float[MAX_SDF_OBJECTS];
        sdfSizes = new float[MAX_SDF_OBJECTS];
        sdfBlendOperations = new float[MAX_SDF_OBJECTS];
        sdfBlends = new float[MAX_SDF_OBJECTS];
        renderer = GetComponent<Renderer>();

        materialPropertyBlock = new MaterialPropertyBlock();

        ValidateTransformChildrenChange();

        hasInitialized = true;
    }

    private void Update()
    {
        if (!hasInitialized)
        {
            return;
        }

        for (int i = 0; i < numSDFObjects; i++)
        {
            SDFObject sdf = sdfObjects[i];
            sdfTypes[i] = (int)sdf.Type;
            sdfTransformMatrices[i] = sdf.transform.worldToLocalMatrix;
            sdfSizes[i] = sdf.Size;
            sdfBlendOperations[i] = (int)sdf.BlendOperation;
            sdfBlends[i] = sdf.BlendFactor;
        }

        materialPropertyBlock.SetFloatArray(SDFTypeShaderPropertyID, sdfTypes);
        materialPropertyBlock.SetMatrixArray(SDFTransformMatricesPropertyID, sdfTransformMatrices);
        materialPropertyBlock.SetFloatArray(SDFSizeShaderPropertyID, sdfSizes);
        materialPropertyBlock.SetFloatArray(SDFBlendOperationShaderPropertyID, sdfBlendOperations);
        materialPropertyBlock.SetFloatArray(SDFBlendFactorShaderPropertyID, sdfBlends);
        materialPropertyBlock.SetInt(SDFCountShaderPropertyID, numSDFObjects);
        renderer.SetPropertyBlock(materialPropertyBlock);
    }

    private void OnDisable()
    {
        if (!hasInitialized)
        {
            return;
        }

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

    private void ValidateTransformChildrenChange()
    {
        SDFObject[] objects = GetComponentsInChildren<SDFObject>();
        numSDFObjects = Mathf.Clamp(objects.Length, 0, MAX_SDF_OBJECTS);
        for (int i = 0; i < numSDFObjects; i++)
        {
            sdfObjects[i] = objects[i];
        }
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
