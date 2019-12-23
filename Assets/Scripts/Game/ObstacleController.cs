/**
 * Copyright (C) 2019-2020 CR dot I Co.,Ltd.
 */
/**
 * タイトル：「障害物を制御する」スクリプト
 * 
 * 作成情報： DATE:2019/06/17 作成者:中村 鷹広
 * 更新情報： DATE: 作成者:
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public GameObject SpriteObject;
    public GameObject ArrowsSpriteObject;

    private ObstacleCancellationController _obstacleCancellationController;

    //private PhotonView photonView = null;

    private void Awake()
    {
        _obstacleCancellationController = null;
        _obstacleCancellationController = SpriteObject.GetComponent<ObstacleCancellationController>();
    }

    private void Start()
    {
    }

    private void Update()
    {
    }

    /*
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            //データの送信
            stream.SendNext((string)_obstacleCancellationController.obstacleCancelDirection);
        }
        else
        {
            string onetimeReceiveString = (string)stream.ReceiveNext();

            //データの受信
            _obstacleCancellationController.obstacleCancelDirection = onetimeReceiveString;
        }
    }
    */
}
