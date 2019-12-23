/**
 * Copyright (C) 2019-2020 CR dot I Co.,Ltd.
 */
/**
 * タイトル：「文字点滅制御」スクリプト
 * 
 * 作成情報： DATE:2019/06/13 作成者:中村 鷹広
 * 更新情報： DATE: 作成者:
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TouchController : MonoBehaviour
{
    public TextBlink startTextBlink;

    [SerializeField]
    private float blickSpeed;
    [SerializeField]
    private float waitTime;

    [SerializeField]
    private bool changeSceneFlg;
    [SerializeField]
    private bool waitCompleteFlg;

    private AsyncOperation async;

    void Start()
    {
        blickSpeed = 4.0f;
        waitTime = 1.5f;
        changeSceneFlg = false;
        waitCompleteFlg = false;
    }

    void Update()
    {
        TouchInfo info = AppUtil.GetTouch();
        if (info == TouchInfo.Ended)
        {
            // タッチ終了
            startTextBlink.SetSpeed(blickSpeed);

            if (!changeSceneFlg)
            {
                changeSceneFlg = true;

                StartCoroutine(Wait(waitTime));
            }
        }

        if (waitCompleteFlg)
        {
            async.allowSceneActivation = true;
        }
    }

    private IEnumerator Wait(float waitTime)
    {
        // シーンの読み込みをする
        async = SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Single);
        async.allowSceneActivation = false;

        yield return StartCoroutine(AppUtil.WaitForSeconds(waitTime));
        waitCompleteFlg = true;
    }
}
