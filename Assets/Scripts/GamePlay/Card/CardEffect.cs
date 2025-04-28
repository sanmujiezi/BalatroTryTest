using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace GamePlay
{
    public class CardEffect : MonoBehaviour, ICardController
    {
        [Header("动画曲线")] public AnimationCurve pointerIn;
        public AnimationCurve pointerOut;
        public List<Tweener> _pointerInTweener;
        public List<Tweener> _pointerOutTweener;
        public List<Tweener> _selectedCardTweener;

        private Canvas _dragCanvas;
        private RectTransform _dragCard;
        private RectTransform _dragGroup;
        private RectTransform _cardFace;
        private RectTransform _cardFaceImage;
        private RectTransform _cardFaceShadow;
        private CardController _controller;

        [HideInInspector] public int _originSortingOrder;
        private Vector3 _originPos;
        private Vector3 _afterPos;

        private float cardFllowSpeed = 25;
        private float shakeAngle = 4;
        private int vibrato = 30;
        private float randomness = 25;
        private float duration = 0.1f;
        private float tiltSpeed = 15;
        public float offsetRoatetionZ;
        public float offsetPostionY;
        private Vector3 _deltaPos;
        private bool isHover;
        private bool isDrag;
        private int savedIndex;


        private void Awake()
        {
            _controller = GetComponent<CardController>();
            _dragGroup = transform.Find("SpriteGroup").GetComponent<RectTransform>();
            _dragCard = _dragGroup.Find("Sprite").GetComponent<RectTransform>();
            _cardFace = _dragGroup.Find("CardFace").GetComponent<RectTransform>();
            _cardFaceImage = _cardFace.Find("ImageGroup").GetComponent<RectTransform>();
            _cardFaceShadow = _cardFace.Find("Shadow").GetComponent<RectTransform>();
            _dragCanvas = _controller._cardFace.GetComponent<Canvas>();

            _originSortingOrder = _dragCanvas.sortingOrder;
            _originPos = _dragGroup.localPosition;
        }

        private void Start()
        {
            _controller.OnSelectedChanged += OnSelected;
            StartDOTween();
        }

        private void Update()
        {
            MoveLerpHandle();
            RotatinoHandle();
            CardFaceShadowHandle();
        }

        private void CardFaceShadowHandle()
        {
            float deltaXWithCenter = _cardFaceShadow.position.x - Screen.width / 2;
            float ratio = 0.03f;
            float x = deltaXWithCenter < 0
                ? Mathf.Max(-20f, deltaXWithCenter * ratio)
                : Mathf.Min(20f, deltaXWithCenter * ratio);
            float y = _cardFaceShadow.localPosition.y;
            float z = _cardFaceShadow.localPosition.z;
            _cardFaceShadow.localPosition = new Vector3(x, y, z);
        }

        private void RotatinoHandle()
        {
            _deltaPos = _dragCard.position - _cardFace.position;
            float roateZ = _deltaPos.x * 0.5f;

            savedIndex = isHover ? 0 : _controller.transform.parent.GetSiblingIndex();
            Vector3 deltaDir = (Input.mousePosition - _dragCard.position).normalized;
            if (isHover)
            {
                Debug.DrawLine(_dragCard.position, Input.mousePosition, Color.red);
            }

            float sine = isHover && !_controller.isDrag ? deltaDir.y * 10f : Mathf.Sin(Time.time + savedIndex) * 10;
            float cosine = isHover && !_controller.isDrag
                ? deltaDir.x * -1 * 10f
                : Mathf.Cos(Time.time + savedIndex) * 10;

            float lerpX = Mathf.LerpAngle(_cardFace.eulerAngles.x, sine, Time.deltaTime * tiltSpeed);
            float lerpY = Mathf.LerpAngle(_cardFace.eulerAngles.y, cosine, Time.deltaTime * tiltSpeed);
            float lerpZ = Mathf.LerpAngle(_cardFace.eulerAngles.z, offsetRoatetionZ + (-roateZ),
                Time.deltaTime * tiltSpeed);

            _cardFace.eulerAngles = new Vector3(lerpX, lerpY, lerpZ);
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
                PlaySelectedCardAnimation();
            }
            else
            {
                _dragGroup.localPosition = Vector2.zero;
                PlaySelectedCardAnimation();
            }
        }

        public void OnPointerEnterCard(PointerEventData eventData)
        {
            isHover = true;
            _dragCard.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            if (!_controller.isDrag)
                PlayPointerInAnimation();
        }

        public void OnPointerClickCard(PointerEventData eventData)
        {
        }

        public void OnPointerExitCard(PointerEventData eventData)
        {
            isHover = false;
            _dragCard.localScale = new Vector3(1f, 1f, 1f);
            if (!_controller.isDrag)
                PlayPointerOutAnimation();
        }

        public void OnBeginDragCard(PointerEventData eventData)
        {
            _originSortingOrder = _dragCanvas.sortingOrder;
            _dragCanvas.sortingOrder = 100;
            _cardFaceShadow.gameObject.SetActive(true);
        }

        public void OnDragCard(PointerEventData eventData)
        {
        }

        public void OnEndDragCard(PointerEventData eventData)
        {
            _cardFaceShadow.gameObject.SetActive(false);
            _dragCanvas.sortingOrder = _originSortingOrder;
            PlayPointerOutAnimation();
        }

        public void OnPointerDownCard(PointerEventData eventData)
        {
        }

        public void OnPointerUpCard(PointerEventData eventData)
        {
        }

        private void StartDOTween()
        {
            AddPointerInList(_cardFace.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.2f).SetEase(pointerIn));
            AddPointerInList(_cardFace
                .DOLocalRotate(new Vector3(_cardFace.eulerAngles.x, _cardFace.eulerAngles.y, _cardFace.eulerAngles.z),
                    0.2f)
                .SetEase(pointerIn));
            AddPointerInList(_cardFace.DOShakeRotation(duration,
                new Vector3(_cardFace.eulerAngles.x, _cardFace.eulerAngles.y,
                    Mathf.Min(_cardFace.eulerAngles.z + shakeAngle, shakeAngle)), vibrato, randomness, true,
                ShakeRandomnessMode.Harmonic));

            AddPointerOutList(_cardFace.DOScale(new Vector3(1, 1, 1), 0.2f).SetEase(pointerOut));
            AddPointerOutList(_cardFace
                .DOLocalRotate(new Vector3(_cardFace.eulerAngles.x, _cardFace.eulerAngles.y, _cardFace.eulerAngles.z),
                    0.2f)
                .SetEase(pointerIn));
            AddPointerOutList(_cardFace.DOShakeRotation(duration,
                new Vector3(_cardFace.eulerAngles.x, _cardFace.eulerAngles.y,
                    Mathf.Min(_cardFace.eulerAngles.z + shakeAngle, shakeAngle)), vibrato, randomness, true,
                ShakeRandomnessMode.Harmonic));

            AddSelectedCardList(_cardFace
                .DOScale(
                    new Vector3(_cardFace.localScale.x + 0.05f, _cardFace.localScale.y + 0.05f,
                        _cardFace.localScale.z + 0.05f), 0.2f).SetEase(Ease.InBounce));
            AddSelectedCardList(_cardFace.DOShakeRotation(duration,
                new Vector3(0, 0, _cardFace.eulerAngles.z + 1), 10, 30, true,
                ShakeRandomnessMode.Harmonic));
        }

        private void PlayPointerInAnimation()
        {
            for (int i = 0; i < _pointerInTweener.Count; i++)
            {
                Debug.Log($"播放了 {_pointerInTweener[i]} {i}");
                _pointerInTweener[i].Restart();
            }
        }

        private void PlayPointerOutAnimation()
        {
            for (int i = 0; i < _pointerOutTweener.Count; i++)
            {
                _pointerOutTweener[i].Restart();
            }
        }

        private void PlaySelectedCardAnimation()
        {
            for (int i = 0; i < _selectedCardTweener.Count; i++)
            {
                _selectedCardTweener[i].Restart();
            }
        }

        private void AddPointerInList(Tweener tweener)
        {
            if (_pointerInTweener == null)
            {
                _pointerInTweener = new List<Tweener>();
            }

            _pointerInTweener.Add(tweener);
            tweener.Pause();
            tweener.SetAutoKill(false);
        }

        private void AddPointerOutList(Tweener tweener)
        {
            if (_pointerOutTweener == null)
            {
                _pointerOutTweener = new List<Tweener>();
            }

            _pointerOutTweener.Add(tweener);
            tweener.Pause();
            tweener.SetAutoKill(false);
        }

        private void AddSelectedCardList(Tweener tweener)
        {
            if (_selectedCardTweener == null)
            {
                _selectedCardTweener = new List<Tweener>();
            }

            _selectedCardTweener.Add(tweener);
            tweener.Pause();
            tweener.SetAutoKill(false);
        }

        public void SetOffSetY(float offSetY)
        {
            this.offsetPostionY = offSetY;
            _dragGroup.localPosition = _originPos + new Vector3(0, offSetY, 0);
        }

        public void SetRotateZ(float rotateZ)
        {
            this.offsetRoatetionZ = rotateZ;
        }
    }
}