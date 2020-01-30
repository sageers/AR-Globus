using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ToggleTutorials : MonoBehaviour
{
    private bool toggle1, toggle2, toggle3;

    public GameObject panel1;
    public GameObject panel2;
    public GameObject panel3;

    public void Toggle1()
    {
        if (toggle1)
        {
            panel1.SetActive(false);
        }
        else
        {
            
        }
    }

    public void Toggle2()
    {
        
    }

    public void Toggle3()
    {
        
    }
    
}
