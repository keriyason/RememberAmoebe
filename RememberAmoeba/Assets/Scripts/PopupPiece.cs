using UnityEngine;
using UnityEngine.EventSystems;

public class PopupPiece : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        gameObject.SetActive(false);
    }
}
