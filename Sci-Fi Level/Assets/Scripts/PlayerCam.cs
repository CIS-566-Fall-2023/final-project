using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX = 400f; // sensitivity
    public float sensY = 400f;

    public Transform orientation; // referencing Player prefab's components
    public Transform cameraPosition;

    private float xRotation;
    private float yRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // move camera
        transform.position = cameraPosition.position;

        // get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        // can't look up or down more than 90 degrees
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // rotate camera and player orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
