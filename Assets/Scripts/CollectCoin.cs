using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCoin : MonoBehaviour
{
    public AudioSource collectSound;
    public int pickupScore;

    //Collision functionality. play sound, add to player score, destroy the coin.
   private void OnTriggerEnter(Collider other)
   {
            collectSound.Play();
            GameController.totalScore += pickupScore;  
             Destroy(gameObject);  
   }
}
