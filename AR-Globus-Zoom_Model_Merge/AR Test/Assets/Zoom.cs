using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{

    public GameObject globe;

    private GameObject cPosition;
    private GameObject earth;
    private GameObject currentLandmass;
    private GameObject currentLandmassMesh;
    private Vector3 currentLandmassPos;
    public GameObject earthTrans;
    private Material mat0;
    public Material mat1;
    private Material matOp0;
    private Material matOp1;

    private Material[] transMats;
    private Material[] opaqueMats;

    public bool zoomIn;
    private bool selected;

    private Vector3 oldPosition;
    // Start is called before the first frame update
    void Start()
    {
        //oldPosition = cPosition.transform.localPosition;
        //transMats = new Material[2] {mat0, mat1};
        //opaqueMats = new Material[2]{matOp0, matOp1};
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        /*
        if (zoomIn)
        {
            if (currentLandmass.transform.localScale.x <= 2)
            {
                currentLandmass.transform.localScale = new Vector3(currentLandmass.transform.localScale.x + 0.1f,currentLandmass.transform.localScale.y  + 0.1f,currentLandmass.transform.localScale.z + 0.1f);
            }

            cPosition.transform.localPosition = Vector3.MoveTowards(cPosition.transform.localPosition, new Vector3(0, 0, 0), 0.07f);
            if (Vector3.Distance(earth.transform.position, transform.position) >= 0.6)
            {
                zoomIn = false;
            }
        }*/
        /*
        else
        {
            if (currentLandmass.transform.localScale.x > 1)
            {
                currentLandmass.transform.localScale = new Vector3(currentLandmass.transform.localScale.x - 0.1f,currentLandmass.transform.localScale.y  - 0.1f,currentLandmass.transform.localScale.z - 0.1f);
            }

            cPosition.transform.localPosition = Vector3.MoveTowards(cPosition.transform.localPosition, oldPosition, 0.07f);
            if (Vector3.Distance(cPosition.transform.localPosition, oldPosition) <= 0.001)
            {
                //earth.GetComponent<MeshRenderer>().enabled = true;
                //earth.GetComponent<Renderer>().materials = opaqueMats;
                //earth.GetComponent<Renderer>().enabled = true;
                //earthTrans.GetComponent<Renderer>().enabled = false;
                //earthTrans.transform.SetParent(earth.transform);
            }
        }*/

        if (zoomIn)
        {
            for (int i = 0; i < globe.transform.childCount; i++)
            {
                if (globe.transform.GetChild(i).gameObject != currentLandmass)
                {
                    GameObject tempGO = globe.transform.GetChild(i).transform.GetChild(0).gameObject;
                    Renderer tempRend = tempGO.GetComponent<Renderer>();
                    Material tempMat = tempRend.material;
                    tempMat.SetFloat("_Mode", 4f);
                    
                    Color32 col = tempRend.material.GetColor("_Color");
                    if (col.a > 6)
                    {
                        col.a -= 6;
                    }
                    
                    tempRend.material.SetColor("_Color", col);
                    
                    tempMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    tempMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    tempMat.SetInt("_ZWrite", 0);
                    tempMat.DisableKeyword("_ALPHATEST_ON");
                    tempMat.EnableKeyword("_ALPHABLEND_ON");
                    tempMat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    tempMat.renderQueue = 3000;
                }
            }

            currentLandmass.transform.localPosition =
                Vector3.MoveTowards(currentLandmass.transform.localPosition, Vector3.zero, 0.002f);
            if (currentLandmass.transform.localScale.x <= 2)
            {
                currentLandmass.transform.localScale = new Vector3(currentLandmass.transform.localScale.x + 0.01f,currentLandmass.transform.localScale.y  + 0.01f,currentLandmass.transform.localScale.z + 0.01f);
            }
            
            if (Vector3.Distance(globe.transform.position, transform.position) >= 0.6)
            {
                zoomIn = false;
            }

        }
        else
        {
            if (currentLandmass.transform.localScale.x > 1)
            {
                currentLandmass.transform.localScale = new Vector3(currentLandmass.transform.localScale.x - 0.1f,currentLandmass.transform.localScale.y  - 0.1f,currentLandmass.transform.localScale.z - 0.1f);
            }

            currentLandmass.transform.localPosition = Vector3.MoveTowards(currentLandmass.transform.localPosition, currentLandmassPos, 0.07f);
            for (int i = 0; i < globe.transform.childCount; i++)
            {
                if (globe.transform.GetChild(i).gameObject != currentLandmass)
                {
                    GameObject tempGO = globe.transform.GetChild(i).transform.GetChild(0).gameObject;
                    Renderer tempRend = tempGO.GetComponent<Renderer>();
                    Material tempMat = tempRend.material;
                    tempMat.SetFloat("_Mode", 4f);
                    
                    Color32 col = tempRend.material.GetColor("_Color");
                    if (col.a <= 249)
                    {
                        col.a += 6;
                    }
                    
                    tempRend.material.SetColor("_Color", col);
                    
                    tempMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    tempMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    tempMat.SetInt("_ZWrite", 0);
                    tempMat.DisableKeyword("_ALPHATEST_ON");
                    tempMat.EnableKeyword("_ALPHABLEND_ON");
                    tempMat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    tempMat.renderQueue = 3000;
                }
            }
        }
        
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (Input.touchCount > 0 && Input.touchCount < 2)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    checkTouch(Input.GetTouch(0).position);
                }
            }
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
            if (Input.GetMouseButtonDown(0))
            {
                checkTouch(Input.mousePosition);
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Landmass") && !selected && !zoomIn)
        {
            //earth.GetComponent<MeshRenderer>().enabled = false;
            //earth.GetComponent<Renderer>().materials[1].color = new Color(255,255,0,100);
            //earth.GetComponent<Renderer>().materials[0] = mat0;
            //earth.GetComponent<Renderer>().materials = transMats;
            selected = true;
            currentLandmassMesh = other.gameObject;
            currentLandmass = other.transform.parent.gameObject;
            mat0 = currentLandmassMesh.GetComponent<Renderer>().material;
            currentLandmassMesh.GetComponent<Renderer>().material = mat1;
            currentLandmassPos = currentLandmass.transform.localPosition;
            //mesh.GetComponent<Renderer>().material = mat0;
            //earth.GetComponent<Renderer>().enabled = false;
            //earthTrans.GetComponent<Renderer>().enabled = true;
            //earthTrans.transform.SetParent(other.transform.parent.transform);
            //zoomIn = true;
        }
        
        
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Landmass") && selected && !zoomIn)
        {
            //earth.GetComponent<MeshRenderer>().enabled = false;
            //earth.GetComponent<Renderer>().materials[1].color = new Color(255,255,0,100);
            //earth.GetComponent<Renderer>().materials[0] = mat0;
            //earth.GetComponent<Renderer>().materials = transMats;
            
            //mat0 = currentLandmass.GetComponent<Renderer>().material;
            currentLandmassMesh.GetComponent<Renderer>().material = mat0;
            selected = false;
            //currentLandmass = null;
            //mesh.GetComponent<Renderer>().material = mat0;
            //earth.GetComponent<Renderer>().enabled = false;
            //earthTrans.GetComponent<Renderer>().enabled = true;
            //earthTrans.transform.SetParent(other.transform.parent.transform);
            //zoomIn = true;
        }
        
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Landmass" && !selected)
        {
            //earth.GetComponent<MeshRenderer>().enabled = false;
            //earth.GetComponent<Renderer>().materials[1].color = new Color(255,255,0,100);
            //earth.GetComponent<Renderer>().materials[0] = mat0;
            //earth.GetComponent<Renderer>().materials = transMats;
            //currentLandmass = other.gameObject;
            //mat0 = currentLandmass.GetComponent<Renderer>().material;
            //currentLandmass.GetComponent<Renderer>().material = mat1;
            //mesh.GetComponent<Renderer>().material = mat0;
            //earth.GetComponent<Renderer>().enabled = false;
            //earthTrans.GetComponent<Renderer>().enabled = true;
            //earthTrans.transform.SetParent(other.transform.parent.transform);
            //zoomIn = true;
        }
        
        
    }
    
    private void checkTouch(Vector3 pos)
    {
        Ray raycast = Camera.main.ScreenPointToRay(pos);
        RaycastHit raycastHit;
        if (Physics.Raycast(raycast, out raycastHit))
        {
            if (selected && raycastHit.collider.gameObject == currentLandmassMesh)
            {
                zoomIn = true;
                currentLandmassMesh.GetComponent<Renderer>().material = mat0;
            }
        }
    }
}
