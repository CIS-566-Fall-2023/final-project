using UnityEngine;

public class SDFObject : MonoBehaviour
{
    public enum SDFObjectType
    {
        Sphere,
        Cube
    }

    [SerializeField] private SDFObjectType type;
    public SDFObjectType Type => type;
    [SerializeField] private float size = 0.02f;
    public float Size => size;
}