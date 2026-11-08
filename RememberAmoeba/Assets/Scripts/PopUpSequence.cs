using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupSequence : MonoBehaviour
{
    public static PopupSequence Instance;

    [SerializeField] private ItemPopup[] popups;
    [SerializeField] private GameObject prompt;

    private int next;
    private ItemPopup current;

    public GameObject Prompt => prompt;

    private void Awake()
    {
        Instance = this;

        foreach (ItemPopup popup in popups)
            if (popup != null) popup.gameObject.SetActive(false);

        if (prompt != null) prompt.SetActive(false);
    }

    public bool OpenNext()
    {
        if (current != null && current.gameObject.activeSelf) return false;

        if (next < popups.Length && popups[next] != null)
        {
            current = popups[next];
            next++;
            current.Open();
        }

        return true;
    }
}