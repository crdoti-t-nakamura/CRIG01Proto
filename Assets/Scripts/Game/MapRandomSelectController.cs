/**
 * Copyright (C) 2019-2020 CR dot I Co.,Ltd.
 */
/**
 * タイトル：「マッチング中、マスタークライアント側でマップを選択する」スクリプト
 * 
 * 作成情報： DATE:2019/06/13 作成者:中村 鷹広
 * 更新情報： DATE: 作成者:
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRandomSelectController : MonoBehaviour
{
    [SerializeField]
    private bool debugMode = true;  // TODO 本番ではfalseに修正

    public static bool selectCompleteFlg = false;

    public static List<int> railNoList;
    [SerializeField]
    private List<int> debugRailNoList;

    public static Map selectMap;
    [SerializeField]
    private Map debugSelectMap;

    private void Start()
    {
        railNoList = new List<int>();

        selectCompleteFlg = false;
        debugRailNoList = new List<int>();

        selectMap = null;
        debugSelectMap = null;
    }

    private void Update()
    {
        if (!selectCompleteFlg && MapDataBase.createMapDataBaseFlg)
        {
            // マップのランダム選択を行う
            int SelectMapNumber = Random.Range(0, MapDataBase.maps.Count);
            Debug.Log("SelectMapNumber:" + SelectMapNumber.ToString());

            selectMap = MapDataBase.maps[SelectMapNumber];

            Debug.Log("mapName:" + selectMap.mapName.ToString());

            for (int i = 0; i < selectMap.mapRailGenerationSize; i++)
            {
                int SelectRailNo = Random.Range(1, selectMap.mapRailGameObjectList.Count + 1);
                Debug.Log("SelectRailNo:" + SelectRailNo.ToString());
                railNoList.Add(SelectRailNo);
                debugRailNoList.Add(SelectRailNo);
            }

            selectCompleteFlg = true;
        }

        if (debugMode)
        {
            // 雪山に設定
            if (Input.GetKeyDown(KeyCode.F1))
            {
                selectMap = MapDataBase.maps[0];
            }
            // 砂漠に設定
            else if (Input.GetKeyDown(KeyCode.F2))
            {
                selectMap = MapDataBase.maps[1];
            }
            debugSelectMap = selectMap;
        }
    }

    public static void SetMapRandomSelect(int selectMapId)
    {
        if (!selectCompleteFlg &&
            MapDataBase.createMapDataBaseFlg)
        {
            foreach (Map map in MapDataBase.maps)
            {
                if (map.mapId == selectMapId)
                {
                    selectMap = map;
                }
            }
        }

        selectCompleteFlg = true;
    }
}
