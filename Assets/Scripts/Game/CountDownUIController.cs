/**
 * Copyright (C) 2019-2020 CR dot I Co.,Ltd.
 */
/**
 * タイトル：「開始前のカウントダウンUIを制御する」スクリプト
 * 
 * 作成情報： DATE:2019/06/18 作成者:中村 鷹広
 * 更新情報： DATE: 作成者:
 */

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountDownUIController : MonoBehaviour
{
    public static bool playFlg = false;
    public static bool isPlay = false;
    public static bool isComplete = false;

    [SerializeField]
    private CanvasGroup _startUICanvasGroup = null;

    [SerializeField]
    private RectTransform _startUIMaskRectTransform = null;
    [SerializeField]
    private RectTransform _startUIBarTopRectTransform = null;
    [SerializeField]
    private RectTransform _startUIBarUnderRectTransform = null;

    [SerializeField]
    private TextMeshProUGUI _startText = null;

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

    private void Awake()
    {
        DOTween.Init();
        DOTween.defaultAutoPlay = AutoPlay.None;
    }

    private void Start()
    {
        _startUICanvasGroup.alpha = 0.0f;
        _startUIMaskRectTransform.sizeDelta = startSizeDelta;
        _startText.characterSpacing = startCharaSpace;

        playFlg = false;
        isPlay = false;
        isComplete = false;

        _sequence =
            DOTween.Sequence()
                .AppendCallback(() => isPlay = true)
                .AppendCallback(() => _startText.text = "")
                .Append(
                        _startUICanvasGroup
                            .DOFade(1.0f, 0.25f)
                            .SetEase(this.easeType)
                    )
                .Append(
                        _startUIMaskRectTransform
                            .DOSizeDelta(stopSizeDelta, durationSeconds)
                            .SetEase(this.easeType)
                    )
                .Join(
                        _startUIBarTopRectTransform
                            .DOAnchorPos(new Vector2(_startUIBarTopRectTransform.anchoredPosition.x, (stopSizeDelta.y + _startUIBarTopRectTransform.sizeDelta.y) / 2 - 1), durationSeconds)
                            .SetEase(this.easeType)
                    )
                .Join(
                        _startUIBarUnderRectTransform
                            .DOAnchorPos(new Vector2(_startUIBarUnderRectTransform.anchoredPosition.x, -(stopSizeDelta.y + _startUIBarUnderRectTransform.sizeDelta.y) / 2 - 1), durationSeconds)
                            .SetEase(this.easeType)
                    )
                .AppendCallback(() => _startText.text = "3")
                .AppendInterval(interval)
                .AppendCallback(() => _startText.text = "2")
                .AppendInterval(interval)
                .AppendCallback(() => _startText.text = "1")
                .AppendInterval(interval)
                .AppendCallback(() => _startText.text = "Go")
                .AppendInterval(interval)
                .Append(
                        _startUIMaskRectTransform
                            .DOSizeDelta(endSizeDelta, durationSeconds)
                            .SetEase(this.easeType)
                    )
                .Join(
                        _startUIBarTopRectTransform
                            .DOAnchorPos(new Vector2(_startUIBarTopRectTransform.anchoredPosition.x, 25.0f), durationSeconds)
                            .SetEase(this.easeType)
                    )
                .Join(
                        _startUIBarUnderRectTransform
                            .DOAnchorPos(new Vector2(_startUIBarUnderRectTransform.anchoredPosition.x, -25.0f), durationSeconds)
                            .SetEase(this.easeType)
                    )
                .Join(
                        _startText
                            .DOCharacterSpacing(endCharaSpace, durationSeconds)
                            .SetEase(this.easeType)
                    )
                .Append(
                        _startUICanvasGroup
                            .DOFade(0.0f, 0.25f)
                            .SetEase(this.easeType)
                    )
                .AppendCallback(() => isPlay = false)
                .OnComplete(() => isComplete = true);

        _sequence.Pause();

        _startUICanvasGroup.gameObject.SetActive(false);
        _startUIBarTopRectTransform.gameObject.SetActive(false);
        _startUIBarUnderRectTransform.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (playFlg && !isPlay && !isComplete)
        {
            _sequence.Play();

            _startUICanvasGroup.gameObject.SetActive(true);
            _startUIBarTopRectTransform.gameObject.SetActive(true);
            _startUIBarUnderRectTransform.gameObject.SetActive(true);
        }

        if (isComplete)
        {
            _startUICanvasGroup.gameObject.SetActive(false);
            _startUIBarTopRectTransform.gameObject.SetActive(false);
            _startUIBarUnderRectTransform.gameObject.SetActive(false);

            _sequence.Kill();
        }
    }
}
