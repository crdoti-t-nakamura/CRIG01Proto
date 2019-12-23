/**
 * Copyright (C) 2019-2020 CR dot I Co.,Ltd.
 */
/**
 * タイトル：「時間を計測する」スクリプト
 * 
 * 作成情報： DATE:2019/06/19 作成者:中村 鷹広
 * 更新情報： DATE: 作成者:
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeCounter : MonoBehaviour
{
    //カウントアップ
    public float countUp = 0.0f;

    //時間を表示するText型の変数
    public TextMeshProUGUI timeText;

    //ポーズしているかどうか
    public static bool isPose = false;

    void Start()
    {
        isPose = true;
        countUp = 0.0f;
    }

    void Update()
    {
        //ポーズ中かどうか
        if (isPose)
        {
            //時間を表示する
            timeText.text = countUp.ToString("f3") + " 秒";

            //カウントダウンしない
            return;
        }

        //時間をカウントする
        countUp += Time.deltaTime;

        //時間を表示する
        timeText.text = countUp.ToString("f3") + " 秒";
    }
}
