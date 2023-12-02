using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
   public GameObject Score;
   public GameObject WinText;
    public static int totalScore;

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
        //loads Main Menu
        SceneManager.LoadScene(0);
   }

   void checkIfWon()
   {
        if(totalScore == 40)
        {
            WinText.SetActive(true);
        }
   }
}
