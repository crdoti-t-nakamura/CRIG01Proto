/**
 * Copyright (C) 2019-2020 CR dot I Co.,Ltd.
 */
/**
 * タイトル：「対象のプレイヤーを追従するカメラ」スクリプト
 * 
 * 作成情報： DATE:2019/06/16 作成者:中村 鷹広
 * 更新情報： DATE: 作成者:
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera _camera;

    [SerializeField]
    private string playerTag;                                           // 対象となるプレイヤーのタグ
    private GameObject player;                                          // 対象となるプレイヤーのオブジェクト

    [SerializeField]
    private bool defaultValidFlag;                                      // デフォルト有効フラグ
    [SerializeField]
    private float cameraOffsetPosX;                                     // プレイヤーとの距離
    [SerializeField]
    private float cameraOffsetPosY;                                     // プレイヤーとの距離
    private static readonly float DEFAULT_CAMERA_OFFSET_POS_X = 4.5f;   // デフォルトの距離
    private static readonly float DEFAULT_CAMERA_OFFSET_POS_Y = 0.0f;   // デフォルトの距離

    private bool backgroundColorChangeFlg = false;

    private void Start()
    {
        backgroundColorChangeFlg = false;

        _camera = null;
        _camera = GetComponent<Camera>();

        // タグ名からキャラクターオブジェクトを取得
        player = GameObject.FindGameObjectWithTag(playerTag);

        // デフォルト有効フラグが"有効"の場合
        if (defaultValidFlag)
        {
            // デフォルト値を入れる
            cameraOffsetPosX = DEFAULT_CAMERA_OFFSET_POS_X;
            cameraOffsetPosY = DEFAULT_CAMERA_OFFSET_POS_Y;
        }
    }

    private void LateUpdate()
    {
        if (!backgroundColorChangeFlg &&
            MapRandomSelectController.selectCompleteFlg &&
            MapDataBase.createMapDataBaseFlg)
        {
            // 背景色の変更
            // 雪山
            if (_camera != null &&
                MapRandomSelectController.selectMap.mapId == MapDataBase.maps[0].mapId)
            {
                _camera.backgroundColor = new Color32(49, 77, 121, 0);
                backgroundColorChangeFlg = true;
            }
            // 砂漠
            else if (_camera != null &&
                MapRandomSelectController.selectMap.mapId == MapDataBase.maps[1].mapId)
            {
                _camera.backgroundColor = new Color32(162, 241, 254, 0);
                backgroundColorChangeFlg = true;
            }
        }

        if (backgroundColorChangeFlg)
        {

            // キャラクターを設定
            if (player == null)
            {
                // タグ名からキャラクターオブジェクトを取得
                player = GameObject.FindGameObjectWithTag(playerTag);
                return;
            }

            // プレイヤーのポジションを取得
            Vector3 playerPos = player.transform.position;

            // 取得したポジションにオフセット値を加えたポジションへ移動
            transform.position = new Vector3(
                playerPos.x + cameraOffsetPosX,
                playerPos.y + cameraOffsetPosY,
                transform.position.z);
        }
    }
}
