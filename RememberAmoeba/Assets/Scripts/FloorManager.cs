using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public static FloorManager Instance;

    [SerializeField] private GameObject startFloor; //the normal floor you start on
    [SerializeField] private GameObject[] floors; //floors in order, first to last
    [SerializeField] private int itemsPerFloor = 2; //items needed for each new floor

    private int itemsCollected;
    private int currentFloor = -1; //-1 means no colored floor yet

    private void Awake()
    {
        Instance = this;

        foreach (GameObject floor in floors)
            if (floor != null) floor.SetActive(false); //hides every floor at the start
    }

    public void ItemCollected()
    {
        itemsCollected++;
        Debug.Log("Items collected: " + itemsCollected);

        int perFloor = Mathf.Max(1, itemsPerFloor); //stops divide by zero
        int target = Mathf.Min(itemsCollected / perFloor, floors.Length) - 1; //which floor we should be on
        if (target <= currentFloor) return;

        if (startFloor != null) startFloor.SetActive(false); //hides the start floor
        if (currentFloor >= 0 && floors[currentFloor] != null) floors[currentFloor].SetActive(false); //hides the old floor
        currentFloor = target;

        if (floors[currentFloor] != null) floors[currentFloor].SetActive(true); //shows the new floor
        else Debug.LogWarning("Floor slot " + currentFloor + " is empty on FloorManager");
    }
}