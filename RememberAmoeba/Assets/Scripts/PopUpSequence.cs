using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupSequence : MonoBehaviour
{
    public static PopupSequence Instance;

[SerializeField] private ItemPopup[] popups;
[SerializeField] private GameObject prompt;
private string nextScene = "EndingStory"; //scene to load after every popup has played

private int next;
private int total; //how many popups are in the list
private int finished; //how many popups have been closed
private ItemPopup current;

public GameObject Prompt => prompt;

private void Awake()
{
    Instance = this;

    foreach (ItemPopup popup in popups)
    {
        if (popup == null) continue;
        popup.gameObject.SetActive(false);
        total++;
    }

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

public bool PopupFinished()
{
    finished++;

    if (finished >= total && !string.IsNullOrEmpty(nextScene))
    {
        SceneTransition.Load(nextScene); //every popup has played, so change scene
        return true;
    }

    return false;
}
}