using UnityEngine;
using UnityEngine.EventSystems;

public class PopupPiece : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        ItemPopup popup = GetComponentInParent<ItemPopup>();
        gameObject.SetActive(false);
        popup.PieceClicked();
    }
}