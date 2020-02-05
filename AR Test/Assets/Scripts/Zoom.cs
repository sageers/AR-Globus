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
    private GameObject currentContinent;
    private GameObject currentAnimals;
    private GameObject currentAnimal;
    public GameObject continentZoomed;
    private GameObject currentZoomedContinent;
    private List<GameObject> currentLandmassMeshes;
    private Vector3 currentLandmassPos;
    private Quaternion currentLandmassRot;
    private Vector3 currentAnimalPos;
    public GameObject earthTrans;
    public Material matDefault;
    public Material matSelected;
    private List<Material> matsDefault;
    private Material matOp0;
    private Material matOp1;

    private Material[] transMats;
    private Material[] opaqueMats;

    private bool zoomIn;
    private bool zoomOut;
    private bool zoomAnimalIn;
    private bool zoomAnimalOut;
    private bool selected;
    private bool animalSelected;
    private bool alphaEnd;
    private bool shrink;
    private bool expand;
    private bool firstSelected;
    private bool firstSelectedAnimal;
    private bool changedToFlat;
    private bool flatRotationEnd;

    public bool questMode;

    private Vector3 oldPosition;
    // Start is called before the first frame update
    void Start()
    {
        //oldPosition = cPosition.transform.localPosition;
        //transMats = new Material[2] {mat0, mat1};
        //opaqueMats = new Material[2]{matOp0, matOp1};

        matsDefault = new List<Material>();
    }

    private void FixedUpdate()
    {

        if (!zoomIn && !zoomOut)
        {
            checkPlatformForTouch();
            checkSelection();
        }

        if (flatRotationEnd && !zoomAnimalIn)
        {
            checkPlatformForTouch();
            checkAnimalSelection();
        }

        if (zoomAnimalIn)
        {
            checkPlatformForTouch();
        }
        
        if (zoomIn)
        {
           zoomingIn();

        }
        else if(zoomOut)
        {
            zoomingOut();
        }

        if (zoomAnimalIn)
        {
            currentAnimal.GetComponent<ShowAnimal>().zoomAnimalIn();
        }
        
        if (zoomAnimalOut)
        {
            currentAnimal.GetComponent<ShowAnimal>().zoomAnimalOut();
            zoomAnimalOut = !currentAnimal.GetComponent<ShowAnimal>().getZoomOutState();
        }
        
        
        
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Landmass") && !selected && !zoomIn)
        {
            selected = true;
            currentLandmassMesh = other.gameObject;
            currentLandmass = other.transform.parent.gameObject;
            matDefault = currentLandmassMesh.GetComponent<Renderer>().material;
            currentLandmassMesh.GetComponent<Renderer>().material = matSelected;
            currentLandmassPos = currentLandmass.transform.localPosition;
        }
        
        
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Landmass") && selected && !zoomIn)
        {
            currentLandmassMesh.GetComponent<Renderer>().material = matDefault;
            selected = false;
        }
    }*/

    private void checkPlatformForTouch()
    {
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

    private void checkSelection()
    {
        if (Vector3.Distance(globe.transform.position, transform.parent.position) <= globe.transform.localScale.x/8)
        {
            Ray raycastUpdate = Camera.main.ScreenPointToRay(new Vector2(Screen.width/2, Screen.height/2));
            RaycastHit raycastHitUpdate;
            if (Physics.Raycast(raycastUpdate, out raycastHitUpdate))
            {
                if (raycastHitUpdate.collider.CompareTag("Landmass"))
                {
                    if (firstSelected && raycastHitUpdate.collider.transform.parent.gameObject != currentLandmass)
                    {
                        for (int i = 0; i < currentLandmass.transform.childCount; i++)
                        {
                            currentLandmass.transform.GetChild(i).GetComponent<Renderer>().material = matsDefault[i];
                        }
                        //currentLandmassMesh.GetComponent<Renderer>().material = matDefault;
                        
                        selected = false;
                    }

                    if (!selected)
                    {
                        matsDefault.Clear();
                        currentLandmassMesh = raycastHitUpdate.collider.gameObject;
                        currentLandmass = raycastHitUpdate.collider.transform.parent.gameObject;
                        currentContinent = currentLandmass.transform.parent.gameObject;
                        for (int g = 0; g < continentZoomed.transform.childCount; g++)
                        {
                            if (continentZoomed.transform.GetChild(g).name == currentContinent.name)
                            {
                                currentZoomedContinent = continentZoomed.transform.GetChild(g).gameObject;
                            }
                        }
                        for (int h = 0; h < currentContinent.transform.childCount; h++)
                        {
                            if (currentContinent.transform.GetChild(h).CompareTag("Animals"))
                            {
                                currentAnimals = currentContinent.transform.GetChild(h).gameObject;
                            }
                        }
                        for (int i = 0; i < currentLandmass.transform.childCount; i++)
                        {
                            matsDefault.Add(currentLandmass.transform.GetChild(i).GetComponent<Renderer>().material);
                            currentLandmass.transform.GetChild(i).GetComponent<Renderer>().material = matSelected;
                        }
                        //matDefault = currentLandmassMesh.GetComponent<Renderer>().material;
                        //currentLandmassMesh.GetComponent<Renderer>().material = matSelected;
                        currentLandmassPos = currentContinent.transform.localPosition;
                        currentLandmassRot = currentContinent.transform.localRotation;
                        selected = true;
                    }
                    firstSelected = true;
                }
                else
                {
                    if (firstSelected && !zoomIn)
                    {
                        for (int i = 0; i < currentLandmass.transform.childCount; i++)
                        {
                            currentLandmass.transform.GetChild(i).GetComponent<Renderer>().material = matsDefault[i];
                        }
                        selected = false;
                    }
                }
            }
        }
        else
        {
            if (firstSelected && !zoomIn)
            {
                for (int i = 0; i < currentLandmass.transform.childCount; i++)
                {
                    currentLandmass.transform.GetChild(i).GetComponent<Renderer>().material = matsDefault[i];
                }
                selected = false;
            }
        }
    }
    
    private void checkAnimalSelection()
    {
        if (Vector3.Distance(globe.transform.position, transform.parent.position) <= globe.transform.localScale.x/8)
        {
            Ray raycastUpdate = Camera.main.ScreenPointToRay(new Vector2(Screen.width/2, Screen.height/2));
            RaycastHit raycastHitUpdate;
            if (Physics.Raycast(raycastUpdate, out raycastHitUpdate))
            {
                if (raycastHitUpdate.collider.CompareTag("Animal"))
                {
                    print("animal");
                    if (firstSelectedAnimal)
                    {
                        currentAnimal.GetComponent<ShowAnimal>().unHighlightAnimal();
                        animalSelected = false;
                    }
                    
                    if (!animalSelected)
                    {
                        currentAnimal = raycastHitUpdate.collider.gameObject;
                        currentAnimalPos = currentAnimal.transform.localPosition;
                        currentAnimal.GetComponent<ShowAnimal>().highlightAnimal();
                        animalSelected = true;
                    }
                    firstSelectedAnimal = true;
                    
                }
                else
                {
                    if (firstSelectedAnimal)
                    {
                        currentAnimal.GetComponent<ShowAnimal>().unHighlightAnimal();
                        animalSelected = false;
                    }
                    
                }
            }
        }
        else
        {
            if (firstSelectedAnimal)
            {
                currentAnimal.GetComponent<ShowAnimal>().unHighlightAnimal();
                animalSelected = false;
            }
        }
    }

    private void fadeOut()
    {
        for (int i = 0; i < globe.transform.childCount; i++)
        {
            if (globe.transform.GetChild(i).gameObject != currentContinent)
            {
                if (!globe.transform.GetChild(i).CompareTag("GlobeTrans") || !globe.transform.GetChild(i).CompareTag("AnimalsDetailed"))
                {
                    globe.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }

        for (int i = 0; i < earthTrans.transform.childCount; i++)
        {
            if (currentContinent.name != earthTrans.transform.GetChild(i).name)
            {
                earthTrans.transform.GetChild(i).gameObject.SetActive(true);
                for (int j = 0; j < earthTrans.transform.GetChild(i).transform.childCount; j++)
                {
                    
                    GameObject tempGO = earthTrans.transform.GetChild(i).transform.GetChild(j).gameObject;
                    Renderer tempRend = tempGO.GetComponent<Renderer>();
                    Material tempMat = tempRend.material;
                    tempMat.SetFloat("_Mode", 3f);
            
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
        }
            
        
    }

    private void changeToFlat()
    {
        if (!expand)
        {
            if (currentContinent.transform.localScale.x > 0.01f)
            {
                float speed = -0.02f;
                currentContinent.transform.localScale = new Vector3(currentContinent.transform.localScale.x + speed,currentContinent.transform.localScale.y  + speed,currentContinent.transform.localScale.z + speed);
            }
            else
            {
                for(int i = 0; i < currentContinent.transform.childCount; i++)
                {
                    if (currentContinent.transform.GetChild(i).CompareTag("Landmass"))
                    {
                        currentContinent.transform.GetChild(i).gameObject.SetActive(false);
                    }
                    
                    if (currentContinent.transform.GetChild(i).CompareTag("FlatLandmass") || currentContinent.transform.GetChild(i).CompareTag("Animals"))
                    {
                        currentContinent.transform.GetChild(i).gameObject.SetActive(true);
                    }
                }
                expand = true;
            }
        }
        else
        {
            if (currentContinent.transform.localScale.x < currentZoomedContinent.transform.localScale.x)
            {
                float speed = 0.1f;
                currentContinent.transform.localScale = new Vector3(currentContinent.transform.localScale.x + speed,currentContinent.transform.localScale.y  + speed,currentContinent.transform.localScale.z + speed);
            }
            else
            {
                changedToFlat = true;
            }
        }
    }

    private void flatRotate()
    {
        Quaternion target = currentZoomedContinent.transform.localRotation;
        //Vector3 current = currentContinent.transform.rotation;
        currentContinent.transform.rotation = Quaternion.Slerp(currentContinent.transform.rotation, target,  0.06f);
        
        if ((target.x <= currentContinent.transform.localRotation.x + 1f && target.x >= currentContinent.transform.localRotation.x - 1f))
        {
            
            flatRotationEnd = true;
        }
       // Vector3.RotateTowards(currentContinent.transform.rotation, target, 1.0f, 1.0f)
        /*if (Quaternion.Euler(currentContinent.transform.rotation.x >= 0.5f))
        {
            print("rotation: " + currentContinent.transform.rotation);
            currentContinent.transform.Rotate(-1f,0,0);
        }*/
        
    }

    private void showAnimals()
    {
        for (int i = 0; i < currentAnimals.transform.childCount; i++)
        {
            currentAnimals.transform.GetChild(i).GetComponent<ShowAnimal>().showAnimals();
        }
    }

    private void hideAnimals()
    {
        for (int i = 0; i < currentAnimals.transform.childCount; i++)
        {
            currentAnimals.transform.GetChild(i).GetComponent<ShowAnimal>().hideAnimals();
        }
    }

    public void globeMode()
    {
        zoomIn = false;
        zoomOut = true;
    }

    public String currentAnimalName()
    {
        if (firstSelected)
        {
            return currentAnimal.name;
        }
        else
        {
            return "no Animal Found";
        }
        
    }

    public void landMode()
    {
        
    }

    private void zoomingIn()
    {
         
            fadeOut();

            currentContinent.transform.position = Vector3.MoveTowards(currentContinent.transform.position, currentZoomedContinent.transform.position, 0.002f);
            if (Vector3.Distance(currentLandmass.transform.position, currentZoomedContinent.transform.position) < 0.001f)
            {
                
                if (!changedToFlat)
                {
                    
                    changeToFlat();
                }
                else
                {
                    flatRotate();
                }

                if (flatRotationEnd)
                {
                    showAnimals();
                }

            }
            
            
            /*if (Vector3.Distance(globe.transform.position, transform.position) >= 0.6 && changedToFlat)
            {
                zoomIn = false;
                zoomOut = true;
            }*/
            
    }
    
    private void zoomingOut()
    {
        if (currentContinent.transform.localScale.x <= 1.1 &&
            Vector3.Distance(currentContinent.transform.localPosition, currentLandmassPos) < 0.07f)
        {
            if (alphaEnd)
            {
                zoomOut = false;
                
            }
            
        }
        
        for(int h = 0; h < currentContinent.transform.childCount; h++)
        {
            if(currentContinent.transform.GetChild(h).CompareTag("Landmass")){
                    
                currentContinent.transform.GetChild(h).gameObject.SetActive(true);
            }
                
            if(currentContinent.transform.GetChild(h).CompareTag("FlatLandmass") || currentContinent.transform.GetChild(h).CompareTag("Animals"))
            { 
                currentContinent.transform.GetChild(h).gameObject.SetActive(false);
            }
        }
        
        if (currentContinent.transform.localScale.x >= 1.1)
        {
            currentContinent.transform.localScale = new Vector3(currentContinent.transform.localScale.x - 0.1f,currentContinent.transform.localScale.y  - 0.1f,currentContinent.transform.localScale.z - 0.1f);
        }

        currentContinent.transform.localPosition = Vector3.MoveTowards(currentContinent.transform.localPosition, currentLandmassPos, 0.05f);

        Quaternion target = currentLandmassRot;
        currentContinent.transform.rotation = Quaternion.Slerp(currentContinent.transform.rotation, target,  0.2f);
        
        hideAnimals();
        for(int h = 0; h < currentLandmass.transform.childCount; h++)
        {
            /*
            GameObject tempGO = currentLandmass.transform.GetChild(h).gameObject;
            Renderer tempRend = tempGO.GetComponent<Renderer>();
            Material tempMat = tempRend.material;
            tempGO.SetActive(true);
            tempMat.SetFloat("_Mode", 3f);
            Color32 col = tempRend.material.GetColor("_Color");
            if(tempGO.CompareTag("Landmass"))
            {
                
                if (col.a <= 249)
                {
                    col.a += 6;
                }
                else
                {
                    currentLandmass.transform.GetChild(h).gameObject.SetActive(true);
                }
                
            }
            else
            {
                if (col.a >= 6)
                {
                    col.a -= 6;
                }
                else
                {
                    currentLandmass.transform.GetChild(h).gameObject.SetActive(false);
                }
            }
            tempRend.material.SetColor("_Color", col);
                    
            tempMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            tempMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            tempMat.SetInt("_ZWrite", 0);
            tempMat.DisableKeyword("_ALPHATEST_ON");
            tempMat.EnableKeyword("_ALPHABLEND_ON");
            tempMat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            tempMat.renderQueue = 3000;*/
        }
        
        for (int i = 0; i < earthTrans.transform.childCount; i++)
        {
            if (earthTrans.transform.GetChild(i).name != currentContinent.name)
            {
                for (int j = 0; j < earthTrans.transform.GetChild(i).transform.childCount; j++)
                {
                    if(!alphaEnd){
                        GameObject tempGO = earthTrans.transform.GetChild(i).transform.GetChild(j).gameObject;
                        Renderer tempRend = tempGO.GetComponent<Renderer>();
                        Material tempMat = tempRend.material;
                        tempMat.SetFloat("_Mode", 3f);
                        
                        Color32 col = tempRend.material.GetColor("_Color");
                        if (col.a <= 249)
                        {
                            col.a += 6;
                        }
                        else
                        {
                            alphaEnd = true;
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

            if (alphaEnd)
            {
                earthTrans.transform.GetChild(i).gameObject.SetActive(false);
            }
            
        }

        if (alphaEnd)
        {
            for (int i = 0; i < globe.transform.childCount; i++)
            {
                
                if (!globe.transform.GetChild(i).CompareTag("GlobeTrans"))
                {
                    globe.transform.GetChild(i).gameObject.SetActive(true);
                }
                
            }
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
                for (int i = 0; i < currentLandmass.transform.childCount; i++)
                {
                    currentLandmass.transform.GetChild(i).GetComponent<Renderer>().material = matsDefault[i];
                }
                alphaEnd = false;
                expand = false;
                changedToFlat = false;
                flatRotationEnd = false;
            }
            if (animalSelected && raycastHit.collider.gameObject == currentAnimal)
            {
                if (!questMode)
                {
                    zoomAnimalIn = true;
                    animalSelected = false;
                    currentAnimal.GetComponent<Collider>().enabled = false;
                    currentAnimal.GetComponent<ShowAnimal>().terrain.SetActive(true);
                    currentAnimal.GetComponent<ShowAnimal>().setParentToTarget();
                    currentAnimal.GetComponent<ShowAnimal>().unHighlightAnimal();
                    for(int h = 0; h < currentContinent.transform.childCount; h++)
                    {
                        if(currentContinent.transform.GetChild(h).CompareTag("FlatLandmass"))
                        { 
                            currentContinent.transform.GetChild(h).gameObject.SetActive(false);
                        }

                        for (int i = 0; i < currentContinent.transform.GetChild(h).childCount; i++)
                        {
                            if (currentContinent.transform.GetChild(h).GetChild(i).gameObject != currentAnimal)
                            {
                                currentContinent.transform.GetChild(h).GetChild(i).gameObject.SetActive(false);
                            }
                        }
                    
                    }
                }
                else
                {
                    //hier kannst du das rein setzen was der Questmodus machen soll wenn das Tier angetippt wird. 
                }
                
            }

            if (zoomAnimalIn && raycastHit.collider.CompareTag("Terrain"))
            {
                zoomAnimalIn = false;
                zoomAnimalOut = true;
                animalSelected = false;
                currentAnimal.GetComponent<Collider>().enabled = true;
                currentAnimal.GetComponent<ShowAnimal>().Canvas.SetActive(false);
                currentAnimal.GetComponent<ShowAnimal>().setParentToOrigin();
                currentAnimal.GetComponent<ShowAnimal>().unHighlightAnimal();
                for(int h = 0; h < currentContinent.transform.childCount; h++)
                {
                    if(currentContinent.transform.GetChild(h).CompareTag("FlatLandmass"))
                    { 
                        currentContinent.transform.GetChild(h).gameObject.SetActive(true);
                    }

                    for (int i = 0; i < currentContinent.transform.GetChild(h).childCount; i++)
                    {
                        if (currentContinent.transform.GetChild(h).GetChild(i).gameObject != currentAnimal)
                        {
                            currentContinent.transform.GetChild(h).GetChild(i).gameObject.SetActive(true);
                        }
                    }
                    
                }
            }
            
        }
    }
}
