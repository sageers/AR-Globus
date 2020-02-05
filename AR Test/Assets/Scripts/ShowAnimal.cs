using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowAnimal : MonoBehaviour
{
    private GameObject mesh;
    private GameObject origin;

    private GameObject targetUp;

    private GameObject targetDown;

    public GameObject targetScaleAnimal;
    public GameObject targetScaleTerrain;
    private GameObject targetScale;

    public GameObject mainMesh;

    private Material[] defaultMats;
    private Material[] highlightMats;

    public Material highlightMat;

    public GameObject Canvas;

    public GameObject terrain;

    private bool zoomedOut;

    private float zoomInSpeed;

    private Quaternion oldRot;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == "TargetUp")
            {
                targetUp = transform.GetChild(i).GetChild(0).gameObject;
            }

            if (transform.GetChild(i).name == "TargetDown")
            {
                targetDown = transform.GetChild(i).GetChild(0).gameObject;
            }

            if (transform.GetChild(i).name == "Mesh")
            {
                mesh = transform.GetChild(i).GetChild(0).gameObject;
            }

            origin = mesh.transform.parent.gameObject;
            targetScale = targetScaleAnimal.transform.parent.gameObject;
            oldRot = mesh.transform.localRotation;
            zoomInSpeed = (targetScaleAnimal.transform.localScale.x - mesh.transform.localScale.x) / 50;
        }

        defaultMats = mainMesh.GetComponent<Renderer>().materials;
        highlightMats = new Material[defaultMats.Length];
        for (int i = 0; i < highlightMats.Length; i++)
        {
            highlightMats[i] = highlightMat;
        }

        Canvas.SetActive(false);
    }

    public void showAnimals()
    {
        print("show");
        mesh.transform.position = Vector3.MoveTowards(mesh.transform.position,
            targetUp.transform.position, 0.001f);
    }

    public void hideAnimals()
    {
        mesh.transform.position = Vector3.MoveTowards(mesh.transform.position,
            targetDown.transform.position, 0.001f);
    }

    public void highlightAnimal()
    {
        mainMesh.GetComponent<Renderer>().materials = highlightMats;
    }

    public void unHighlightAnimal()
    {
        mainMesh.GetComponent<Renderer>().materials = defaultMats;
    }

    public void setParentToOrigin()
    {
        mesh.transform.SetParent(origin.transform);
    }

    public void setParentToTarget()
    {
        mesh.transform.SetParent(targetScale.transform);
    }

    public bool getZoomOutState()
    {
        return zoomedOut;
    }

    public void zoomAnimalIn()
    {
        
        mesh.transform.position = Vector3.MoveTowards(mesh.transform.position,
            targetScaleAnimal.transform.position, 0.05f);
        if (mesh.transform.localScale.x < targetScaleAnimal.transform.localScale.x)
        {
            mesh.transform.localScale = new Vector3(mesh.transform.localScale.x + zoomInSpeed,
                mesh.transform.localScale.y + zoomInSpeed, mesh.transform.localScale.z + zoomInSpeed);
        }
        else
        {
            Canvas.SetActive(true);
            zoomedOut = false;
        }

        if (terrain.transform.localScale.x < targetScaleTerrain.transform.localScale.x)
        {
            terrain.transform.localScale = new Vector3(terrain.transform.localScale.x + 0.01f,
                terrain.transform.localScale.y + 0.01f, terrain.transform.localScale.z + 0.01f);
        }
        //mesh.transform.rotation = Quaternion.Slerp(mesh.transform.rotation, targetScaleAnimal.transform.rotation,  0.06f);

        unHighlightAnimal();
    }

    public void zoomAnimalOut()
    {
        mesh.transform.localPosition = Vector3.MoveTowards(mesh.transform.localPosition,
            targetUp.transform.localPosition, 0.1f);
        if (mesh.transform.localScale.x > targetUp.transform.localScale.x)
        {
            mesh.transform.localScale = new Vector3(mesh.transform.localScale.x - 0.01f,
                mesh.transform.localScale.y - 0.01f, mesh.transform.localScale.z - 0.01f);
        }

        if (terrain.transform.localScale.x > 0)
        {
            terrain.transform.localScale = new Vector3(terrain.transform.localScale.x - 0.1f,
                terrain.transform.localScale.y - 0.1f, terrain.transform.localScale.z - 0.1f);
        }
        else
        {
            terrain.SetActive(false);
            zoomedOut = true;
        }
        //mesh.transform.localRotation = Quaternion.Slerp(mesh.transform.localRotation, oldRot,  0.06f);
        unHighlightAnimal();
    }
}