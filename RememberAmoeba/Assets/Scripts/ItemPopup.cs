using UnityEngine;

public class ItemPopup : MonoBehaviour
{
    [SerializeField] private PlayerMovement player; //calls to player
    [SerializeField] private MemoryFade memoryFade; //calls to memory fade
    [SerializeField] private AudioClip openSound; //attach audio
    [SerializeField] private AudioClip pieceSound; //piece sound

    private AudioSource audioSource; //calls to audio source
    private bool closeNext;
    private bool finishing;
    private int piecesLeft;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void Open()
    {
        closeNext = false; //start off
        finishing = false;
        gameObject.SetActive(true);
        piecesLeft = GetComponentsInChildren<PopupPiece>().Length;
        player.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (openSound != null) audioSource.PlayOneShot(openSound);
    }

    public void PieceClicked()
    {
        if (pieceSound != null) audioSource.PlayOneShot(pieceSound); //plays sound

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
        player.enabled = true;
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