using UnityEngine;

public class ItemPopup : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;

    private bool closeNext;

    public void Open()
    {
        closeNext = false;
        gameObject.SetActive(true);
        player.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) closeNext = true;
    }

    private void LateUpdate()
    {
        if (!closeNext) return;
        closeNext = false;
        player.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameObject.SetActive(false);
    }
}