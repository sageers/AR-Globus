using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{

    public GameObject globe;

    
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

    public GameObject questUI;
    public GameObject panel;
    public GameObject panel_k;

    private bool zoomIn;
    private bool zoomOut;
    private bool zoomAnimalIn;
    private bool zoomAnimalOut;
    private bool selected;
    private bool animalSelected;
    private bool alphaEnd;
    private bool alphaEnd2;
    private bool shrink;
    private bool expand;
    private bool firstSelected;
    private bool firstSelectedAnimal;
    private bool changedToFlat;
    private bool flatRotationEnd;
    private bool switchedToAnimal;

    private bool globeState;
    private bool landState;
    private bool animalState;

    public bool animalDetailZoomedOut;

    private int NoStateNumber = 0;
    private int globeStateNumber = 1;
    private int landStateNumber = 2;
    private int animalStateNumber = 3;

    public bool questMode;
    
    

    private Vector3 oldPosition;
    // Start is called before the first frame update
    void Start()
    {
        //oldPosition = cPosition.transform.localPosition;
        //transMats = new Material[2] {mat0, mat1};
        //opaqueMats = new Material[2]{matOp0, matOp1};

        globeState = true;
        matsDefault = new List<Material>();
    }

    private void FixedUpdate()
    {

        if (globeState)
        {
            
            checkSelection();
        }

        if (landState)
        {
            checkAnimalSelection();
        }

        if (zoomAnimalIn && !animalState)
        {
            
            changeToAnimal();
        }
        
        if (zoomIn && !landState)
        {
           zoomingIn();

        }
        
        if(zoomOut && !globeState)
        {
            zoomingOut();
        }

        if (zoomAnimalOut && !landState)
        {
            changeAnimalToFlat();
        }

        if (globeState || landState || animalState)
        {
            checkPlatformForTouch();
        }
        
        
        //print("globeState: " + globeState+ ", landState: " + landState + ", animalState: " + animalState);
    }

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
                if (!globe.transform.GetChild(i).CompareTag("GlobeTrans"))
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
    
    private void fadeIn()
    {
        //print("alphaEnd2: " + alphaEnd2);
        if (alphaEnd2)
        {
            for (int i = 0; i < globe.transform.childCount; i++)
            {
                if (globe.transform.GetChild(i).gameObject != currentContinent)
                {
                    if (!globe.transform.GetChild(i).CompareTag("GlobeTrans"))
                    {
                        globe.transform.GetChild(i).gameObject.SetActive(true);
                    }
                }
            }

            alphaEnd2 = false;
        }
        else
        {
            for (int i = 0; i < earthTrans.transform.childCount; i++)
            {
                if (currentContinent.name != earthTrans.transform.GetChild(i).name)
                {
                
                    for (int j = 0; j < earthTrans.transform.GetChild(i).transform.childCount; j++)
                    {
                    
                        GameObject tempGO = earthTrans.transform.GetChild(i).transform.GetChild(j).gameObject;
                        Renderer tempRend = tempGO.GetComponent<Renderer>();
                        Material tempMat = tempRend.material;
                        tempMat.SetFloat("_Mode", 3f);
            
                        Color32 col = tempRend.material.GetColor("_Color");
                        if (col.a <= 240)
                        {
                            col.a += 15;
                        }
                        else
                        {
                            alphaEnd2 = true;
                            earthTrans.transform.GetChild(i).gameObject.SetActive(false);
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
        

        
            
        
    }

    private void changeAnimalToFlat()
    {
        if (!animalDetailZoomedOut)
        {
            currentAnimal.GetComponent<ShowAnimal>().zoomAnimalOut();
        }
        else
        {
            for(int i = 0; i < currentContinent.transform.childCount; i++)
            {
                if (currentContinent.transform.GetChild(i).CompareTag("FlatLandmass") || currentContinent.transform.GetChild(i).CompareTag("Animals"))
                {
                    currentContinent.transform.GetChild(i).gameObject.SetActive(true);
                }
            }
            if (currentContinent.transform.localScale.x < currentZoomedContinent.transform.localScale.x)
            {
                float speed = 0.1f;
                currentContinent.transform.localScale = new Vector3(currentContinent.transform.localScale.x + speed,currentContinent.transform.localScale.y  + speed,currentContinent.transform.localScale.z + speed);
            }
            else
            {
                showAnimals();
            }
        }

    }

    private void changeToAnimal()
    {
        if (!switchedToAnimal)
        {
            currentAnimal.GetComponent<ShowAnimal>().hideAnimals();
            if (currentContinent.transform.localScale.x > 0.01f)
            {
                float speed = -0.06f;
                currentContinent.transform.localScale = new Vector3(currentContinent.transform.localScale.x + speed,currentContinent.transform.localScale.y  + speed,currentContinent.transform.localScale.z + speed);
            }
            else
            {
                for(int i = 0; i < currentContinent.transform.childCount; i++)
                {
                    if (currentContinent.transform.GetChild(i).CompareTag("Landmass") || currentContinent.transform.GetChild(i).CompareTag("FlatLandmass") || currentContinent.transform.GetChild(i).CompareTag("Animals"))
                    {
                        currentContinent.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
                switchedToAnimal = true;
            }
        }
        else
        {
            currentAnimal.GetComponent<ShowAnimal>().zoomAnimalIn();
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
        
        if ((target.x <= currentContinent.transform.localRotation.x + 2f && target.x >= currentContinent.transform.localRotation.x - 2f))
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
            if (currentAnimals.transform.GetChild(i).CompareTag("Animal"))
            {
                currentAnimals.transform.GetChild(i).GetComponent<ShowAnimal>().showAnimals();
            }
            
        }
    }

    private void hideAnimals()
    {
        for (int i = 0; i < currentAnimals.transform.childCount; i++)
        {
            if (currentAnimals.transform.GetChild(i).CompareTag("Animal"))
            {
                currentAnimals.transform.GetChild(i).GetComponent<ShowAnimal>().hideAnimals();
            }
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
            else
            {
                
            }
            
    }
    
    private void zoomingOut()
    {
        if (currentContinent.transform.localScale.x <= 1.1 &&
            Vector3.Distance(currentContinent.transform.localPosition, currentLandmassPos) < 0.07f)
        {
            if (alphaEnd2)
            {
                zoomOut = false;
                changeState(globeStateNumber);
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
        fadeIn();
    }


    private void checkTouch(Vector3 pos)
    {
        Ray raycast = Camera.main.ScreenPointToRay(pos);
        RaycastHit raycastHit;
        if (Physics.Raycast(raycast, out raycastHit))
        {
            if (selected && raycastHit.collider.gameObject == currentLandmassMesh)
            {
                changeState(NoStateNumber);
                zoomIn = true;
                for (int i = 0; i < currentLandmass.transform.childCount; i++)
                {
                    currentLandmass.transform.GetChild(i).GetComponent<Renderer>().material = matsDefault[i];
                }
                alphaEnd = false;
                //alphaEnd2 = false;
                expand = false;
                changedToFlat = false;
                flatRotationEnd = false;
            }
            if (animalSelected && raycastHit.collider.gameObject == currentAnimal)
            {
                if (panel.activeSelf || panel_k.activeSelf)
                {
                    //Questmodus

                    if (questUI.GetComponent<QuestModeUIEvents>().getSolution().Equals(currentAnimal.name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        questUI.SetActive(true);
                        questUI.GetComponent<QuestModeUIEvents>().nextQ();
                    }
                }
                else
                {
                    changeState(NoStateNumber);
                    zoomAnimalIn = true;
                    animalSelected = false;
                    currentAnimal.GetComponent<ShowAnimal>().terrain.SetActive(true);
                    currentAnimal.GetComponent<ShowAnimal>().unHighlightAnimal();
                }
            }

            
            
        }
        else
        {
            if (animalState)
            {
                changeState(NoStateNumber);
                animalModeToLandMode();
                
            }
            if (landState)
            {
                changeState(NoStateNumber);
                landModeToGlobeMode();
            }
        }
    }

    public void setGlobeState(bool b)
    {
        globeState = b;
    }
    
    public void setLandState(bool b)
    {
        landState = b;
    }
    
    public void setAnimalState(bool b)
    {
        animalState = b;
    }

    public void changeState(int state)
    {
        if (state == 1)
        {
            setGlobeState(true);
            setAnimalState(false);
            setLandState(false);
            resetBools();
        }
        if (state == 2)
        {
            setGlobeState(false);
            setAnimalState(false);
            setLandState(true);
            resetBools();
        }
        if (state == 3)
        {
            setGlobeState(false);
            setAnimalState(true);
            setLandState(false);
            resetBools();
        }
        if (state == 0)
        {
            setGlobeState(false);
            setAnimalState(false);
            setLandState(false);
        }
    }

    
    
    
    
    
    
    private void globeModeToLandMode()
    {
        
    }

    private void landModeToAnimalMode()
    {
        
    }

    private void animalModeToLandMode()
    {
        zoomAnimalIn = false;
        zoomAnimalOut = true;
        animalSelected = false;
        currentAnimal.GetComponent<ShowAnimal>().unHighlightAnimal();
    }

    private void landModeToGlobeMode()
    {
        zoomIn = false;
        zoomOut = true;
        animalSelected = false;
        selected = false; 
        zoomAnimalIn = false;
        zoomAnimalOut = false;
        animalSelected = false;
        alphaEnd = false;
        //alphaEnd2 = false;
        shrink = false;
        expand = false;
        firstSelected = false;
        firstSelectedAnimal = false;
        changedToFlat = false;
        flatRotationEnd = false;
        switchedToAnimal = false;
    }

    private void resetBools()
    {
        zoomIn = false;
        zoomOut = false;
        animalSelected = false;
        selected = false; 
        zoomAnimalIn = false;
        zoomAnimalOut = false;
        animalSelected = false;
        alphaEnd = false;
        //alphaEnd2 = false;
        shrink = false;
        expand = false;
        firstSelected = false;
        firstSelectedAnimal = false;
        changedToFlat = false;
        flatRotationEnd = false;
        switchedToAnimal = false;
        animalDetailZoomedOut = false;
    }
    
}
