using UnityEngine;

public class SDFObject : MonoBehaviour
{
    public enum SDFObjectType
    {
        Sphere,
        Cube
    }

    public enum BlendOperationEnum
    {
        Add = 0,
        Subtract = 1,
        Intersect = 2
    }

    [SerializeField] private SDFObjectType type;
    public SDFObjectType Type => type;
    [SerializeField] private float size = 0.02f;
    public float Size => size;

    [SerializeField] private BlendOperationEnum blendOperation;
    public BlendOperationEnum BlendOperation => blendOperation;

    [SerializeField] private float blendFactor = 0.02f;
    public float BlendFactor => blendFactor;
}