/**
 * Copyright (C) 2019-2020 CR dot I Co.,Ltd.
 */
/**
 * タイトル：「スピードメーターウィンドウを制御する」スクリプト
 * 
 * 作成情報： DATE:2019/06/18 作成者:中村 鷹広
 * 更新情報： DATE: 作成者:
 */

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedMeteWindowController : MonoBehaviour
{
    public static bool isComplete = false;

    [SerializeField]
    private bool changeableFlg = false;

    //[SerializeField]
    //private RectTransform rectTransform = null;

    [SerializeField]
    private RectTransform speedMeterBG01RectTF = null;
    [SerializeField]
    private RectTransform speedMeterBG02RectTF = null;
    [SerializeField]
    private RectTransform speedMeterBG03RectTF = null;

    [SerializeField]
    private CanvasGroup canvasGroup = null;
    [SerializeField]
    private float duration = 1.0f;
    //[SerializeField]
    //private Ease displayEaseType = Ease.OutQuart;

    [SerializeField]
    private Ease speedMeterBG01DisplayEaseType = Ease.OutQuart;
    [SerializeField]
    private Ease speedMeterBG02DisplayEaseType = Ease.OutQuint;
    [SerializeField]
    private Ease speedMeterBG03DisplayEaseType = Ease.OutSine;

    //[SerializeField]
    //private Ease notDisplayEaseType = Ease.InQuart;

    [SerializeField]
    private Ease speedMeterBG01NotDisplayEaseType = Ease.InQuart;
    [SerializeField]
    private Ease speedMeterBG02NotDisplayEaseType = Ease.InQuint;
    [SerializeField]
    private Ease speedMeterBG03NotDisplayEaseType = Ease.InSine;

    private Sequence sequence = null;

    //[SerializeField]
    //private Vector2 displayPosition = new Vector2(0.0f, 0.0f);

    [SerializeField]
    private Vector2 speedMeterBG01DisplayPosition = new Vector2(-442.0f, -375.0f);
    [SerializeField]
    private Vector2 speedMeterBG02DisplayPosition = new Vector2(-417.0f, -375.0f);
    [SerializeField]
    private Vector2 speedMeterBG03DisplayPosition = new Vector2(-392.0f, -375.0f);

    //[SerializeField]
    //private Vector2 notDisplayPosition = new Vector2(-225.0f, -250.0f);

    [SerializeField]
    private Vector2 speedMeterBG01NotDisplayPosition = new Vector2(-667.0f, -625.5f);
    [SerializeField]
    private Vector2 speedMeterBG02NotDisplayPosition = new Vector2(-642.0f, -625.0f);
    [SerializeField]
    private Vector2 speedMeterBG03NotDisplayPosition = new Vector2(-617.0f, -625.0f);

    void Start()
    {
        changeableFlg = false;
        //rectTransform.anchoredPosition = notDisplayPosition;

        speedMeterBG01RectTF.anchoredPosition = speedMeterBG01NotDisplayPosition;
        speedMeterBG02RectTF.anchoredPosition = speedMeterBG02NotDisplayPosition;
        speedMeterBG03RectTF.anchoredPosition = speedMeterBG03NotDisplayPosition;

        canvasGroup.alpha = 0.0f;

        changeableFlg = true;

        /*
        sequence = DOTween.Sequence()
            .Append(
                rectTransform
                    .DOAnchorPos(displayPosition, duration)
                    .SetEase(displayEaseType)
                    )
            .Join(
                canvasGroup.DOFade(1.0f, duration)
                    )
            .AppendCallback(() => changeableFlg = true)
            .AppendCallback(() => sequence.Pause())
            .AppendCallback(() => changeableFlg = false)
            .Append(
                rectTransform
                    .DOAnchorPos(notDisplayPosition, duration)
                    .SetEase(notDisplayEaseType)
                    )
            .Join(
                canvasGroup.DOFade(0.0f, duration)
                    )
            .OnComplete(() => isComplete = true);
        */
        sequence = DOTween.Sequence()
            .Append(
                speedMeterBG01RectTF
                    .DOAnchorPos(speedMeterBG01DisplayPosition, duration)
                    .SetEase(speedMeterBG01DisplayEaseType)
                    )
            .Join(
                speedMeterBG02RectTF
                    .DOAnchorPos(speedMeterBG02DisplayPosition, duration)
                    .SetEase(speedMeterBG02DisplayEaseType)
                    )
            .Join(
                speedMeterBG03RectTF
                    .DOAnchorPos(speedMeterBG03DisplayPosition, duration)
                    .SetEase(speedMeterBG03DisplayEaseType)
                    )
            .Join(
                canvasGroup.DOFade(1.0f, duration)
                    )
            .AppendCallback(() => changeableFlg = true)
            .AppendCallback(() => sequence.Pause())
            .AppendCallback(() => changeableFlg = false)
            .Append(
                speedMeterBG01RectTF
                    .DOAnchorPos(speedMeterBG01NotDisplayPosition, duration)
                    .SetEase(speedMeterBG01NotDisplayEaseType)
                    )
            .Join(
                speedMeterBG02RectTF
                    .DOAnchorPos(speedMeterBG02NotDisplayPosition, duration)
                    .SetEase(speedMeterBG02NotDisplayEaseType)
                    )
            .Join(
                speedMeterBG03RectTF
                    .DOAnchorPos(speedMeterBG03NotDisplayPosition, duration)
                    .SetEase(speedMeterBG03NotDisplayEaseType)
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
