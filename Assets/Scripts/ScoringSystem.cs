using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class ScoringSystem : MonoBehaviour
{
    public GameObject Score;
    public static int totalScore;
   void Update()
   {
            Score.GetComponent<TMPro.TextMeshProUGUI>().text = "Score " + totalScore;    
   }
}
