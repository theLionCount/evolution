using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WASDCamera : MonoBehaviour
{
    Camera cam;
    public float speed = 0.03f;
    public float zoomSpeed = 0.03f;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        var pos = transform.position;
        if (Input.GetKey(KeyCode.W)) pos.y += cam.orthographicSize * speed;
        if (Input.GetKey(KeyCode.A)) pos.x -= cam.orthographicSize * speed;
        if (Input.GetKey(KeyCode.S)) pos.y -= cam.orthographicSize * speed;
        if (Input.GetKey(KeyCode.D)) pos.x += cam.orthographicSize * speed;
        if (Input.GetKey(KeyCode.UpArrow)) cam.orthographicSize -= zoomSpeed;
        if (Input.GetKey(KeyCode.DownArrow)) cam.orthographicSize += zoomSpeed;
        transform.position = pos;
    }
}
