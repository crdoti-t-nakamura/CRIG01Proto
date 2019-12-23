/**
 * Copyright (C) 2019-2020 CR dot I Co.,Ltd.
 */
/**
 * タイトル：「マップ」スクリプト
 * 
 * 作成情報： DATE:2019/06/13 作成者:中村 鷹広
 * 更新情報： DATE: 作成者:
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]// この属性を使ってインスペクター上で表示
public class Map
{
    public int mapId;                               // マップID
    public string mapName;                          // マップ名
    public string mapCharaName;                     // マップ名（英字）
    public Sprite mapIcon;                          // マップアイコン
    public int mapRailGenerationSize;               // マップ線路生成数
    public List<GameObject> mapRailGameObjectList;  // マップ線路オブジェクトリスト

    // ここでリスト化時に渡す引数をあてがいます   
    public Map(
        int id,
        string name,
        string charaName,
        int railGenerationSize,
        List<GameObject> railGameObjectList
        )
    {
        mapId = id;                                     // マップID
        mapName = name;                                 // マップ名
        mapCharaName = charaName;                       // マップ名（英字）
        // アイコンはcharaNameとイコールにするのでアイコンがあるパス＋charaNameで取ってきます 
        mapIcon = Resources.Load<Sprite>("Maps/Icons/icon_" + charaName.ToLower());
        Debug.Log("mapIcon = Maps/Icons/icon_" + charaName.ToLower());
        mapRailGenerationSize = railGenerationSize;     // マップ線路生成数
        mapRailGameObjectList = railGameObjectList;     // マップ線路オブジェクトリスト
    }
}
