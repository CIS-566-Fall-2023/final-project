using UnityEngine;

public class Turntable : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 1.0f;

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
