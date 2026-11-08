using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : MonoBehaviour
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

        if (BoostBar.Instance != null) BoostBar.Instance.AddBoost(); //fill the boost bar

        if (PlayerAnimation.Instance != null) PlayerAnimation.Instance.PelletEaten(); //counts toward the eating animation

        if (pickupSound != null) sharedSource.PlayOneShot(pickupSound); //play the sound right away

        Destroy(gameObject); //pellet die
    }
}
