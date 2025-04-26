using UnityEngine.EventSystems;

namespace GamePlay
{
    public interface ICardController
    {
        void OnPointerEnterCard(PointerEventData eventData);
        void OnPointerClickCard(PointerEventData eventData);
        void OnPointerExitCard(PointerEventData eventData);
        void OnBeginDragCard(PointerEventData eventData);
        void OnDragCard(PointerEventData eventData);
        void OnEndDragCard(PointerEventData eventData);
        void OnPointerDownCard(PointerEventData eventData);
        void OnPointerUpCard(PointerEventData eventData);
    }
}