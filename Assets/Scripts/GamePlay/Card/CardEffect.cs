using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GamePlay
{
    public class CardEffect : MonoBehaviour, ICardController
    {
        public float _cardFllowSpeed;

        private Canvas _dragCanvas;
        private RectTransform _sprite;
        private RectTransform _spriteGroup;
        private RectTransform _cardFace;
        private CardController _controller;
        public int _originSortingOrder;
        
        private Vector3 _lastPosition;
        private Vector3 _moveDelta;
        private Vector3 _velocity;
        private Vector3 _targetEulerAngles;

        private Vector3 _prePos;
        private Vector3 _deltaPos;

        private void Awake()
        {
            _controller = GetComponent<CardController>();
            _spriteGroup = transform.Find("SpriteGroup").GetComponent<RectTransform>();
            _sprite = _spriteGroup.Find("Sprite").GetComponent<RectTransform>();
            _cardFace = _controller._cardFace;
            _dragCanvas = _controller._cardFace.GetComponent<Canvas>();

            _originSortingOrder = _dragCanvas.sortingOrder;
        }

        private void Start()
        {
            _controller.OnSelectedChanged += OnSelected;
        }

        private void Update()
        {
            MoveLerpHandle();
            
            RotatinoHandle(_deltaPos.x * 0.5f);
        }

        private void RotatinoHandle(float _deltaX)
        {
            _deltaPos = _sprite.position - _cardFace.position;
            float deltaX = _deltaX;
            _cardFace.eulerAngles = new Vector3(0, 0, -deltaX);
        }

        private void MoveLerpHandle()
        {
            Vector3 startPos = new Vector3(_cardFace.position.x, _cardFace.position.y, 0);
            Vector3 targetPos = new Vector3(_sprite.position.x, _sprite.position.y, 0);
            _cardFace.position = Vector3.Lerp(startPos, targetPos, Time.deltaTime * _cardFllowSpeed);
        }

        private void OnDestroy()
        {
            _controller.OnSelectedChanged -= OnSelected;
        }

        private void OnSelected()
        {
            if (_controller.isSelected)
            {
                _spriteGroup.localPosition = new Vector2(0, 20);
            }
            else
            {
                _spriteGroup.localPosition = Vector2.zero;
            }
        }

        public void OnPointerEnterCard(PointerEventData eventData)
        {
        }

        public void OnPointerClickCard(PointerEventData eventData)
        {
        }

        public void OnPointerExitCard(PointerEventData eventData)
        {
        }

        public void OnBeginDragCard(PointerEventData eventData)
        {
            _originSortingOrder = _dragCanvas.sortingOrder;
            _dragCanvas.sortingOrder = 100;
        }

        public void OnDragCard(PointerEventData eventData)
        {

        }

        public void OnEndDragCard(PointerEventData eventData)
        {
            _dragCanvas.sortingOrder = _originSortingOrder;
        }

        public void OnPointerDownCard(PointerEventData eventData)
        {
            
        }

        public void OnPointerUpCard(PointerEventData eventData)
        {

        }
    }
}