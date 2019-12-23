/**
 * Copyright (C) 2019-2020 CR dot I Co.,Ltd.
 */
/**
 * タイトル：「ゲームプレイ時のエリアを限定する」スクリプト
 * 
 * 作成情報： DATE:2019/06/19 作成者:中村 鷹広
 * 更新情報： DATE: 作成者:
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayAreaController : MonoBehaviour
{
    private PlayerController _playerController;

    [SerializeField]
    // プレイヤーが出現する位置
    private Vector3 initPos = new Vector3(-1.7500f, 1.0000f, 0.0000f);

    void Start()
    {
        _playerController = null;
    }

    public void OnTriggerExit2D(Collider2D hit)
    {
        Debug.Log("OnTriggerEnter2D");

        _playerController = null;

        if (hit.gameObject.tag == "Player")
        {
            _playerController = hit.gameObject.GetComponent<PlayerController>();

            _playerController.ResetVelocity();
            _playerController.transform.position = initPos;
            _playerController.transform.rotation = Quaternion.identity;
        }
    }
}
