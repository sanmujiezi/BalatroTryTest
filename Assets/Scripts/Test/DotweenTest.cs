using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DotweenTest : MonoBehaviour
{
    [Range(0, 360)] public float sineAngle;
    [Range(0, 360)] public float cosineAngle;
    public RectTransform content;
    public AnimationCurve curve;
    
    public AnimationCurve heightCurve; // 高度变化曲线（Inspector中编辑）
    public float waveSpeed = 1f;       // 波浪速度
    public float waveHeight = 2f;      // 波浪幅度
    public Vector3[] originPoss;
    
    public RectTransform target;
    public float shakeAngle = 10;
    public int vibrato = 20;
    public float randomness = 50;
    public float inful = 0;
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
        Debug.Log($"Curve: {curve}");
        LayoutRebuilder.ForceRebuildLayoutImmediate(content);
        originPoss = new Vector3[content.childCount];
        for (int i = 0; i < content.childCount; i++)
        {
            originPoss[i] = content.GetChild(i).position;
        }
    }

    private void Update()
    {
        float time = Time.time * waveSpeed;
        for (int i = 0; i < content.childCount; i++)
        {
            float curveValue = heightCurve.Evaluate(time);
            Vector3 pos = originPoss[i];
            pos.y += curveValue * waveHeight; // 调整Y轴高度
            content.GetChild(i).position = pos;
        }

        
        float sine = Mathf.Sin(sineAngle) * 10;
        float cosine = Mathf.Cos(cosineAngle) * 10;

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
        AddTweenStack(target.DOLocalMove(new Vector3(30, 30, 0), 1f));
        AddTweenStack(target.DOScale(new Vector3(1.2f, 1.2f, 1), 1f));
        AddTweenStack(target.DOShakeRotation(0.2f, new Vector3(0, 0, shakeAngle), vibrato, randomness, true,
            ShakeRandomnessMode.Harmonic));
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
        for (int i = _tweeners.Count - 1; i >= 0; i--)
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