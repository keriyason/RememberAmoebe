using UnityEngine;
using UnityEngine.EventSystems;

public class PopupPiece : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        ItemPopup popup = GetComponentInParent<ItemPopup>();
        if (popup != null) popup.SetHovering(true); //shows the Eat text
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ItemPopup popup = GetComponentInParent<ItemPopup>();
        if (popup != null) popup.SetHovering(false); //hides the Eat text
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ItemPopup popup = GetComponentInParent<ItemPopup>(); //the panel this piece belongs to
        popup.SetHovering(false);
        popup.SpawnBite(eventData); //bite mark appears where you clicked
        gameObject.SetActive(false); //the piece disappears
        popup.PieceClicked(); //tells the popup one piece is gone

    }
}