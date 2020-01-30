using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastHit : MonoBehaviour
{
    
    public Material highlight;

    private MeshRenderer meshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
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




   

    private void checkTouch(Vector3 pos)
    {
        Ray raycast = Camera.main.ScreenPointToRay(pos);
        RaycastHit raycastHit;
        if (Physics.Raycast(raycast, out raycastHit))
        {
            if (raycastHit.collider.name.Equals(this.name))
            {
                meshRenderer.material = highlight;
            }
        }
    }
}
