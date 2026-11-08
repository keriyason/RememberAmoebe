using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpSequence : MonoBehaviour
{
    public static PopUpSequence Instance;

    [SerializeField] private ItemPopup[] popups; //Order of pop up

    private int next; //which popup comes next
    private ItemPopup current; // tracks the current pop up

    private void Awake()
    {
        Instance = this;

        foreach (ItemPopup popup in popups)
            if (popup != null) popup.gameObject.SetActive(false); //hides every popup at the start
    }

    public bool OpenNext()
    {
        if (current != null && current.gameObject.activeSelf) return false; // ta popup is already open

        if (next < popups.Length && popups[next] != null)
        {
            current = popups[next];
            next++;
            current.Open(); //opens the next popup in the list
        }

        return true;
    }
}
