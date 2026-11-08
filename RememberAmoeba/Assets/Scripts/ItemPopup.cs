using UnityEngine;
using UnityEngine.EventSystems;

public class ItemPopup : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    [SerializeField] private MemoryFade memoryFade;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip pieceSound;

    private AudioSource audioSource;
    private bool closeNext;
    private bool finishing;
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
        gameObject.SetActive(true);

        PopupPiece[] pieces = GetComponentsInChildren<PopupPiece>(true);
        foreach (PopupPiece piece in pieces) piece.gameObject.SetActive(true);
        piecesLeft = pieces.Length;

        if (player != null) player.enabled = false;
        else Debug.LogWarning("Player slot is empty on ItemPopup");

        if (openSound != null) audioSource.PlayOneShot(openSound);
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
    }

    private void LateUpdate()
    {
        if (!closeNext) return;
        closeNext = false;
        ReturnControl();
        Hide();
    }
}