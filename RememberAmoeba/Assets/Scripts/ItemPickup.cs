using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private ItemPopup popup;

    private bool inRange;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) inRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) inRange = false;
    }

    private void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.E))
        {
            popup.Open();
            gameObject.SetActive(false);
        }
    }
}