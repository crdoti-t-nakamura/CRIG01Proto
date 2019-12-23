/**
 * Copyright (C) 2019-2020 CR dot I Co.,Ltd.
 */
/**
 * タイトル：「スコアタイムウィンドウを制御する」スクリプト
 * 
 * 作成情報： DATE:2019/06/19 作成者:中村 鷹広
 * 更新情報： DATE: 作成者:
 */

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTimeWindowController : MonoBehaviour
{
    public static bool isComplete = false;

    [SerializeField]
    private bool changeableFlg = false;

    [SerializeField]
    private RectTransform scoreTimeBG01RectTF = null;
    [SerializeField]
    private RectTransform scoreTimeBG02RectTF = null;
    [SerializeField]
    private RectTransform scoreTimeBG03RectTF = null;

    [SerializeField]
    private CanvasGroup canvasGroup = null;
    [SerializeField]
    private float duration = 1.0f;

    [SerializeField]
    private Ease scoreTimeBG01DisplayEaseType = Ease.OutCubic;
    [SerializeField]
    private Ease scoreTimeBG02DisplayEaseType = Ease.OutQuart;
    [SerializeField]
    private Ease scoreTimeBG03DisplayEaseType = Ease.OutQuint;
    
    [SerializeField]
    private Ease scoreTimeBG01NotDisplayEaseType = Ease.InCubic;
    [SerializeField]
    private Ease scoreTimeBG02NotDisplayEaseType = Ease.InQuart;
    [SerializeField]
    private Ease scoreTimeBG03NotDisplayEaseType = Ease.InQuint;

    private Sequence sequence = null;

    [SerializeField]
    private Vector2 scoreTimeBG01DisplayPosition = new Vector2(-442.0f, -375.0f);
    [SerializeField]
    private Vector2 scoreTimeBG02DisplayPosition = new Vector2(-417.0f, -375.0f);
    [SerializeField]
    private Vector2 scoreTimeBG03DisplayPosition = new Vector2(-392.0f, -375.0f);
    
    [SerializeField]
    private Vector2 scoreTimeBG01NotDisplayPosition = new Vector2(-667.0f, -625.5f);
    [SerializeField]
    private Vector2 scoreTimeBG02NotDisplayPosition = new Vector2(-642.0f, -625.0f);
    [SerializeField]
    private Vector2 scoreTimeBG03NotDisplayPosition = new Vector2(-617.0f, -625.0f);

    void Start()
    {
        changeableFlg = false;

        scoreTimeBG01RectTF.anchoredPosition = scoreTimeBG01NotDisplayPosition;
        scoreTimeBG02RectTF.anchoredPosition = scoreTimeBG02NotDisplayPosition;
        scoreTimeBG03RectTF.anchoredPosition = scoreTimeBG03NotDisplayPosition;

        canvasGroup.alpha = 0.0f;

        changeableFlg = true;

        sequence = DOTween.Sequence()
            .Append(
                scoreTimeBG01RectTF
                    .DOAnchorPos(scoreTimeBG01DisplayPosition, duration)
                    .SetEase(scoreTimeBG01DisplayEaseType)
                    )
            .Join(
                scoreTimeBG02RectTF
                    .DOAnchorPos(scoreTimeBG02DisplayPosition, duration)
                    .SetEase(scoreTimeBG02DisplayEaseType)
                    )
            .Join(
                scoreTimeBG03RectTF
                    .DOAnchorPos(scoreTimeBG03DisplayPosition, duration)
                    .SetEase(scoreTimeBG03DisplayEaseType)
                    )
            .Join(
                canvasGroup.DOFade(1.0f, duration)
                    )
            .AppendCallback(() => changeableFlg = true)
            .AppendCallback(() => sequence.Pause())
            .AppendCallback(() => changeableFlg = false)
            .Append(
                scoreTimeBG01RectTF
                    .DOAnchorPos(scoreTimeBG01NotDisplayPosition, duration)
                    .SetEase(scoreTimeBG01NotDisplayEaseType)
                    )
            .Join(
                scoreTimeBG02RectTF
                    .DOAnchorPos(scoreTimeBG02NotDisplayPosition, duration)
                    .SetEase(scoreTimeBG02NotDisplayEaseType)
                    )
            .Join(
                scoreTimeBG03RectTF
                    .DOAnchorPos(scoreTimeBG03NotDisplayPosition, duration)
                    .SetEase(scoreTimeBG03NotDisplayEaseType)
                    )
            .Join(
                canvasGroup.DOFade(0.0f, duration)
                    )
            .OnComplete(() => isComplete = true);

        sequence.Pause();
    }

    void Update()
    {

    }

    public void ChangeDisplay()
    {
        if (changeableFlg)
        {
            sequence.Play();
        }
    }
}
