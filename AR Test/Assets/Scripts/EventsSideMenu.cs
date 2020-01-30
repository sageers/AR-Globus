using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventsSideMenu : MonoBehaviour
{
    private void Start()
    {
        var positionOld = globus.transform.position;
        slider.GetComponent<Slider>().minValue = positionOld.y;
    }

    public GameObject globus;
    public GameObject slider;

    public GameObject sidePanel;
    
    
    public void ChangeHeight()
    {
        var positionOld = globus.transform.position;
        
        
        var value = slider.GetComponent<Slider>().value;
        Vector3 t = new Vector3(positionOld.x,value,positionOld.z);
        globus.transform.position = t;
    }

    public void ToggleSideMenü()
    {
        sidePanel.SetActive(!sidePanel.activeSelf);
    }
}
