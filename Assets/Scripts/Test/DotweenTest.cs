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

    private List<Tweener> _tweeners;
    private Tweener _tweener1;
    private Tweener _tweener2;
    
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
        AddTweenStack(target.DOLocalMove(new Vector3(30, 30, 0),1f));
        AddTweenStack(target.DOScale(new Vector3(1.2f,1.2f,1),1f));
       
    }

    private void PlayeAnimation()
    {
        for (int i = 0; i < _tweeners.Count; i++)
        {
            _tweeners[i].PlayForward();
        }
    }

    private void BackwardPlayAnimation()
    {
        for (int i = _tweeners.Count -1; i >= 0 ; i--)
        {
            _tweeners[i].PlayBackwards();
        }
    }

    private void AddTweenStack(Tweener tweener)
    {
        if (_tweeners == null)
        {
            _tweeners = new List<Tweener>();
        }
        _tweeners.Add(tweener);
        tweener.Pause();
        tweener.SetAutoKill(false);
    }
    
    
    
}
