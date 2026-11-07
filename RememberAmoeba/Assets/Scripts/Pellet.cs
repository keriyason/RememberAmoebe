using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellette : MonoBehaviour
{
    [SerializeField] private AudioClip pickupSound;  // munch sound 

    private static AudioSource sharedSource; // audio source for instant sound trigger

    private void Start()
    {
        if (pickupSound != null) pickupSound.LoadAudioData(); // no delay on sound it will load right away

        if (sharedSource == null) //makes speaker if not exist
        {
            sharedSource = new GameObject("PelletAudio").AddComponent<AudioSource>();
            sharedSource.playOnAwake = false;
        }
    }

    private void OnTriggerEnter(Collider other) //collider trigger
    {
        if (!other.CompareTag("Player")) return; //checks player tag

        if (pickupSound != null) sharedSource.PlayOneShot(pickupSound); //plays sound

        Destroy(gameObject); //removes pellet
    }
}
