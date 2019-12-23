/**
 * Copyright (C) 2019-2020 CR dot I Co.,Ltd.
 */
/**
 * タイトル：「ゴール後、ゴールしたことを表示するUIの制御」スクリプト
 * 
 * 作成情報： DATE:2019/06/18 作成者:中村 鷹広
 * 更新情報： DATE: 作成者:
 */

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoalUIController : MonoBehaviour
{
    public static bool playFlg = false;
    public static bool isPlay = false;
    public static bool isComplete = false;

    [SerializeField]
    private CanvasGroup _finishUICanvasGroup = null;

    [SerializeField]
    private RectTransform _finishUIMaskRectTransform = null;
    [SerializeField]
    private RectTransform _finishUIBarTopRectTransform = null;
    [SerializeField]
    private RectTransform _finishUIBarUnderRectTransform = null;

    [SerializeField]
    private TextMeshProUGUI _finishText = null;

    [SerializeField]
    private Vector2 startSizeDelta = Vector2.zero;
    [SerializeField]
    private Vector2 stopSizeDelta = Vector2.zero;
    [SerializeField]
    private Vector2 endSizeDelta = Vector2.zero;

    [SerializeField]
    private float startCharaSpace = 5.0f;
    [SerializeField]
    private float stopCharaSpace = 30.0f;
    [SerializeField]
    private float endCharaSpace = 100.0f;

    [SerializeField]
    private float durationSeconds = 0.5f;
    [SerializeField]
    private float CharaSpaceDurationSeconds = 2.5f;
    [SerializeField]
    private Ease easeType = Ease.Unset;
    [SerializeField]
    private float interval = 3.0f;

    private Sequence _sequence = null;

    [SerializeField]
    private bool debugPlayFlg = false;
    [SerializeField]
    private bool debugIsPlay = false;
    [SerializeField]
    private bool debugIsComplete = false;

    private void Start()
    {
        _finishUICanvasGroup.alpha = 0.0f;
        _finishUIMaskRectTransform.sizeDelta = startSizeDelta;
        _finishText.characterSpacing = startCharaSpace;

        playFlg = false;
        isPlay = false;
        isComplete = false;

        debugPlayFlg = false;
        debugIsPlay = false;
        debugIsComplete = false;

        _sequence =
            DOTween.Sequence()
                .AppendCallback(() => isPlay = true)
                .AppendCallback(() => _finishText.text = "GOAL")
                .Append(
                        _finishUICanvasGroup
                            .DOFade(1.0f, 0.25f)
                            .SetEase(this.easeType)
                    )
                .Append(
                        _finishUIMaskRectTransform
                            .DOSizeDelta(stopSizeDelta, durationSeconds)
                            .SetEase(this.easeType)
                    )
                .Join(
                        _finishUIBarTopRectTransform
                            .DOAnchorPos(new Vector2(_finishUIBarTopRectTransform.anchoredPosition.x, (stopSizeDelta.y + _finishUIBarTopRectTransform.sizeDelta.y) / 2 - 1), durationSeconds)
                            .SetEase(this.easeType)
                    )
                .Join(
                        _finishUIBarUnderRectTransform
                            .DOAnchorPos(new Vector2(_finishUIBarUnderRectTransform.anchoredPosition.x, -(stopSizeDelta.y + _finishUIBarUnderRectTransform.sizeDelta.y) / 2 - 1), durationSeconds)
                            .SetEase(this.easeType)
                    )
                .Join(
                        _finishText
                            .DOCharacterSpacing(stopCharaSpace, CharaSpaceDurationSeconds)
                            .SetEase(this.easeType)
                    )
                .AppendInterval(interval)
                .Append(
                        _finishUIMaskRectTransform
                            .DOSizeDelta(endSizeDelta, durationSeconds)
                            .SetEase(this.easeType)
                    )
                .Join(
                        _finishUIBarTopRectTransform
                            .DOAnchorPos(new Vector2(_finishUIBarTopRectTransform.anchoredPosition.x, 25.0f), durationSeconds)
                            .SetEase(this.easeType)
                    )
                .Join(
                        _finishUIBarUnderRectTransform
                            .DOAnchorPos(new Vector2(_finishUIBarUnderRectTransform.anchoredPosition.x, -25.0f), durationSeconds)
                            .SetEase(this.easeType)
                    )
                .Join(
                        _finishText
                            .DOCharacterSpacing(endCharaSpace, durationSeconds)
                            .SetEase(this.easeType)
                    )
                .Append(
                        _finishUICanvasGroup
                            .DOFade(0.0f, 0.25f)
                            .SetEase(this.easeType)
                    )
                .AppendCallback(() => isPlay = false)
                .OnComplete(() => isComplete = true);

        _sequence.Pause();

        _finishUICanvasGroup.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (playFlg && !isPlay && !isComplete)
        {
            _finishUICanvasGroup.gameObject.SetActive(true);

            _sequence.Play();
        }

        if (isComplete)
        {
            _finishUICanvasGroup.gameObject.SetActive(false);

            _sequence.Kill();
        }

        debugPlayFlg = playFlg;
        debugIsPlay = isPlay;
        debugIsComplete = isComplete;
    }
}
