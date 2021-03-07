using UnityEngine;
using UnityEngine.EventSystems;

public class ShopCloseClick : MonoBehaviour, IPointerClickHandler
{
    public delegate void ShopClose();
    public static event ShopClose Start;

    public void OnPointerClick(PointerEventData eventData)
    {
        Start?.Invoke();
    }
}
