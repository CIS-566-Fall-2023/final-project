using System.Runtime.CompilerServices;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(MeshRenderer))]
public class SDFCollection : MonoBehaviour
{
    #region Shader Properties
    private const int MAX_SDF_OBJECTS = 128;
    private static readonly int SDFTypeShaderPropertyID = Shader.PropertyToID("SDFType");
    private static readonly int SDFTransformMatricesPropertyID = Shader.PropertyToID("SDFTransformMatrices");
    private static readonly int SDFDataShaderPropertyID = Shader.PropertyToID("SDFData");
    private static readonly int SDFBlendOperationShaderPropertyID = Shader.PropertyToID("SDFBlendOperation");
    private static readonly int SDFBlendFactorShaderPropertyID = Shader.PropertyToID("SDFBlendFactor");
    private static readonly int SDFCountTotalShaderPropertyID = Shader.PropertyToID("SDFCountTotal");
    private static readonly int SDFCountCompoundedShaderPropertyID = Shader.PropertyToID("SDFCountCompounded");
    private static readonly int SDFOffsetsShaderPropertyID = Shader.PropertyToID("OffsetsToNextSdf");
    private static readonly int SDFPrimaryColorsPropertyID = Shader.PropertyToID("SDFPrimaryColors");
    private static readonly int SDFSecondaryColorsPropertyID = Shader.PropertyToID("SDFSecondaryColors");
    private static readonly int SDFTextureTypePropertyID = Shader.PropertyToID("SDFTextureType");
    private static readonly int SDFTextureDataShaderPropertyID = Shader.PropertyToID("SDFTextureData");

    private static readonly int SDFEmissionColorsPropertyID = Shader.PropertyToID("SDFEmissionColors");
    private static readonly int SDFSmoothnessPropertyID = Shader.PropertyToID("SDFSmoothness");
    private static readonly int SDFMetallicPropertyID = Shader.PropertyToID("SDFMetallic");
    #endregion

    private SDFObject[] sdfObjects;
    private int sdfCountTotal;      // compound objects are counted = number of the children in that compound
    private int sdfCountCompounded; // compound objects are counted as 1 object here

    [SerializeField] private SDFObject sdfObjectPrefab;

    private Renderer renderer;
    private MaterialPropertyBlock materialPropertyBlock;

    private Matrix4x4[] sdfTransformMatrices = new Matrix4x4[MAX_SDF_OBJECTS];
    private float[] sdfTypes = new float[MAX_SDF_OBJECTS];
    private Vector4[] sdfData = new Vector4[MAX_SDF_OBJECTS];
    private float[] sdfBlendOperations = new float[MAX_SDF_OBJECTS];
    private float[] sdfBlends = new float[MAX_SDF_OBJECTS];
    private Vector4[] sdfPrimaryColors = new Vector4[MAX_SDF_OBJECTS];
    private Vector4[] sdfSecondaryColors = new Vector4[MAX_SDF_OBJECTS];
    private float[] sdfTextureTypes = new float[MAX_SDF_OBJECTS];
    private Vector4[] sdfTextureData = new Vector4[MAX_SDF_OBJECTS];
    private float[] sdfSmoothnessValues = new float[MAX_SDF_OBJECTS];
    private float[] sdfMetallicValues = new float[MAX_SDF_OBJECTS];
    private Vector4[] sdfEmissionColors = new Vector4[MAX_SDF_OBJECTS];
    private float[] offsetsToNextSdf = new float[MAX_SDF_OBJECTS];

    private bool hasInitialized = false;

    private void OnEnable()
    {
        Initialize();
    }

    private void OnTransformChildrenChanged()
    {
        // OnTransformChildrenChanged() is NOT fired if a grand-child changes.
        // Only the immediate child hierarchy below this parent is evaluated and fires this.
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
        sdfData = new Vector4[MAX_SDF_OBJECTS];
        sdfBlendOperations = new float[MAX_SDF_OBJECTS];
        sdfBlends = new float[MAX_SDF_OBJECTS];
        sdfPrimaryColors = new Vector4[MAX_SDF_OBJECTS];
        sdfSmoothnessValues = new float[MAX_SDF_OBJECTS];
        sdfTextureTypes = new float[MAX_SDF_OBJECTS];
        sdfTextureData = new Vector4[MAX_SDF_OBJECTS];
        sdfMetallicValues = new float[MAX_SDF_OBJECTS];
        sdfEmissionColors = new Vector4[MAX_SDF_OBJECTS];
        offsetsToNextSdf = new float[MAX_SDF_OBJECTS];

        renderer = GetComponent<Renderer>();

        materialPropertyBlock = new MaterialPropertyBlock();

        ValidateTransformChildrenChange();

        SDFObject.OnChildrenUpdated += OnCompoundSDFObjectUpdated;

        hasInitialized = true;
    }

