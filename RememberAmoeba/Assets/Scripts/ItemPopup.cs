using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemPopup : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    [SerializeField] private MemoryFade memoryFade;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip pieceSound;
    [SerializeField] private GameObject bitePrefab; //your black bite mark image
    [SerializeField] private RectTransform biteParent; //where bite marks are placed, usually the popup panel
    [SerializeField] private GameObject eatLabel; //the Eat text that follows the mouse
    [SerializeField] private Vector3 labelOffset = new Vector3(40f, 40f, 0f); //how far the Eat text sits from the mouse

    private AudioSource audioSource;
    private readonly List<GameObject> bites = new List<GameObject>(); //every bite mark spawned
    private bool closeNext;
    private bool finishing;
    private bool hovering; //true while the mouse is over a piece
    private int piecesLeft;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        if (EventSystem.current == null)
            new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
    }

    public void Open()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f;

        closeNext = false;
        finishing = false;
        SetHovering(false);
        ClearBites(); //removes bite marks from last time
        gameObject.SetActive(true);

        PopupPiece[] pieces = GetComponentsInChildren<PopupPiece>(true);
        foreach (PopupPiece piece in pieces) piece.gameObject.SetActive(true);
        piecesLeft = pieces.Length;

        if (player != null) player.enabled = false;
        else Debug.LogWarning("Player slot is empty on ItemPopup");

        if (openSound != null) audioSource.PlayOneShot(openSound);
    }

    public void SetHovering(bool value)
    {
        hovering = value;
        if (eatLabel != null) eatLabel.SetActive(value); //shows or hides the Eat text
    }

    public void SpawnBite(PointerEventData eventData)
    {
        if (bitePrefab == null || biteParent == null) return;

        Vector2 local;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(biteParent, eventData.position, eventData.pressEventCamera, out local); //click spot on the panel

        GameObject bite = Instantiate(bitePrefab, biteParent);
        bite.transform.localPosition = local; //puts the bite where you clicked
        bite.transform.localRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)); //random angle
        bite.transform.localScale = Vector3.one * Random.Range(0.9f, 1.2f); //random size

        foreach (Graphic graphic in bite.GetComponentsInChildren<Graphic>())
            graphic.raycastTarget = false; //so clicks go through the bite to the pieces

        bites.Add(bite);
    }

    private void ClearBites()
    {
        foreach (GameObject bite in bites)
            if (bite != null) Destroy(bite);

        bites.Clear();
    }

    public void PieceClicked()
    {
        if (pieceSound != null) audioSource.PlayOneShot(pieceSound);

        piecesLeft--;
        if (piecesLeft > 0 || finishing) return;
        finishing = true;

        if (memoryFade != null)
        {
            memoryFade.Play(this);
        }
        else
        {
            ReturnControl();
            Hide();
        }
    }

    public void Hide()
    {
        SetHovering(false);
        gameObject.SetActive(false);
    }

    public void ReturnControl()
    {
        if (player != null) player.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (!finishing && Input.GetKeyDown(KeyCode.Escape)) closeNext = true;

        if (hovering && eatLabel != null)
        {
            eatLabel.transform.position = Input.mousePosition + labelOffset; //Eat text follows the mouse
            eatLabel.transform.SetAsLastSibling(); //keeps it above the bite marks
        }
    }

    private void LateUpdate()
    {
        if (!closeNext) return;
        closeNext = false;
        ReturnControl();
        Hide();
    }
}