using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCoin : MonoBehaviour
{
    public AudioSource collectSound;
    public int pickupScore;
   private void OnTriggerEnter(Collider other)
   {
            collectSound.Play();
            ScoringSystem.totalScore += pickupScore;  
             Destroy(gameObject);  
   }
}
