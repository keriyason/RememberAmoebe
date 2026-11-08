
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private ItemPopup popup;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject prompt;
    [SerializeField] private bool faceCamera = false;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private float pickupRange = 2f;

    private Collider itemCollider;
    private bool showingPrompt;

    private void Start()
    {
        itemCollider = GetComponentInChildren<Collider>();

        if (itemCollider is MeshCollider meshCollider && !meshCollider.convex)
        {
            Debug.LogWarning("ItemPickup: tick Convex on the Mesh Collider, using the item's center for now", this);
            itemCollider = null;
        }

        if (player == null)
        {
            GameObject found = GameObject.FindWithTag("Player");
            if (found != null) player = found.transform;
            else Debug.LogWarning("ItemPickup: no object is tagged Player", this);
        }

        if (prompt == null && PopupSequence.Instance != null) prompt = PopupSequence.Instance.Prompt;
        if (prompt == null) Debug.LogWarning("ItemPickup: fill the Prompt slot on this item", this);

        if (prompt != null && prompt.transform.IsChildOf(transform)) prompt.SetActive(false);
    }

    private void Update()
    {
        if (player == null) return;

        Vector3 closest = itemCollider != null ? itemCollider.ClosestPoint(player.position) : transform.position;
        bool inRange = Vector3.Distance(closest, player.position) <= pickupRange;

        if (prompt != null)
        {
            if (inRange)
            {
                prompt.SetActive(true);
                showingPrompt = true;
            }
            else if (showingPrompt)
            {
                prompt.SetActive(false);
                showingPrompt = false;
            }

            if (showingPrompt && faceCamera && Camera.main != null)
                prompt.transform.rotation = Camera.main.transform.rotation;
        }

        if (inRange && Input.GetKeyDown(KeyCode.E))
        {
            if (PopupSequence.Instance != null)
            {
                if (!PopupSequence.Instance.OpenNext()) return;
            }
            else if (popup != null)
            {
                popup.Open();
            }

            if (pickupSound != null) AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            gameObject.SetActive(false);
            if (FloorManager.Instance != null) FloorManager.Instance.ItemCollected();
        }
    }

    private void OnDisable()
    {
        if (showingPrompt && prompt != null) prompt.SetActive(false);
        showingPrompt = false;
    }
}