using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;


namespace GamePlay
{
    public class CardController : MonoBehaviour, ICardController
    {
        public event UnityAction OnSelectedChanged;
        public RectTransform _dragCard { get; private set; }
        public RectTransform _cardFace { get; private set; }
        [HideInInspector]public CardGroup cardGroup;
        
        private CardView _cardView;
        private CardEffect _cardEffect;
        private Vector2 _beginDragOffset;
        private Canvas _cardFaceCanvas;

        private bool _isSelected;
        private bool _isPressDown;
        private bool _isDrag;
        public bool isDrag => _isDrag;
        

        public bool isSelected
        {
            get { return _isSelected; }
            private set
            {
                _isSelected = value;
                OnSelectedChanged?.Invoke();
            }
        }

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            _cardFace = transform.Find("SpriteGroup").Find("CardFace").GetComponent<RectTransform>();
            _dragCard = transform.Find("SpriteGroup").Find("Sprite").GetComponent<RectTransform>();
            _cardFaceCanvas = _cardFace.GetComponent<Canvas>();
            _cardView = GetComponent<CardView>();
            _cardEffect = GetComponent<CardEffect>();
        }

        public void OnPointerEnterCard(PointerEventData eventData)
        {
            _cardEffect.OnPointerEnterCard(eventData);
        }

        public void OnPointerClickCard(PointerEventData eventData)
        {
            if (!_isDrag)
            {
                isSelected = !isSelected;
            }

            _cardEffect.OnPointerClickCard(eventData);
        }

        public void OnPointerExitCard(PointerEventData eventData)
        {
            _cardEffect.OnPointerExitCard(eventData);
        }

        public void OnBeginDragCard(PointerEventData eventData)
        {
            _isDrag = true;
            _cardEffect.OnBeginDragCard(eventData);

            cardGroup.SetDragCard(this);
            _beginDragOffset = new Vector3(_dragCard.position.x, _dragCard.position.y, 0) -
                               new Vector3(eventData.position.x, eventData.position.y, 0);
        }

        public void OnDragCard(PointerEventData eventData)
        {
            _cardEffect.OnDragCard(eventData);

            cardGroup.CardCheckOnDrag();
            _dragCard.position = eventData.position + _beginDragOffset;
        }

        public void OnEndDragCard(PointerEventData eventData)
        {
            _isDrag = false;
            _cardEffect.OnEndDragCard(eventData);

            cardGroup.SetDragCard(null);

            float bufferDis = (_dragCard.localPosition - Vector3.zero).magnitude;
            float time = bufferDis < 10 ? 0 : 0.2f;

            _dragCard.DOLocalMove(Vector3.zero, time).SetEase(Ease.OutBack);
        }

        public void OnPointerDownCard(PointerEventData eventData)
        {
            _isPressDown = true;
            _cardEffect.OnPointerDownCard(eventData);
        }

        public void OnPointerUpCard(PointerEventData eventData)
        {
            _isPressDown = false;
            _cardEffect.OnPointerUpCard(eventData);
        }

        /// <summary>
        /// 在重新排序卡槽的卡牌时刷新canvas排序
        /// </summary>
        /// <param name="sortOrder">要设置的序号</param>
        public void SetSortOrder(int sortOrder, bool isSetOrigin = false)
        {
            if (isSetOrigin)
            {
                _cardEffect._originSortingOrder = sortOrder;
            }
            else
            {
                if (_cardFaceCanvas == null)
                    _cardFaceCanvas = _cardFace.AddComponent<Canvas>();
                _cardFaceCanvas.sortingOrder = sortOrder;
            }
        }

        public void SetCardFaceParent(RectTransform transform)
        {
            _cardFace.SetParent(transform);
            _cardFace.position = _dragCard.position;
        }
    }
}