using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastHit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
//
//        for (var i = 0; i < Input.touchCount; i++)
//        {
//            if (Input.GetTouch(i).phase == TouchPhase.Began)
//            {
//                RaycastHit hit;
//                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
//                
//                if (Physics.Raycast(ray, out hit))
//                {
//                    if (hit.collider != null)
//                    {
//                        hit.collider.enabled = false;
//                    }
//                }
//            }
//        }



    public Material highlight;

    private void checkTouch(Vector3 pos)
    {
        Ray raycast = Camera.main.ScreenPointToRay(pos);
        RaycastHit raycastHit;
        if (Physics.Raycast(raycast, out raycastHit))
        {
            if (raycastHit.collider.CompareTag("Animal"))
            {
                raycastHit.transform.gameObject.GetComponent<MeshRenderer>().material = highlight;
            }
        }
    }
}
