using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowAnimal : MonoBehaviour
{

    public GameObject sensor;
    private List<Material[]> defaultMatsList;
    private List<Material[]> highlightMatsList;
    private Material[] defaultMats1;
    private Material[] defaultMats2;
    private Material[] defaultMats3;
    private Material[] highlightMats1;
    private Material[] highlightMats2;
    private Material[] highlightMats3;

    public Material highlightMat;

    public GameObject terrain;
    public GameObject upReference;
    private GameObject mesh;
    public GameObject mainMesh1;
    public GameObject mainMesh2;
    public GameObject mainMesh3;

    private bool zoomedOut;
    private bool zoomedIn;

    public bool aniamalsHidden;
    public bool animalsUnHidden;

    private Vector3 upPos;
    private Vector3 downPos;


    // Start is called before the first frame update
    void Start()
    {
        downPos = transform.localPosition;
        
        defaultMatsList = new List<Material[]>();
        highlightMatsList = new List<Material[]>();

        for (int h = 0; h < transform.childCount; h++)
        {
            if (transform.GetChild(h).CompareTag("Mesh"))
            {
                mesh = transform.GetChild(h).gameObject;
            }
        }

        if (mainMesh1 != null)
        {
            defaultMats1 = mainMesh1.GetComponent<Renderer>().materials;
            highlightMats1 = new Material[defaultMats1.Length];
            for (int j = 0; j < highlightMats1.Length; j++)
            {
                highlightMats1[j] = highlightMat;
            }
        }
        
        if (mainMesh2 != null)
        {
            defaultMats2 = mainMesh2.GetComponent<Renderer>().materials;
            highlightMats2 = new Material[defaultMats2.Length];
            for (int j = 0; j < highlightMats2.Length; j++)
            {
                highlightMats2[j] = highlightMat;
            }
        }

        if (mainMesh3 != null)
        {
            defaultMats3 = mainMesh3.GetComponent<Renderer>().materials;
            highlightMats3 = new Material[defaultMats3.Length];
            for (int j = 0; j < highlightMats3.Length; j++)
            {
                highlightMats3[j] = highlightMat;
            }
        }
        
        /*
        for (int i = 0; i < mesh.transform.childCount; i++)
        {
            if (mesh.transform.GetChild(i).GetComponent<Renderer>())
            {
                defaultMatsList.Add(transform.GetChild(0).GetChild(i).GetComponent<Renderer>().materials);
                Material[] highlightMats = new Material[defaultMatsList[i].Length];
                for (int j = 0; j < defaultMatsList[i].Length; j++)
                {
                    highlightMats[j] = highlightMat;
                }
                highlightMatsList.Add(highlightMats);
            }
        }*/
    }

    public void showAnimals()
    {
        
        
        print("shown");
        if (Vector3.Distance(transform.position, upReference.transform.position) < 0.01f)
        {
            if (!animalsUnHidden)
            {
                sensor.GetComponent<Zoom>().changeState(2);
                animalsUnHidden = true;
            }
            
        }
        else
        {
            animalsUnHidden = false;
            transform.position = Vector3.MoveTowards(transform.position,
                upReference.transform.position, 0.0005f);
        }
    }

    public void hideAnimals()
    {
        if (Vector3.Distance(transform.position, downPos) < 0.01)
        {
            if (animalsUnHidden)
            {
                animalsUnHidden = false;
            }
            
        }
        else
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition,
                downPos, 0.001f);
        }
        
    }

    public void highlightAnimal()
    {
        if (mainMesh1 != null)
        {
            mainMesh1.GetComponent<Renderer>().materials = highlightMats1;
        }
        if (mainMesh2 != null)
        {
            mainMesh2.GetComponent<Renderer>().materials = highlightMats2;
        }
        if (mainMesh3 != null)
        {
            mainMesh3.GetComponent<Renderer>().materials = highlightMats3;
        }
    }

    public void unHighlightAnimal()
    {
        if (mainMesh1 != null)
        {
            mainMesh1.GetComponent<Renderer>().materials = defaultMats1;
        }
        if (mainMesh2 != null)
        {
            mainMesh2.GetComponent<Renderer>().materials = defaultMats2;
        }
        if (mainMesh3 != null)
        {
            mainMesh3.GetComponent<Renderer>().materials = defaultMats3;
        }
    }

    public bool getZoomOutState()
    {
        return zoomedOut;
    }

    public void zoomAnimalIn()
    {
        if (terrain.transform.localScale.x < 1)
        {
            terrain.transform.localScale = new Vector3(terrain.transform.localScale.x + 0.05f,
                terrain.transform.localScale.y + 0.05f, terrain.transform.localScale.z + 0.05f);
        }
        else
        {
            sensor.GetComponent<Zoom>().changeState(3);
        }
        unHighlightAnimal();
    }

    public void zoomAnimalOut()
    {
        if (terrain.transform.localScale.x > 0)
        {
            terrain.transform.localScale = new Vector3(terrain.transform.localScale.x - 0.1f,
                terrain.transform.localScale.y - 0.1f, terrain.transform.localScale.z - 0.1f);
        }
        else
        {
            terrain.SetActive(false);
            terrain.transform.localScale = Vector3.zero;
            //zoomedOut = true;
            sensor.GetComponent<Zoom>().animalDetailZoomedOut = true;
        }
        unHighlightAnimal();
    }
    
}