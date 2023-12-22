using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCoin : MonoBehaviour
{
    //Adds an audio to game object that can be called later
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
