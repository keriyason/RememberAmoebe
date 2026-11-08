using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupSequence : MonoBehaviour
{
    public static PopupSequence Instance;

    [SerializeField] private ItemPopup[] popups; //popups in the order they should appear

    private int next; //which popup comes next
    private ItemPopup current; //the popup that is open right now

    private void Awake()
    {
        Instance = this;

        foreach (ItemPopup popup in popups)
            if (popup != null) popup.gameObject.SetActive(false); //hides every popup at the start
    }

    public bool OpenNext()
    {
        if (current != null && current.gameObject.activeSelf) return false; //a popup is already open

        if (next < popups.Length && popups[next] != null)
        {
            current = popups[next];
            next++;
            current.Open(); //opens the next popup in the list
        }

        return true;
    }
}
