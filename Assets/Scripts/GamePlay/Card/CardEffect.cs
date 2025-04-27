using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace GamePlay
{
    public class CardEffect : MonoBehaviour, ICardController
    {
        [Header("卡片跟随速度")] public float cardFllowSpeed;

        [Header("动画曲线")] public AnimationCurve pointerIn;
        [FormerlySerializedAs("pointerout")] public AnimationCurve pointerOut;
        public Tweener _pointerInTweener;
        public Tweener _pointerOutTweener;


        private Canvas _dragCanvas;
        private RectTransform _dragCard;
        private RectTransform _dragGroup;
        private RectTransform _cardFace;
        private CardController _controller;

        private Vector3 _originScale;
        private Vector3 _originEulerAngles;
        [HideInInspector] public int _originSortingOrder;

        private Vector3 _lastPosition;
        private Vector3 _moveDelta;
        private Vector3 _velocity;
        private Vector3 _targetEulerAngles;

        private Vector3 _prePos;
        private Vector3 _deltaPos;

        private void Awake()
        {
            _controller = GetComponent<CardController>();
            _dragGroup = transform.Find("SpriteGroup").GetComponent<RectTransform>();
            _dragCard = _dragGroup.Find("Sprite").GetComponent<RectTransform>();
            _cardFace = _dragGroup.Find("CardFace").GetComponent<RectTransform>();
            _dragCanvas = _controller._cardFace.GetComponent<Canvas>();

            _originSortingOrder = _dragCanvas.sortingOrder;
        }

        private void Start()
        {
            _controller.OnSelectedChanged += OnSelected;
            StartDOTween();
        }

        private void Update()
        {
            MoveLerpHandle();

            RotatinoHandle(_deltaPos.x * 0.5f);
        }

        private void RotatinoHandle(float _deltaX)
        {
            _deltaPos = _dragCard.position - _cardFace.position;
            float deltaX = _deltaX;
            _cardFace.eulerAngles = new Vector3(0, 0, -deltaX);
        }

        private void MoveLerpHandle()
        {
            Vector3 startPos = new Vector3(_cardFace.position.x, _cardFace.position.y, 0);
            Vector3 targetPos = new Vector3(_dragCard.position.x, _dragCard.position.y, 0);
            _cardFace.position = Vector3.Lerp(startPos, targetPos, Time.deltaTime * cardFllowSpeed);
        }

        private void OnDestroy()
        {
            _controller.OnSelectedChanged -= OnSelected;
        }

        private void OnSelected()
        {
            if (_controller.isSelected)
            {
                _dragGroup.localPosition = new Vector2(0, 20);
            }
            else
            {
                _dragGroup.localPosition = Vector2.zero;
            }
        }

        public void OnPointerEnterCard(PointerEventData eventData)
        {
            _pointerInTweener.Restart();
        }

        public void OnPointerClickCard(PointerEventData eventData)
        {
        }

        public void OnPointerExitCard(PointerEventData eventData)
        {
            _pointerOutTweener.Restart();
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

        private void StartDOTween()
        {
            _pointerInTweener = _cardFace.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.2f).SetEase(pointerIn);
            _pointerOutTweener = _cardFace.DOScale(new Vector3(1, 1, 1), 0.2f).SetEase(pointerOut);
            _pointerInTweener.Pause();
            _pointerOutTweener.Pause();
            _pointerInTweener.SetAutoKill(false);
            _pointerOutTweener.SetAutoKill(false);
        }
    }
}