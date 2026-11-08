using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupSequence : MonoBehaviour
{
    public static PopupSequence Instance;

    [SerializeField] private ItemPopup[] popups; //popups in the order they appear
    [SerializeField] private GameObject prompt; //press e text shared by every item
    [SerializeField] private string nextScene; //scene to load after every popup is done

    private int next;
    private int finished;
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
        if (current != null && current.gameObject.activeSelf) return false; //a popup is already open

        if (next < popups.Length && popups[next] != null)
        {
            current = popups[next];
            next++;
            current.Open();
        }

        return true;
    }

    public bool PopupFinished()
    {
        finished++;

        if (finished >= popups.Length && !string.IsNullOrEmpty(nextScene))
        {
            SceneTransition.Load(nextScene); //every popup is done
            return true;
        }

        return false;
    }
}