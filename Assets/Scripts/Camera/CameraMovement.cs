using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Camera camera;
    public GameObject parent;
    public float moveSpeed = 4.0f;
    public float rotateSpeed = 20.0f;
    public float zoomSensitivity = 20.0f;
    public float zoomSpeed = 20.0f;
    public float zoomMin = 5.0f;
    public float zoomMax = 80.0f;

    private float zoom;

    // Start is called before the first frame update
    void Start()
    {
            zoom = camera.fieldOfView;
            camera.transform.LookAt(parent.transform.position);
    }
    
    // Update is called once per frame
    void Update()
    {
        //Check if the mouse has been clicked and route the camera based on the mouse movement//
        if(Input.GetMouseButton(0))
        {
            float h = 5 * Input.GetAxis("Mouse Y");
            float y = (5 * Input.GetAxis("Mouse X")) * -1;

            Vector3 axisOfRotaion = new Vector3(h,y, 0) * 10;
            camera.transform.LookAt(transform.position);
            camera.transform.RotateAround(parent.transform.position, axisOfRotaion, 30 * Time.deltaTime);
        }

        float horizontal = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float vertical = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        camera.transform.Translate(horizontal, 0, vertical);

        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.up, -rotateSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
        }

        zoom -=  Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;
        zoom = Mathf.Clamp(zoom, zoomMin, zoomMax);


    }

    void LateUpdate()
    {
        //camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, zoom, Time.deltaTime * zoomSpeed);
    }
}
