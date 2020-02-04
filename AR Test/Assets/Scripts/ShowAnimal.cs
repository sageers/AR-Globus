using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowAnimal : MonoBehaviour
{
    private GameObject mesh;

    private GameObject targetUp;

    private GameObject targetDown;

    private GameObject targetScale;

    public GameObject mainMesh;

    private Material[] defaultMats;
    private Material[] highlightMats;

    public Material highlightMat;
    
    public GameObject Canvas;

    public GameObject terrain;
    
    
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
            
            if (transform.GetChild(i).name == "TargetScale")
            {
                targetScale = transform.GetChild(i).GetChild(0).gameObject;
            }
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
        
        mesh.transform.localPosition = Vector3.MoveTowards(mesh.transform.localPosition,
            targetUp.transform.localPosition, 0.001f);
    }

    public void hideAnimals()
    {
        mesh.transform.localPosition = Vector3.MoveTowards(mesh.transform.localPosition,
            targetDown.transform.localPosition, 0.001f);
    }

    public void highlightAnimal()
    {
        mainMesh.GetComponent<Renderer>().materials = highlightMats;
    }

    public void unHighlightAnimal()
    {
        
        mainMesh.GetComponent<Renderer>().materials = defaultMats;
    }

    public void zoomAnimalIn()
    {
        mesh.transform.position = Vector3.MoveTowards(mesh.transform.position, targetScale.transform.position, 0.05f);
        if (mesh.transform.localScale.x < targetScale.transform.localScale.x)
        {
            mesh.transform.localScale = new Vector3(mesh.transform.localScale.x + 0.01f,mesh.transform.localScale.y  + 0.01f,mesh.transform.localScale.z + 0.01f);
        }
        unHighlightAnimal();

    }
}
