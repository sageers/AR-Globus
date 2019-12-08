using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{

    public GameObject mesh;

    public GameObject cPosition;
    public GameObject earth;

    private bool zoomIn;

    private Vector3 oldPosition;
    // Start is called before the first frame update
    void Start()
    {
        oldPosition = cPosition.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (zoomIn)
        {
            if (mesh.transform.localScale.x <= 2)
            {
                mesh.transform.localScale = new Vector3(mesh.transform.localScale.x + 0.1f,mesh.transform.localScale.y  + 0.1f,mesh.transform.localScale.z + 0.1f);
            }

            cPosition.transform.localPosition = Vector3.MoveTowards(cPosition.transform.localPosition, new Vector3(0, 0, 0), 0.07f);
            if (Vector3.Distance(earth.transform.position, transform.position) >= 0.6)
            {
                zoomIn = false;
            }
        }
        else
        {
            if (mesh.transform.localScale.x > 1)
            {
                mesh.transform.localScale = new Vector3(mesh.transform.localScale.x - 0.1f,mesh.transform.localScale.y  - 0.1f,mesh.transform.localScale.z - 0.1f);
            }

            cPosition.transform.localPosition = Vector3.MoveTowards(cPosition.transform.localPosition, oldPosition, 0.07f);
            if (Vector3.Distance(cPosition.transform.localPosition, oldPosition) <= 0.001)
            {
                earth.GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        earth.GetComponent<MeshRenderer>().enabled = false;
        zoomIn = true;
    }
}
