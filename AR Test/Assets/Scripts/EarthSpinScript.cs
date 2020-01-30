using System;
using UnityEngine;
using System.Collections;

public class EarthSpinScript : MonoBehaviour {
    public float speed = 1f;

    
    void OnMouseDrag()
    {
        transform.Rotate(Vector3.up, Input.GetAxis("Mouse X")*-speed, Space.World);
        transform.Rotate(Vector3.left, Input.GetAxis("Mouse Y")*-speed, Space.World);
    }


    
    void Update() {
        //transform.Rotate(Vector3.up, speed * Time.deltaTime, Space.World);
    }
}