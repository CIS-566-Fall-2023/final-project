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
        Intersect = 2
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
    public Color Color => color;
}