using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFloat : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 60f; //degrees per second it spins
    [SerializeField] private float bobHeight = 0.25f; //how far it moves up and down
    [SerializeField] private float bobSpeed = 2f; //how fast it bobs
    [SerializeField, Range(0f, 1f)] private float variation = 0.3f; //how different each copy is, 0 = all identical
    [SerializeField] private bool randomDirection = true; //some copies spin the other way

    private Vector3 startPos;
    private float phase; //random point in the bob so copies don't move together

    private void Start()
    {
        startPos = transform.localPosition; //remembers where it was placed

        rotateSpeed *= Random.Range(1f - variation, 1f + variation); //random spin speed
        bobHeight *= Random.Range(1f - variation, 1f + variation); //random bob height
        bobSpeed *= Random.Range(1f - variation, 1f + variation); //random bob speed
        phase = Random.Range(0f, Mathf.PI * 2f); //random start point

        if (randomDirection && Random.value > 0.5f) rotateSpeed = -rotateSpeed; //random spin direction
    }

    private void Update()
    {
        transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f, Space.World); //spins around the up axis

        float offset = Mathf.Sin(Time.time * bobSpeed + phase) * bobHeight; //up and down movement
        transform.localPosition = startPos + new Vector3(0f, offset, 0f); //applies the bob
    }
}