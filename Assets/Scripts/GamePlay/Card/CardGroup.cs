using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GamePlay
{
    public class CardGroup : MonoBehaviour
    {
        private GameObject _cardPrefab;
        private GameObject _soltsPrefab;
        private GameObject _cardFacePrefab;
        private RectTransform _content;
        private RectTransform _visualGroup;
        private List<CardController> _cards;
        private List<GameObject> _cardsolts;
        private CardController _dragCard;
        private float _matchCardArea;


        int cardLength = 7;

        private void Awake()
        {
            _cardPrefab = Resources.Load<GameObject>("Prefabs/Card");
            _soltsPrefab = Resources.Load<GameObject>("Prefabs/CardSolts");
            _content = transform.Find("Content").GetComponent<RectTransform>();
            _visualGroup = transform.Find("VisualGroup").GetComponent<RectTransform>();
            _matchCardArea = 50;

            InitCard();
        }

        private void InitCard()
        {
            if (_content.childCount > 0)
            {
                for (int i = _content.childCount - 1; i > -1; i--)
                {
                    Destroy(_content.GetChild(i).gameObject);
                }
            }

            if (_cards != null && _cards.Count > 0)
            {
                _cards.Clear();
            }
            else
            {
                _cards = new List<CardController>(cardLength);
            }

            for (int i = 0; i < cardLength; i++)
            {
                GameObject solts = Instantiate(_soltsPrefab, _content);
                GameObject card = Instantiate(_cardPrefab, solts.transform);
                CardController cardController = card.GetComponent<CardController>();
                
                cardController.SetCardFaceParent(_visualGroup);
                cardController.cardGroup = this;
                cardController.SetSortOrder(i + 1);

                card.gameObject.name = i.ToString();
                
                _cards.Add(cardController);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(_content);
        }

        public void SetDragCard(CardController controller)
        {
            _dragCard = controller;
        }

        public void CardCheckOnDrag()
        {
            float dragCardX = _dragCard._dragCard.position.x;
            //Debug.Log($"{dragCardX}");
            for (int i = 0; i < _content.childCount; i++)
            {
                Transform child = _content.GetChild(i);
                if (dragCardX >= child.position.x - _matchCardArea && dragCardX <= child.position.x + _matchCardArea)
                {
                    MoveCard(i);
                    return;
                }
            }
        }

        // 检测拖拽物体在其父对象中的排序位置，如果拖动超过设定值则调整排

        private void MoveCard(int soltsIndex)
        {
            Transform dragParent = _dragCard.transform.parent;
            Vector3 DragPos = _dragCard._dragCard.position;
            Vector3 FacePos = _dragCard._cardFace.position;
            int dragIndex = dragParent.GetSiblingIndex();
            int targetIndex = soltsIndex;

            int moveDir = targetIndex - dragIndex;

            //左移
            if (moveDir > 0)
            {
                Transform temp = _content.GetChild(dragIndex).GetChild(0);

                for (int i = targetIndex; i >= dragIndex; i--)
                {
                    Transform solts = _content.GetChild(i);
                    if (i == targetIndex)
                    {
                        temp.GetComponent<CardController>().SetSortOrder(i + 1,true);
                    }
                    else
                    {
                        temp.GetComponent<CardController>().SetSortOrder(i + 1);
                    }
                    temp.SetParent(solts);
                    temp.localPosition = Vector2.zero;
                    _dragCard._dragCard.position = DragPos;
                    _dragCard._cardFace.position = FacePos;
                    temp = solts.GetChild(0);
                }
            }
            else if (moveDir < 0)
            {
                Transform temp = _content.GetChild(dragIndex).GetChild(0);
                for (int i = targetIndex; i <= dragIndex; i++)
                {
                    Debug.Log($"{temp.gameObject.name}");
                    Transform solts = _content.GetChild(i);
                    if (i == targetIndex)
                    {
                        temp.GetComponent<CardController>().SetSortOrder(i + 1,true);
                    }
                    else
                    {
                        temp.GetComponent<CardController>().SetSortOrder(i + 1);
                    }

                    temp.SetParent(solts);
                    temp.localPosition = Vector2.zero;
                    _dragCard._dragCard.position = DragPos;
                    _dragCard._cardFace.position = FacePos;
                    temp = solts.GetChild(0);
                }
            }
        }
    }
}