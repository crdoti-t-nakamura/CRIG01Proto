/**
 * Copyright (C) 2019-2020 CR dot I Co.,Ltd.
 */
/**
 * タイトル：「マップ情報作成」スクリプト
 * 
 * 作成情報： DATE:2019/06/13 作成者:中村 鷹広
 * 更新情報： DATE: 作成者:
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDataBase : MonoBehaviour
{
    public static bool createMapDataBaseFlg = false;

    // リスト化をして下のvoid Start内でリストに値を追加、値は適当です
    public static List<Map> maps = null;
    
    public List<GameObject> snowyMtRailList = new List<GameObject>();

    private void Awake()
    {
        createMapDataBaseFlg = false;
        maps = new List<Map>();
    }

    private void Start()
    {
        // アニマ山脈（雪山）
        maps.Add(new Map(
            0,                          // マップID
            "アニマ山脈",               // マップ名
            "SnowyMt",                  // マップ名（英字）
            7,                          // マップ線路生成数
            snowyMtRailList             // マップ線路オブジェクトリスト
            ));
        /*
        // リミカ砂漠（砂漠）
        maps.Add(new Map(
            1,                          // マップID
            "リミカ砂漠",               // マップ名
            "Desert",                   // マップ名（英字）
            7,                          // マップ線路生成数
            5                           // マップ線路パターン
            ));
        */
        /*
        // グリム炭坑（洞窟）
        maps.Add(new Map(
            1,                          // マップID
            "グリム炭坑",               // マップ名
            "Cave",                     // マップ名（英字）
            7,                          // マップ線路生成数
            5                           // マップ線路パターン
            ));
        */

        // 今後実装予定のもの
        // ゴブロ遺跡（遺跡）
        // 海底遺跡（海底遺跡）

        createMapDataBaseFlg = true;
    }
}
