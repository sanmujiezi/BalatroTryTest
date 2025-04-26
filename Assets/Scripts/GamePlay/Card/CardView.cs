using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GamePlay
{
    public class CardView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler,
        IDragHandler, IBeginDragHandler, IEndDragHandler,IPointerDownHandler,IPointerUpHandler
    {
        private CardController _controller;

        private void Awake()
        {
            _controller = GetComponent<CardController>();   
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("OnClick");
            _controller.OnPointerClickCard(eventData);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("OnPointerEnter");
            _controller.OnPointerEnterCard(eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("OnPointerExit");
            _controller.OnPointerExitCard(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            //Debug.Log("OnDrag");
            _controller.OnDragCard(eventData);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("OnBeginDrag");
            _controller.OnBeginDragCard(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("OnEndDrag");
            _controller.OnEndDragCard(eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("OnPointerDown");
            _controller.OnPointerDownCard(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Debug.Log("OnPointerUP");
            _controller.OnPointerUpCard(eventData);
        }
    }
}