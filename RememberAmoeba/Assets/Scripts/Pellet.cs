using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellette : MonoBehaviour
{
    [SerializeField] private AudioClip pickupSound; //pellet sound

    private static AudioSource sharedSource;

    private void Start()
    {
        if (pickupSound != null) pickupSound.LoadAudioData(); //loads sound asap

        if (sharedSource == null)
        {
            sharedSource = new GameObject("PelletAudio").AddComponent<AudioSource>();
            sharedSource.playOnAwake = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return; //player tag

        // Fill the boost bar
        if (BoostBar.Instance != null) BoostBar.Instance.AddBoost();         // Fill the boost bar


        if (pickupSound != null) sharedSource.PlayOneShot(pickupSound);        // Play the sound right away


        Destroy(gameObject); //pellet die
    }
}
