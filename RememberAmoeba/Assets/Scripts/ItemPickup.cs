using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private ItemPopup popup;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject prompt;
    [SerializeField] private float pickupRange = 3f;

    private void Update()
    {
        bool inRange = Vector3.Distance(transform.position, player.position) <= pickupRange;

        if (prompt != null) prompt.SetActive(inRange);

        if (inRange && Input.GetKeyDown(KeyCode.E))
        {
            if (prompt != null) prompt.SetActive(false);
            popup.Open();
            gameObject.SetActive(false);
        }
    }
}