
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private ItemPopup popup; // attach panel
    [SerializeField] private Transform player; //player tracker
    [SerializeField] private GameObject prompt; // this is where u can see press e
    [SerializeField] private AudioClip pickupSound; //audio 
    [SerializeField] private float pickupRange = 3f; //range in which u can pickup and see e

    private void Update()
    {
        bool inRange = Vector3.Distance(transform.position, player.position) <= pickupRange; // if the player is in range it can pickup

        if (prompt != null) prompt.SetActive(inRange);

        if (inRange && Input.GetKeyDown(KeyCode.E)) //press e 
        {
            if (prompt != null) prompt.SetActive(false);
            if (pickupSound != null) AudioSource.PlayClipAtPoint(pickupSound, transform.position); //sounds plays
            popup.Open(); //popup opens
            gameObject.SetActive(false); //makes panel disappear
        }
    }
}