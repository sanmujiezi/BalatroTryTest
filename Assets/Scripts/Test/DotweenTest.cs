using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DotweenTest : MonoBehaviour
{
    [Range(0,360)] public float sineAngle;
    [Range(0,360)] public float cosineAngle;
    public AnimationCurve curve;
    public RectTransform target;
    public float shakeAngle = 10;
    public int vibrato = 20;
    public float randomness = 50;
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

    private void Update()
    {
        float sine =  Mathf.Sin(sineAngle ) * 10;
        float cosine = Mathf.Cos(cosineAngle ) * 10;
            
        target.eulerAngles = new Vector3(sine, cosine, 0);
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
        AddTweenStack(target.DOShakeRotation(0.2f,new Vector3(0,0,shakeAngle),vibrato,randomness,true,ShakeRandomnessMode.Harmonic));
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