    private void Update()
    {
        if (!hasInitialized)
        {
            return;
        }

        for (int i = 0; i < sdfCountTotal; i++)
        {
            SDFObject sdf = sdfObjects[i];
            sdfTypes[i] = (int)sdf.Type;
            sdfTransformMatrices[i] = sdf.transform.worldToLocalMatrix;
            sdfData[i] = sdf.ShapeData;
            sdfBlendOperations[i] = (int)sdf.BlendOperation;
            sdfBlends[i] = sdf.BlendFactor;
            sdfPrimaryColors[i] = sdf.PrimaryColor;
            sdfSecondaryColors[i] = sdf.SecondaryColor;
            sdfTextureTypes[i] = (int)sdf.TextureType;
            sdfTextureData[i] = sdf.TextureData;
            sdfSmoothnessValues[i] = sdf.Smoothness;
            sdfMetallicValues[i] = sdf.Metallic;
            sdfEmissionColors[i] = sdf.EmissionColor;
        }

        materialPropertyBlock.SetFloatArray(SDFTypeShaderPropertyID, sdfTypes);
        materialPropertyBlock.SetMatrixArray(SDFTransformMatricesPropertyID, sdfTransformMatrices);
        materialPropertyBlock.SetVectorArray(SDFDataShaderPropertyID, sdfData);
        materialPropertyBlock.SetFloatArray(SDFBlendOperationShaderPropertyID, sdfBlendOperations);
        materialPropertyBlock.SetFloatArray(SDFBlendFactorShaderPropertyID, sdfBlends);
        materialPropertyBlock.SetInt(SDFCountTotalShaderPropertyID, sdfCountTotal);
        materialPropertyBlock.SetInt(SDFCountCompoundedShaderPropertyID, sdfCountCompounded);
        materialPropertyBlock.SetVectorArray(SDFPrimaryColorsPropertyID, sdfPrimaryColors);
        materialPropertyBlock.SetVectorArray(SDFSecondaryColorsPropertyID, sdfSecondaryColors);
        materialPropertyBlock.SetFloatArray(SDFTextureTypePropertyID, sdfTextureTypes);
        materialPropertyBlock.SetVectorArray(SDFTextureDataShaderPropertyID, sdfTextureData);
        materialPropertyBlock.SetFloatArray(SDFSmoothnessPropertyID, sdfSmoothnessValues);
        materialPropertyBlock.SetFloatArray(SDFMetallicPropertyID, sdfMetallicValues);
        materialPropertyBlock.SetVectorArray(SDFEmissionColorsPropertyID, sdfEmissionColors);
        materialPropertyBlock.SetFloatArray(SDFOffsetsShaderPropertyID, offsetsToNextSdf);

        renderer.SetPropertyBlock(materialPropertyBlock);
    }

    private void OnDisable()
    {
        if (!hasInitialized)
        {
            return;
        }

        hasInitialized = false;
        materialPropertyBlock.Clear();
        materialPropertyBlock = null;
        renderer.SetPropertyBlock(null);
    }

    private void OnDestroy()
    {
        SDFObject.OnChildrenUpdated -= OnCompoundSDFObjectUpdated;
    }

    #region HELPERS
    [ContextMenu("Add SDF Object")]
    private void AddSDFObject()
    {
        if (sdfCountCompounded == MAX_SDF_OBJECTS)
        {
            // Can't add any more to this SDFCollection!
            return;
        }

        SDFObject sdfObject = InstantiateSDFObject(transform);
    }

    public SDFObject InstantiateSDFObject(Transform parent)
    {
        SDFObject sdfObject = Instantiate(sdfObjectPrefab, parent);
        sdfObject.SetParentCollection(this);
        return sdfObject;
    }

    private void OnCompoundSDFObjectUpdated(SDFCollection parentCollection)
    {
        if (parentCollection != this)
        {
            // someone else's sdf child
            return;
        }

        ValidateTransformChildrenChange();
    }

    private void ValidateTransformChildrenChange()
    {
        SDFObject[] objects = GetComponentsInChildren<SDFObject>();
        int childCount = Mathf.Clamp(objects.Length, 0, MAX_SDF_OBJECTS);

        sdfCountTotal = 0;
        sdfCountCompounded = 0;
        
        for (int i = 0; i < childCount; i++)
        {
            SDFObject curObject = objects[i];
            if (curObject.IsChildOfCompoundSDF)
            {
                // this case happens when a child object is destroyed
                continue;
            }
            offsetsToNextSdf[sdfCountCompounded] = sdfCountTotal + curObject.NumSDFChildren + 1;
            sdfObjects[sdfCountTotal] = curObject;
            curObject.SetParentCollection(this);
            if (curObject.Type == SDFObject.SDFObjectType.Compound)
            {
                // for compound objects we don't send both parent and children to the GPU
                InsertCompoundObject(curObject);

                i += curObject.NumSDFChildren;
            }
            sdfCountTotal += curObject.NumSDFChildren + 1;
            sdfCountCompounded++;
        }
    }

    private void InsertCompoundObject(SDFObject compoundSdf)
    {
        for (int i = 0; i < compoundSdf.NumSDFChildren; i++)
        {
            SDFObject compObjChild = compoundSdf.SDFChildren[i];
            sdfObjects[sdfCountCompounded + i + 1] = compObjChild;
            compObjChild.SetParentCollection(this);
        }
    }
    #endregion

    private void TraverseCompound(int index, int lastIndex)
    {
        for (int i = index; i < lastIndex; i++)
        {
            int crap = 0;
        }
    }

    [ContextMenu("Run test")]
    private void Traverse()
    {
        int i = 0;
        int elementIndex = 0;
        int sdfsDone = 0;
        while (sdfsDone < sdfCountCompounded)
        {
            elementIndex = i;
            if (sdfObjects[i].Type == SDFObject.SDFObjectType.Compound)
            {
                elementIndex++;
            }
            TraverseCompound(elementIndex, (int)offsetsToNextSdf[sdfsDone]);
            i = (int)offsetsToNextSdf[sdfsDone];
            sdfsDone++;
        }

        Debug.Log("done");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
