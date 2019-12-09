using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{

    public GameObject mesh;

    public GameObject cPosition;
    public GameObject earth;
    public Material mat0;
    public Material mat1;
    public Material matOp0;
    public Material matOp1;

    private Material[] transMats;
    private Material[] opaqueMats;

    private bool zoomIn;

    private Vector3 oldPosition;
    // Start is called before the first frame update
    void Start()
    {
        oldPosition = cPosition.transform.localPosition;
        transMats = new Material[2] {mat0, mat1};
        opaqueMats = new Material[2]{matOp0, matOp1};
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
                //earth.GetComponent<MeshRenderer>().enabled = true;
                earth.GetComponent<Renderer>().materials = opaqueMats;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //earth.GetComponent<MeshRenderer>().enabled = false;
        //earth.GetComponent<Renderer>().materials[1].color = new Color(255,255,0,100);
        //earth.GetComponent<Renderer>().materials[0] = mat0;
        earth.GetComponent<Renderer>().materials = transMats;
        //mesh.GetComponent<Renderer>().material = mat0;
        zoomIn = true;
        
    }
}
