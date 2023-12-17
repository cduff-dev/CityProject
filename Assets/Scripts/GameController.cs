using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
   public GameObject Score;
   public GameObject WinText;
    public static int totalScore;
    public static bool Level1Finished = false;
    public static bool Level2Finished = false;

   void Update()
   {
       Score.GetComponent<TMPro.TextMeshProUGUI>().text = "Score: " + totalScore;  
       checkIfWon();  
   }

    //Only called by button press
   public void returnToMainMenu()
   {
        //resets Score to 0 as score persists between scenes.
        totalScore = 0;
        //Resets Level Progress
        Level1Finished = false;
        Level2Finished = false;
        //loads Main Menu
        SceneManager.LoadScene(0);
   }

   void checkIfWon()
   {
        if(totalScore == 40)
        {
            if(Level1Finished == false)
            {
                Level1Finished = true;
                SceneManager.LoadScene(3);
                
            }
            
        }

        if(totalScore == 80)
        {
            if(Level1Finished == true && Level2Finished == false)
            {
                Level2Finished = true;
                SceneManager.LoadScene(4);
                
            }
        }
        
         if(totalScore == 120)
        {
            WinText.SetActive(true);
        }
   }
}
