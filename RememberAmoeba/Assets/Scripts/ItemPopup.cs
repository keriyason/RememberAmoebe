using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPopup : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    [SerializeField] private MemoryFade memoryFade;
    [SerializeField] private AudioClip openSound; //plays when the popup opens
    [SerializeField] private AudioClip pieceSound; //plays on every click
    [SerializeField] private GameObject bitePrefab; //your black bite mark image
    [SerializeField] private RectTransform biteParent; //empty container that fills the panel, bites spawn inside it
    [SerializeField] private GameObject hintText; //the "Click to eat" text
    [SerializeField] private float hintHoldTime = 1.5f; //seconds the text stays before fading
    [SerializeField] private float hintFadeTime = 1f; //seconds the text takes to fade out
    [SerializeField] private int clicksToFinish = 5; //number of clicks until satisfied

    private AudioSource audioSource;
    private readonly List<GameObject> bites = new List<GameObject>(); //every bite mark spawned
    private bool closeNext;
    private bool finishing;
    private bool opened; //true once a pickup has opened the popup
    private bool returned; //true once the popup has been closed
    private int clicksLeft;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        FindReferences();

        if (!opened) gameObject.SetActive(false); //hides the popup at the start of the game
    }

    private void FindReferences()
    {
        if (player == null)
        {
            GameObject found = GameObject.FindWithTag("Player"); //finds the player on its own
            if (found != null) player = found.GetComponent<PlayerMovement>();
        }

        if (memoryFade == null) memoryFade = FindFirstObjectByType<MemoryFade>(); //finds the memory erased screen on its own
    }

    public void Open()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f;

        FindReferences();
        opened = true; //stops Awake from hiding it again
        closeNext = false;
        finishing = false;
        returned = false;
        clicksLeft = Mathf.Max(1, clicksToFinish); //resets the click counter
        ClearBites(); //removes bite marks from last time
        gameObject.SetActive(true);

        if (player != null) player.enabled = false;
        else Debug.LogWarning("Player slot is empty on ItemPopup");

        if (openSound != null) audioSource.PlayOneShot(openSound);

        StopAllCoroutines();
        if (hintText != null) StartCoroutine(ShowHint()); //Click to eat text appears then fades
    }

    private IEnumerator ShowHint()
    {
        CanvasGroup group = hintText.GetComponent<CanvasGroup>();
        if (group == null) group = hintText.AddComponent<CanvasGroup>();

        hintText.SetActive(true);
        group.alpha = 1f;

        yield return new WaitForSecondsRealtime(hintHoldTime); //text stays for a moment

        float t = 0f;
        while (t < hintFadeTime)
        {
            t += Time.unscaledDeltaTime;
            group.alpha = Mathf.Lerp(1f, 0f, t / hintFadeTime); //fades out
            yield return null;
        }

        group.alpha = 0f;
        hintText.SetActive(false);
    }

    private void Update()
    {
        if (finishing) return;

        if (Input.GetKeyDown(KeyCode.Escape)) closeNext = true; //esc closes the popup

        if (Input.GetMouseButtonDown(0)) Eat(Input.mousePosition); //click anywhere on the screen
    }

    private void Eat(Vector2 screenPoint)
    {
        SpawnBite(screenPoint); //bite mark appears where you clicked
        if (pieceSound != null) audioSource.PlayOneShot(pieceSound);

        clicksLeft--;
        if (clicksLeft > 0) return; //not satisfied yet
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

    private void SpawnBite(Vector2 screenPoint)
    {
        if (bitePrefab == null || biteParent == null) return;

        Canvas canvas = GetComponentInParent<Canvas>();
        Camera cam = canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay ? canvas.worldCamera : null;

        Vector3 world;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(biteParent, screenPoint, cam, out world); //click spot in the UI

        GameObject bite = Instantiate(bitePrefab, biteParent);
        bite.transform.position = world;
        bite.transform.localRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)); //random angle
        bite.transform.localScale = Vector3.one * Random.Range(0.9f, 1.2f); //random size

        foreach (Graphic graphic in bite.GetComponentsInChildren<Graphic>())
            graphic.raycastTarget = false;

        bites.Add(bite);
    }

    private void ClearBites()
    {
        foreach (GameObject bite in bites)
            if (bite != null) Destroy(bite);

        bites.Clear();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void ReturnControl()
    {
        if (returned) return; //already closed once
        returned = true;

        if (PopupSequence.Instance != null && PopupSequence.Instance.PopupFinished()) return; //last popup, the scene is changing so the player stays frozen

        if (player != null) player.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        if (!closeNext) return;
        closeNext = false;
        ReturnControl();
        Hide();
    }
}