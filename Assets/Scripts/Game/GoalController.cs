/**
 * Copyright (C) 2019-2020 CR dot I Co.,Ltd.
 */
/**
 * タイトル：「ゴール」スクリプト
 * 
 * 作成情報： DATE:2019/06/16 作成者:中村 鷹広
 * 更新情報： DATE: 作成者:
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    // ステート
    public enum GOAL_STATE
    {
        NONE = -1,
        NONGOAL,
        GOAL
    }

    public static bool initializeFlg;
    public GOAL_STATE _goalState = GOAL_STATE.NONE;

    private PlayerController _playerController;
    //private PlayerController _rivalController;

    private GameObject _goalPos;

    [SerializeField]    // マッチング成功時の各アニメーション終了後、待機する時間(秒)
    private float intervalTime = 3.0f;

    private void Start()
    {
        initializeFlg = false;
        _goalState = GOAL_STATE.NONGOAL;

        _playerController = null;
        //_rivalController = null;
        _goalPos = null;
    }

    private void Update()
    {
        if (_playerController == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                _playerController = player.GetComponent<PlayerController>();
            }
        }
        /*
        if (_rivalController == null)
        {
            GameObject rival = GameObject.FindGameObjectWithTag("Rival");

            if (rival != null)
            {
                _rivalController = rival.GetComponent<PlayerController>();
            }
        }
        */
        if (_goalPos == null)
        {
            _goalPos = GameObject.FindGameObjectWithTag("GoalMaps");
        }

        if (!initializeFlg &&
            _goalPos != null)
        {
            transform.position = _goalPos.transform.position;
        }

        /*
        if (!initializeFlg &&
            (_playerController == null || _rivalController == null))
        */
        if (!initializeFlg && _playerController == null)
        {
            return;
        }
        else
        {
            initializeFlg = true;
        }

        // ゴールした場合の制御
        if (_goalState == GOAL_STATE.GOAL)
        {
            StartCoroutine(Interval(intervalTime));

            // リザルトキャンバスをアクティブ
            //ResultCanvasController.playFlg = true;
            //_playerController.ResetVelocity();
        }
    }

    // オブジェクトと接触した時に呼ばれるコールバック
    public void OnTriggerEnter2D(Collider2D hit)
    {
        Debug.Log("OnTriggerEnter2D");

        if (initializeFlg)
        {
            // 接触したオブジェクトのタグが"Player"の場合
            if (hit.gameObject.tag == "Player")
            {
                if (_goalState == GOAL_STATE.NONGOAL)
                {
                    // Playerの勝ち
                    Debug.Log("PlayerWin");
                    _playerController.ChangeGoalResultStatusWin();
                    //_rivalController.ChangeGoalResultStatusLose();

                    // 各種ステータスをゴールへ変更
                    _goalState = GOAL_STATE.GOAL;
                    _playerController.ChangePlayerStatusGoal();
                    //_rivalController.ChangePlayerStatusGoal();

                    // GamePhotonManagerのゲームステータスをゴールへ変更
                    GameManager.gameStatus = GameManager.GameStatus.GOAL;

                    // _goalStateの同期
                    /*
                    photonView.RPC("ChangeGoalState", PhotonTargets.AllViaServer);
                    photonView.RPC("ChangePlayerState", PhotonTargets.Others);
                    */
                }
                /*
                else if (_goalState == GOAL_STATE.GOAL)
                {
                    // Playerの負け
                    Debug.Log("PlayerLose");
                    _playerController.ChangeGoalResultStatusLose();
                    _rivalController.ChangeGoalResultStatusWin();

                    // 各種ステータスをゴールへ変更
                    _goalState = GOAL_STATE.GOAL;
                    _playerController.ChangePlayerStatusGoal();
                    _rivalController.ChangePlayerStatusGoal();
                }
                */
            }
            /*
            else if (hit.gameObject.name == "Rival")
            {
                _rivalController.ChangePlayerStatusGoal();
            }
            */
        }
        else
        {
            Debug.Log(name + "が初期化されていません。");
        }
    }

    /*
    void OnPhotonSerializeView(PhotonStream i_stream, PhotonMessageInfo i_info)
    {
        if (i_stream.isWriting)
        {
            //データの送信
            i_stream.SendNext((int)_goalState);
        }
        else
        {
            //データの受信
            int receiveGoalStateNo = (int)i_stream.ReceiveNext();
            _goalState = (GOAL_STATE)Enum.ToObject(typeof(GOAL_STATE), receiveGoalStateNo);
        }
    }

    [PunRPC]
    private void ChangeGoalState()
    {
        Debug.Log("RPC:ChangeGoalState");
        _goalState = GOAL_STATE.GOAL;
        // GamePhotonManagerのゲームステータスをゴールへ変更
        GamePhotonManager.gameStatus = GamePhotonManager.GameStatus.GOAL;
        _playerController.ChangePlayerStatusGoal();
    }

    [PunRPC]
    private void ChangePlayerState()
    {
        Debug.Log("RPC:ChangePlayerState");
        _playerController.ChangeGoalResultStatusLose();
    }
    */

    private IEnumerator Interval(float interval)
    {
        yield return new WaitForSeconds(interval);
    }
}
