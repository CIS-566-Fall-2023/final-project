using UnityEngine;

public class SDFObject : MonoBehaviour
{
    public enum SDFObjectType
    {
        Sphere = 0,
        Cube = 1,
        Torus = 2,
        Cylinder = 3,
        Capsule = 4,
        Octahedron = 5,
        Cone = 6,
    }

    public enum BlendOperationEnum
    {
        Add = 0,
        Subtract = 1,
        Intersect = 2,
        ColorBlend = 3,
    }

    public enum ProceduralTextureType
    {
        Plain = 0,
        StripedTriplanar = 1,
        WavesTriplanar = 2,
        DotsTriplanar = 3
    }

    [SerializeField] private SDFObjectType type;
    public SDFObjectType Type => type;

    [SerializeField] private Vector4 data = new Vector4(0.3f, 0, 0, 0);
    public Vector4 Data => data;

    [SerializeField] private BlendOperationEnum blendOperation;
    public BlendOperationEnum BlendOperation => blendOperation;

    [Range(0.0f, 10.0f)]
    [SerializeField] private float blendFactor = 0.02f;
    public float BlendFactor => blendFactor;

    [SerializeField] private Color color = Color.white;
    public Color PrimaryColor => color;

    [SerializeField] private Color secondaryColor = Color.black;
    public Color SecondaryColor => secondaryColor;

    [SerializeField] private ProceduralTextureType textureType = ProceduralTextureType.Plain;
    public ProceduralTextureType TextureType => textureType;

    [SerializeField, Range(0, 1)] private float smoothness = 0.0f;
    public float Smoothness => smoothness;

    [SerializeField, Range(0, 1)] private float metallic = 0.0f;
    public float Metallic => metallic;

    [SerializeField, ColorUsage(false, true)] private Color emissionColor;
    public Color EmissionColor => emissionColor;
}