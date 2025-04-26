using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DotweenTest : MonoBehaviour
{
    public RectTransform target;
    private Button _button;
    private bool isFirst = true;

    private Tweener _tweener;
    private Vector3 originPos;
    
    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
        originPos = target.localPosition;
    }

    private void Start()
    {
        StartAnimation();
    }

    private void OnClick()
    {
        if (isFirst)
        {
            isFirst = false;
            PlayeAnimation();
        }
        else
        {
            isFirst = true;
            BackwardPlayAnimation();
        }
    }

    private void StartAnimation()
    {
        _tweener = target.DOLocalMove(new Vector3(30, 30, 0),1f);
        _tweener.Pause();
        _tweener.SetAutoKill(false);
    }

    private void PlayeAnimation()
    {
       _tweener.PlayForward();
    }

    private void BackwardPlayAnimation()
    {
        _tweener.PlayBackwards();
    }
    
    
    
}
