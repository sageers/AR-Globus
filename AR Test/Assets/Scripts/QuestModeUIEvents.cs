using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using Vuforia;
using Button = UnityEngine.UI.Button;
using Random = System.Random;

public class QuestModeUIEvents : MonoBehaviour
{

   public GameObject txt_Frage;
   public GameObject txt_Frage_klein;
   public GameObject panel_Frage;
   public GameObject panel_Frage_klein;
   public Button btn_Frage_weiter;

   public TextAsset questQuestion;
   
   private bool frage_weiter;

   
   protected FileInfo fileQuestions = null;
   protected StreamReader reader = null;
   protected string QuestionText = " ";
   private static string solution = "";
   public int AnzahlmöglicherFragen;
   private bool coroutineRunning = true;
   
   
   void Awake () {
      
      
      fileQuestions = new FileInfo (@"Assets\Resources\FragenQuestModus.txt");
      //print(fileQuestions);
      reader = fileQuestions.OpenText();
      
   }
   
   void Start () {
      
      
      fileQuestions = new FileInfo (@"Assets\Resources\FragenQuestModus.txt");
      //print(fileQuestions);
      reader = fileQuestions.OpenText();
      
   }


   public void stopQuestMode()
   {
      panel_Frage.SetActive(false);
      panel_Frage_klein.SetActive(false);
      btn_Frage_weiter.gameObject.SetActive(false);
   }
   
   public void toggleFrage()
   {
      frage_weiter = !frage_weiter;
   }

   public void newQ()
   {
      coroutineRunning = true;
      //StopAllCoroutines();
      StartCoroutine(showNewQuestion());
   }

   public void nextQ()
   {
      coroutineRunning = true;
      StartCoroutine(nextQuestion());
   }

   public string getSolution()
   {
       return solution;
   }

   public IEnumerator showNewQuestion()
   {
      
      while (coroutineRunning)
      {
         
         panel_Frage_klein.SetActive(false);
         btn_Frage_weiter.gameObject.SetActive(false);
         
         var rand = UnityEngine.Random.Range(1, AnzahlmöglicherFragen + 1);
         print(rand);
         QuestionText = chooseQuestion(rand);
         
         panel_Frage.SetActive(true);
         txt_Frage.GetComponent<TextMeshProUGUI>().text = QuestionText;
         txt_Frage_klein.GetComponent<TextMeshProUGUI>().text = QuestionText;
         
         
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
         btn_Frage_weiter.gameObject.SetActive(true);
         frage_weiter = false;
         
         panel_Frage.SetActive(false);
         panel_Frage_klein.SetActive(true);

         coroutineRunning = false;
         yield return null;
      }
      
      
      
      //Frage nach oben links
   }

   public IEnumerator nextQuestion()
   {
      while (coroutineRunning)
      {
         
         coroutineRunning = false;
         panel_Frage_klein.SetActive(false);
         btn_Frage_weiter.gameObject.SetActive(false);
         
         var rand = UnityEngine.Random.Range(1, AnzahlmöglicherFragen + 1);
         print(rand);
         QuestionText = chooseQuestion(rand);
               
         panel_Frage.SetActive(true);
         txt_Frage.GetComponent<TextMeshProUGUI>().text = QuestionText;
         txt_Frage_klein.GetComponent<TextMeshProUGUI>().text = QuestionText;
         
               
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
         btn_Frage_weiter.gameObject.SetActive(true);
         frage_weiter = false;
               
         panel_Frage.SetActive(false);
         panel_Frage_klein.SetActive(true);

         
         yield return null;
      }
   }
   
   private String chooseQuestion(int number)
   {
      
      var textTemp = "";
      
      for (int i = 0; i < number; i++)
      {
         
         textTemp = reader.ReadLine();
         solution = textTemp.Substring(textTemp.IndexOf("?") + 2);
         textTemp = textTemp.Remove(textTemp.IndexOf("?") + 1);
         
      }
         reader.Close();
         reader = fileQuestions.OpenText();
         //QuestionText = textTemp;
        
      
      return textTemp;
   }
}
