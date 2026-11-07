using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public static FloorManager Instance;

    [SerializeField] private GameObject[] floors;
    [SerializeField] private int itemsPerFloor = 2;

    private int itemsCollected;
    private int currentFloor = -1;

    private void Awake()
    {
        Instance = this;

        foreach (GameObject floor in floors) floor.SetActive(false);
    }

    public void ItemCollected()
    {
        itemsCollected++;

        int target = Mathf.Min(itemsCollected / itemsPerFloor, floors.Length) - 1;
        if (target <= currentFloor) return;

        if (currentFloor >= 0) floors[currentFloor].SetActive(false);
        currentFloor = target;
        floors[currentFloor].SetActive(true);
    }
}
