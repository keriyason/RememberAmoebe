
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private ItemPopup popup; //pop up ref
    [SerializeField] private Transform player; //player tracker
    [SerializeField] private GameObject prompt; // press e
    [SerializeField] private AudioClip pickupSound; //audio
    [SerializeField] private float pickupRange = 3f; //range in which u can pickup and see e

    private void Update()
    {
        bool inRange = Vector3.Distance(transform.position, player.position) <= pickupRange; //true when the player is close enough

        if (prompt != null) prompt.SetActive(inRange);

        if (inRange && Input.GetKeyDown(KeyCode.E)) //press e
        {
            if (PopupSequence.Instance != null)
            {
                if (!PopupSequence.Instance.OpenNext()) return; //stops if a popup is already open
            }
            else if (popup != null)
            {
                popup.Open(); //no sequence, so open this item's own popup
            }

            if (prompt != null) prompt.SetActive(false);
            if (pickupSound != null) AudioSource.PlayClipAtPoint(pickupSound, transform.position); //sound plays
            gameObject.SetActive(false); //makes the item disappear
            if (FloorManager.Instance != null) FloorManager.Instance.ItemCollected(); //counts toward the next floor version
        }
    }
}