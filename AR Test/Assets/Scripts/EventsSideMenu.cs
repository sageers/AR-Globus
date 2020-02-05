using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventsSideMenu : MonoBehaviour
{
    private Color32 blau;
    private void Start()
    {
        blau = new Color32(88,150,255,255);
        var positionOld = globus.transform.position;
        slider.GetComponent<Slider>().minValue = positionOld.y;
    }

    public GameObject globus;
    public GameObject slider;

    public GameObject sidePanel;
    public Button menuButton;
    
    public GameObject QuestUI;
    private bool questModeactive = false;
    public Button btn_QuestMode;



    public void ActivateQuestMode()
    {
        QuestUI.SetActive(true);
        QuestUI.GetComponentInChildren<QuestModeUIEvents>().newQ();
    }

    public void toggleQuestMode()
    {
        if (questModeactive)
        {
            QuestUI.GetComponentInChildren<QuestModeUIEvents>().stopQuestMode();
            btn_QuestMode.GetComponent<Image>().color = Color.white;
            btn_QuestMode.GetComponentInChildren<TextMeshProUGUI>().color = blau;
            btn_QuestMode.GetComponentInChildren<TextMeshProUGUI>().text = "Quest-Modus einschalten";
            questModeactive = false;
        }
        else
        {
            QuestUI.GetComponentInChildren<QuestModeUIEvents>().newQ();
            btn_QuestMode.GetComponent<Image>().color = blau;
            btn_QuestMode.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
            btn_QuestMode.GetComponentInChildren<TextMeshProUGUI>().text = "Quest-Modus ausschalten";
            questModeactive = true;
        }
    }
    
    public void DeactivateQuestMode()
    {
        QuestUI.SetActive(false);
    }
    
    
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

        menuButton.GetComponentInChildren<TextMeshProUGUI>().text = sidePanel.activeSelf ? "Zurück" : "Menü";
    }
    
    
}
