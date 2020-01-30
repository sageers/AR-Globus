using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using Vuforia;

public class QuestModeUIEvents : MonoBehaviour
{

   public GameObject txt_Frage;
   public GameObject txt_Frage_klein;
   public GameObject panel_Frage;
   public GameObject panel_Frage_klein;
   public Button btn_Frage_weiter;
   
   
   private bool frage_weiter;

   public void toggleFrage()
   {
      frage_weiter = !frage_weiter;
   }

   public void newQ()
   {
      StartCoroutine(showNewQuestion());
   }
   
   public IEnumerator showNewQuestion()
   {
      panel_Frage_klein.SetActive(false);
      panel_Frage.SetActive(true);
      txt_Frage.GetComponent<TextMeshProUGUI>().text = "Get random Question out of Question-catalogue";
      txt_Frage_klein.GetComponent<TextMeshProUGUI>().text = "Get random Question out of Question-catalogue";

      var counter = 0;
      while (!frage_weiter)
      {
         yield return new WaitForSeconds(1);
         counter++;

         if (counter > 20)
         {
            break;
         }
      }

      frage_weiter = false;
      
      panel_Frage.SetActive(false);
      panel_Frage_klein.SetActive(true);
      
      
      //Frage nach oben links
   }
}
