using UnityEngine;
using UnityEngine.EventSystems;

public class PopupPiece : MonoBehaviour, IPointerClickHandler //call script automatically when clicked
{
    public void OnPointerClick(PointerEventData eventData) //runs when piece is clicked
    {
        ItemPopup popup = GetComponentInParent<ItemPopup>(); //find popup that piece is attached to
        gameObject.SetActive(false); //makes piece disappear
        popup.PieceClicked(); //calls to popup removed
    }
}