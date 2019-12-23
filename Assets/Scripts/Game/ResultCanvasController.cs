/**
 * Copyright (C) 2019-2020 CR dot I Co.,Ltd.
 */
/**
 * タイトル：「リザルト画面の制御」スクリプト
 * 
 * 作成情報： DATE:2019/06/18 作成者:中村 鷹広
 * 更新情報： DATE: 作成者:
 */

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultCanvasController : MonoBehaviour
{
    public static bool playFlg = false;
    public static bool isPlay = false;
    public static bool isComplete = false;

    [SerializeField]
    private Image _panelImage = null;
    [SerializeField]
    private RectTransform _windowRectTransform = null;

    [SerializeField]
    private float durationSeconds = 0.5f;
    [SerializeField]
    private Ease easeType = Ease.Linear;

    private Sequence _sequence = null;

    public TextMeshProUGUI _resultDetailScoreTimeText;
    public TimeCounter _timeCounter;

    [SerializeField]
    private bool debugPlayFlg = false;
    [SerializeField]
    private bool debugIsPlay = false;
    [SerializeField]
    private bool debugIsComplete = false;
    
    private void Start()
    {
        playFlg = false;
        isPlay = false;
        isComplete = false;

        debugPlayFlg = false;
        debugIsPlay = false;
        debugIsComplete = false;

        _sequence = null;

        Color startColor = new Color(
                                    _panelImage.color.r,
                                    _panelImage.color.g,
                                    _panelImage.color.b,
                                    0.0f
                                    );

        Color endColor = new Color(
                                    _panelImage.color.r,
                                    _panelImage.color.g,
                                    _panelImage.color.b,
                                    0.5f
                                    );


        Vector3 startPosition = new Vector3(
                                    _windowRectTransform.position.x,
                                    _windowRectTransform.position.y,
                                    _windowRectTransform.position.z
                                    );

        Vector3 endPosition = new Vector3(
                                    0.0f,
                                    0.0f,
                                    _windowRectTransform.position.z
                                    );

        _panelImage.color = startColor;
        _windowRectTransform.position = startPosition;

        _sequence =
            DOTween.Sequence()
                .AppendCallback(() => isPlay = true)
                .AppendCallback(() => _resultDetailScoreTimeText.text = _timeCounter.countUp.ToString("f3") + " 秒")
                .Append(
                        _panelImage
                            .DOColor(endColor, durationSeconds)
                            .SetEase(this.easeType)
                    )
                .Join(
                        _windowRectTransform
                            .DOAnchorPos(endPosition, durationSeconds)
                            .SetEase(this.easeType)
                    )
                .AppendCallback(() => isPlay = false)
                .OnComplete(() => isComplete = true);

        _sequence.Pause();

        _panelImage.gameObject.SetActive(false);
        _windowRectTransform.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (playFlg & !isPlay && !isComplete)
        {
            _sequence.Play();
            //isPlay = true;

            _panelImage.gameObject.SetActive(true);
            _windowRectTransform.gameObject.SetActive(true);
        }

        debugPlayFlg = playFlg;
        debugIsPlay = isPlay;
        debugIsComplete = isComplete;
    }
}
