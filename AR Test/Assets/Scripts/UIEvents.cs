using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIEvents : MonoBehaviour
{
    
    
    public GameObject pnlKörpergröße;
    public GameObject txtKörpergröße;

    public GameObject pnlNahrung;
    
    public float ScaleGrößePanel = 1;
    public float ScaleGrößeText = 1;
    
    
    
    private Vector3 fullPnl ;
    private Vector3 fullTxt;
    private Vector3 noneTxt ;
    private Vector3 nonePnl ;
    private Vector3 nonePnlNahrung;
    private float TimeScale = 6f;
    private float TimeScale2 = 6f;

    private void Start()
    {
        fullPnl = new Vector3(ScaleGrößePanel,ScaleGrößePanel,ScaleGrößePanel);
        fullTxt = new Vector3(ScaleGrößeText,ScaleGrößeText,ScaleGrößeText);
        noneTxt = new Vector3(0,ScaleGrößeText,ScaleGrößeText);
        nonePnl = new Vector3(ScaleGrößePanel,0,ScaleGrößePanel);
        nonePnlNahrung = new Vector3(0,0,ScaleGrößePanel);
    }


    
    
    

    private IEnumerator HideNahrung()
    {
        float progress = 1;
     
        while(progress >= 0){
            pnlNahrung.transform.localScale = Vector3.Lerp(nonePnlNahrung, fullPnl, progress);
            progress -= Time.deltaTime * TimeScale;
            yield return null;
        }
        pnlNahrung.transform.localScale = nonePnlNahrung;
        
        pnlNahrung.SetActive(false);
    }

    private IEnumerator ShowNahrung()
    { 
        pnlNahrung.SetActive(true);
        float progress = 0;
     
        while(progress <= 1){
            pnlNahrung.transform.localScale = Vector3.Lerp(nonePnlNahrung, fullPnl, progress);
            progress += Time.deltaTime * TimeScale;
            yield return null;
        }
        pnlNahrung.transform.localScale = fullPnl;
        
    }

    public void ToggleNahrung()
    {
         if (pnlNahrung.activeSelf)
         {
             StartCoroutine(HideNahrung());
         }
         else
         {
             StartCoroutine(ShowNahrung());
         }
    }
    
    
    private IEnumerator ShowBodyHeight()
    {
        pnlKörpergröße.SetActive(true);

        float progress = 0;
     
        while(progress <= 1){
            pnlKörpergröße.transform.localScale = Vector3.Lerp(nonePnl, fullPnl, progress);
            progress += Time.deltaTime * TimeScale;
            yield return null;
        }
        pnlKörpergröße.transform.localScale = fullPnl;
        

        yield return new WaitForSeconds(0.1f);

        
        float progress2 = 0;
     
        while(progress2 <= 1){
            txtKörpergröße.transform.localScale = Vector3.Lerp(noneTxt, fullTxt, progress2);
            progress2 += Time.deltaTime * TimeScale2;
           yield return null;
        }

        txtKörpergröße.transform.localScale = fullTxt;
        
        
    }

    private IEnumerator HideBodyHeight()
    {
        float progress2 = 1;
     
        while(progress2 >= 0){
            txtKörpergröße.transform.localScale = Vector3.Lerp(noneTxt, fullTxt, progress2);
            progress2 -= Time.deltaTime * TimeScale2;
            yield return null;
        }

        txtKörpergröße.transform.localScale = noneTxt;

        yield return new WaitForSeconds(0.1f);

        float progress = 1;
     
        while(progress >= 0){
            pnlKörpergröße.transform.localScale = Vector3.Lerp(nonePnl, fullPnl, progress);
            progress -= Time.deltaTime * TimeScale;
            yield return null;
        }

        pnlKörpergröße.transform.localScale = nonePnl;
        
        pnlKörpergröße.SetActive(false);
    }

    public void ToggleBodyHeight()
    {
        if (pnlKörpergröße.activeSelf)
        {
            StartCoroutine(HideBodyHeight());
        }
        else
        {
            StartCoroutine(ShowBodyHeight());
        }
    }
}
