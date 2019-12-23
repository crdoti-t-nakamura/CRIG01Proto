/**
 * Copyright (C) 2019-2020 CR dot I Co.,Ltd.
 */
/**
 * タイトル：「ゲームマネージャー」スクリプト
 * 
 * 作成情報： DATE:2019/06/18 作成者:中村 鷹広
 * 更新情報： DATE: 作成者:
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // ゲームステータス
    public enum GameStatus
    {
        NONE = -1,
        WAIT,           // 待機
        READY,          // 準備
        COUNTDOWN,
        RUN,            // 走る
        GOAL,           // ゴール
        END,            // 終了
        LINEDISCONNECT  // 回線切断
    }

    public static GameStatus gameStatus;
    [SerializeField]
    private GameStatus debugGameStatus;

    private GameObject _player = null;
    private PlayerController _playerController = null;

    public static bool playerGetObjectCompleteFlg = false;

    [SerializeField]
    // プレイヤーが出現する位置
    private Vector3 initPos = new Vector3(-1.7500f, 1.0000f, 0.0000f);

    public GameObject templatePlayerSnowyMt;
    public SpeedMeteWindowController _speedMeteWindowController;
    public ItemButtonWindowController _itemButtonWindowController;
    public ScoreTimeWindowController _scoreTimeWindowController;

    public BoxCollider2D _gamePlayArea;

    void Start()
    {
        gameStatus = GameStatus.WAIT;
        debugGameStatus = gameStatus;

        _player = null;
        _playerController = null;

        playerGetObjectCompleteFlg = false;
    }

    void Update()
    {
        switch (gameStatus)
        {
            case GameStatus.WAIT:
                // 待機中
                // 自分のキャラとマップが生成された場合
                // キャラ情報等を取得
                if (!playerGetObjectCompleteFlg)
                {
                    _player = Instantiate(templatePlayerSnowyMt, initPos, Quaternion.identity);
                    _player.tag = "Player";
                    _player.name = "Player";
                    _player.layer = 8;         // 8:Player

                    _playerController = _player.GetComponent<PlayerController>();

                    // ステータスをREADYに変更
                    gameStatus = GameStatus.READY;

                    playerGetObjectCompleteFlg = true;
                }
                break;
            case GameStatus.READY:
                // 準備中
                if (_playerController.GetPlayerStatus() == PlayerController.PlayerStatus.WAIT)
                {
                    // プレイヤーのステータスをREADY状態に変更
                    _playerController.ChangePlayerStatusReady();
                }
                if (_playerController.GetPlayerStatus() == PlayerController.PlayerStatus.READY)
                {
                    // ステータスをCOUNTDOWN状態に変更
                    gameStatus = GameStatus.COUNTDOWN;
                }
                break;
            case GameStatus.COUNTDOWN:
                // カウントダウン
                // ステータス変更同期

                // カウントダウン開始
                if (!CountDownUIController.playFlg &&
                    !CountDownUIController.isPlay &&
                    !CountDownUIController.isComplete)
                {
                    CountDownUIController.playFlg = true;
                }

                // カウントダウン終了後
                if (CountDownUIController.isComplete)
                {
                    // 完了後、RUNに変更
                    gameStatus = GameStatus.RUN;
                }
                break;
            case GameStatus.RUN:
                if (_playerController.GetPlayerStatus() == PlayerController.PlayerStatus.READY)
                {
                    // スピードメーターUI表示
                    _speedMeteWindowController.ChangeDisplay();
                    // アイテムボタンUI表示
                    _itemButtonWindowController.ChangeDisplay();
                    // プレイヤーのステータスをRUN状態に変更
                    _playerController.ChangePlayerStatusRun();
                }
                // 滑走中
                break;
            case GameStatus.GOAL:
                // ゴール
                // ステータス変更同期

                // カウントダウン開始
                if (!GoalUIController.playFlg &&
                    !GoalUIController.isPlay &&
                    !GoalUIController.isComplete)
                {
                    GoalUIController.playFlg = true;
                    // スピードメーターUI非表示
                    _speedMeteWindowController.ChangeDisplay();
                    // アイテムボタンUI非表示
                    _itemButtonWindowController.ChangeDisplay();
                }

                // カウントダウン終了後
                if (GoalUIController.isComplete)
                {
                    // 完了後、リザルトキャンバスをアクティブ
                    if (!ResultCanvasController.playFlg &&
                        !ResultCanvasController.isPlay &&
                        !ResultCanvasController.isComplete)
                    {
                        ResultCanvasController.playFlg = true;
                        if (_playerController != null)
                        {
                            _playerController.ResetVelocity();
                        }
                    }
                }
                break;
            case GameStatus.END:
                // 終了
                Caching.ClearCache();
                Resources.UnloadUnusedAssets();
                break;
        }
        debugGameStatus = gameStatus;
    }


}
