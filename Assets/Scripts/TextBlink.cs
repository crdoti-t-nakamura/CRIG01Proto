/**
 * Copyright (C) 2019-2020 CR dot I Co.,Ltd.
 */
/**
 * タイトル：「文字点滅」スクリプト
 * 
 * 作成情報： DATE:2019/06/13 作成者:中村 鷹広
 * 更新情報： DATE: 作成者:
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextBlink : MonoBehaviour
{
    private TextMeshProUGUI text;

    [SerializeField]
    private float speed = 1.0f;
    [SerializeField]
    private float time;

    void Start()
    {
        time = 0.0f;
        text = gameObject.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        text.color = GetAlphaColor(text.color);
    }

    //Alpha値を更新してColorを返す
    Color GetAlphaColor(Color color)
    {
        time += Time.deltaTime * 5.0f * speed;
        color.a = Mathf.Sin(time) * 0.5f + 0.5f;

        return color;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }
}
