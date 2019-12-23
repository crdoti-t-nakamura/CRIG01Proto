/**
 * Copyright (C) 2019-2020 CR dot I Co.,Ltd.
 */
/**
 * タイトル：「アイテムボタンウィンドウを制御する」スクリプト
 * 
 * 作成情報： DATE:2019/06/19 作成者:中村 鷹広
 * 更新情報： DATE: 作成者:
 */

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemButtonWindowController : MonoBehaviour
{
    public static bool isComplete = false;

    [SerializeField]
    private bool changeableFlg = false;

    [SerializeField]
    private RectTransform rectTransform = null;

    [SerializeField]
    private CanvasGroup canvasGroup = null;
    [SerializeField]
    private float duration = 1.0f;
    [SerializeField]
    private Ease displayEaseType = Ease.OutQuart;

    [SerializeField]
    private Ease notDisplayEaseType = Ease.InQuart;

    private Sequence sequence = null;

    [SerializeField]
    private Vector2 displayPosition = new Vector2(0.0f, 0.0f);

    [SerializeField]
    private Vector2 notDisplayPosition = new Vector2(-225.0f, -250.0f);

    void Start()
    {
        changeableFlg = false;
        rectTransform.anchoredPosition = notDisplayPosition;
        canvasGroup.alpha = 0.0f;

        changeableFlg = true;

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
        
        sequence.Pause();
    }

    public void ChangeDisplay()
    {
        if (changeableFlg)
        {
            sequence.Play();
        }
    }
}
