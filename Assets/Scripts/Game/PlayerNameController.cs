/**
 * Copyright (C) 2019-2020 CR dot I Co.,Ltd.
 */
/**
 * タイトル：「プレイヤー名を取得する」スクリプト
 * 
 * 作成情報： DATE:2019/06/17 作成者:中村 鷹広
 * 更新情報： DATE: 作成者:
 */

using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNameController : MonoBehaviour
{
    public enum PlayerType
    {
        NONE = -1,
        PLAYER,
        RIVAL
    }

    [SerializeField]
    private TextMeshProUGUI playerName;

    [SerializeField]
    private PlayerType playerType;

    private void Awake()
    {
        playerName = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        switch (playerType)
        {
            case PlayerType.PLAYER:
                //playerName.color = Color.blue;
                //playerName.text = PlayerInfo.player.playerUserNickName;
                break;
            case PlayerType.RIVAL:
                //playerName.color = Color.red;
                //playerName.text = RivalInfo.rival.playerUserNickName;
                break;
        }
    }
}
