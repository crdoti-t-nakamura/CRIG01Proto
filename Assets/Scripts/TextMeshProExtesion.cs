/**
 * Copyright (C) 2019-2020 CR dot I Co.,Ltd.
 */
/**
 * タイトル：「TextMeshProを拡張して、文字間隔をDOTweenに対応させる」スクリプト
 * 
 * 作成情報： DATE:2019/06/18 作成者:中村 鷹広
 * 更新情報： DATE: 作成者:
 */

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class TextMeshProExtesion
{
    public static Tween DOCharacterSpacing(this TextMeshProUGUI textMeshProUgui, float amount, float duration)
    {
        return DOTween.To(() => textMeshProUgui.characterSpacing, x => textMeshProUgui.characterSpacing = x, amount, duration);
    }
}