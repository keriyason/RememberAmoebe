using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public static PlayerAnimation Instance;

    [SerializeField] private Animator animator; //the Animator on this object
    [SerializeField] private Transform player; //the player capsule, leave empty to use the parent
    [SerializeField] private float movingThreshold = 0.1f; //how fast you must move to count as walking
    [SerializeField] private int pelletsPerEat = 10; //pellets you must eat before the eating animation plays
    [SerializeField] private bool repeat = true; //untick to only play it once

    private Vector3 lastPosition;
    private int pelletCount; //pellets eaten since the last eating animation
    private bool played; //true after the animation has played once

    private void Awake()
    {
        Instance = this;

        if (animator == null) animator = GetComponent<Animator>();
        if (player == null) player = transform.parent;

        lastPosition = player.position;
    }

    private void Update()
    {
        Vector3 moved = player.position - lastPosition; //how far the player moved this frame
        moved.y = 0f; //ignores jumping and falling
        lastPosition = player.position;

        float speed = moved.magnitude / Time.deltaTime;
        animator.SetBool("IsMoving", speed > movingThreshold); //walk when moving, idle when not
    }

    public void PelletEaten()
    {
        if (played && !repeat) return; //already played and repeat is off

        pelletCount++;
        if (pelletCount < pelletsPerEat) return; //not enough pellets yet

        pelletCount = 0; //start counting again
        played = true;
        PlayEat();
    }

    public void PlayEat()
    {
        animator.SetTrigger("Eat"); //plays the eating animation once
    }
}