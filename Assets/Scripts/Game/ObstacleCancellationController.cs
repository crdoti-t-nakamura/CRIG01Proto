/**
 * Copyright (C) 2019-2020 CR dot I Co.,Ltd.
 */
/**
 * タイトル：「障害物の解除を制御する」スクリプト
 * 
 * 作成情報： DATE:2019/06/17 作成者:中村 鷹広
 * 更新情報： DATE: 作成者:
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCancellationController : MonoBehaviour
{
    public enum Players
    {
        NONE = -1,
        PLAYER,
        RIVAL,
    }

    public Players players = Players.NONE;

    private Transform _playerPosTF;
    private Rigidbody2D _playerRB2D;
    private PlayerController _playerController;

    public GameObject _obstacleObject;

    public string obstacleCancelDirection = null;

    public Transform arrowsTF;
    public bool arrowsRotateFlg = false;

    [SerializeField]
    private bool displayFlg = false;
    [SerializeField]
    private bool cancelCompleteFlg = false;

    [SerializeField]
    private Vector3 touchStartPos;
    [SerializeField]
    private Vector3 touchEndPos;

    private void Start()
    {
        players = Players.NONE;
        _playerPosTF = null;
        _playerRB2D = null;
        _playerController = null;
        arrowsRotateFlg = false;
        displayFlg = false;
        cancelCompleteFlg = false;
    }

    private void Update()
    {
        /*
        if (GamePhotonManager.playerInstanceCompleteFlg &&
            _playerPosTF == null &&
            !cancelCompleteFlg)
        */
        if (_playerPosTF == null &&
            !cancelCompleteFlg)
        {
            Debug.Log("_playerPosTF == null");
            GameObject player = null;

            if (gameObject.layer == 8)      // 8:Player
            {
                player = GameObject.FindGameObjectWithTag("Player");
                players = Players.PLAYER;
            }
            else if (gameObject.layer == 9) // 9:Rival
            {
                player = GameObject.FindGameObjectWithTag("Rival");
                players = Players.RIVAL;
            }

            if (player != null)
            {
                Debug.Log(players.ToString() + " != null");
                _playerPosTF = player.transform;
                _playerRB2D = player.GetComponent<Rigidbody2D>();
                _playerController = player.GetComponent<PlayerController>();
            }
        }

        // 矢印の方向修正
        if (!arrowsRotateFlg &&
            obstacleCancelDirection != null &&
            obstacleCancelDirection != "")
        {
            Quaternion q;
            Quaternion parentQ;
            parentQ = Quaternion.Euler(0f, 0f, -_obstacleObject.transform.rotation.z);  // クォータニオン parentQ

            switch (obstacleCancelDirection)
            {
                case "up":
                    // 上フリックされた時の処理
                    q = Quaternion.Euler(0f, 0f, 0f);   // クォータニオン q
                    //arrowsTF.rotation = q * arrowsTF.rotation;
                    arrowsTF.rotation = q * parentQ;
                    arrowsRotateFlg = true;
                    break;

                case "down":
                    // 下フリックされた時の処理
                    q = Quaternion.Euler(0f, 0f, 180f); // クォータニオン q
                    //arrowsTF.rotation = q * arrowsTF.rotation;
                    arrowsTF.rotation = q * parentQ;
                    arrowsRotateFlg = true;
                    break;

                case "right":
                    // 右フリックされた時の処理
                    q = Quaternion.Euler(0f, 0f, 270f); // クォータニオン q
                    //arrowsTF.rotation = q * arrowsTF.rotation;
                    arrowsTF.rotation = q * parentQ;
                    arrowsRotateFlg = true;
                    break;

                case "left":
                    // 左フリックされた時の処理
                    q = Quaternion.Euler(0f, 0f, 90f); // クォータニオン q
                    //arrowsTF.rotation = q * arrowsTF.rotation;
                    arrowsTF.rotation = q * parentQ;
                    arrowsRotateFlg = true;
                    break;
            }
        }

        if (players == Players.PLAYER &&
            _playerPosTF != null &&
            !cancelCompleteFlg)
        {
            if (_playerPosTF.position.x <= transform.position.x)
            {
                if (displayFlg)
                {
                    // フリック入力受付
                    Flick();
                }
            }
            /*
            else
            {
                displayFlg = false;
            }
            */
        }
    }
    private void OnWillRenderObject()
    {
        // PlayerCamera内に映っている場合
        if (players == Players.PLAYER &&
            Camera.current.name == "PlayerCamera" &&
            !cancelCompleteFlg)
        {
            displayFlg = true;
        }

        if (players == Players.PLAYER &&
            _playerPosTF != null &&
            !cancelCompleteFlg)
        {
            if (transform.position.x < _playerPosTF.position.x)
            {
                displayFlg = false;
                cancelCompleteFlg = true;
            }
        }
    }

    private void OnBecameVisible()
    {

    }

    private void OnBecameInvisible()
    {
        displayFlg = false;
        touchStartPos = Vector3.zero;
        touchEndPos = Vector3.zero;
        cancelCompleteFlg = true;
    }

    private void Flick()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            touchStartPos = new Vector3(Input.mousePosition.x,
                                        Input.mousePosition.y,
                                        Input.mousePosition.z);
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            touchEndPos = new Vector3(Input.mousePosition.x,
                                      Input.mousePosition.y,
                                      Input.mousePosition.z);
            GetDirection();
        }
    }

    private void GetDirection()
    {
        float directionX = touchEndPos.x - touchStartPos.x;
        float directionY = touchEndPos.y - touchStartPos.y;
        string Direction = "";

        if (Mathf.Abs(directionY) < Mathf.Abs(directionX))
        {
            if (30 < directionX)
            {
                //右向きにフリック
                Direction = "right";
            }
            else if (-30 > directionX)
            {
                //左向きにフリック
                Direction = "left";
            }
        }
        else if (Mathf.Abs(directionX) < Mathf.Abs(directionY))
        {
            if (30 < directionY)
            {
                //上向きにフリック
                Direction = "up";
            }
            else if (-30 > directionY)
            {
                //下向きのフリック
                Direction = "down";
            }
            else
            {
                //タッチを検出
                Direction = "touch";
            }
        }

        if (obstacleCancelDirection == Direction)
        {
            Debug.Log("解除成功");
            displayFlg = false;
            cancelCompleteFlg = true;

            _obstacleObject.SetActive(false);
        }

        /*
        switch (Direction)
        {
            case "up":
                //上フリックされた時の処理
                Debug.Log(Direction);
                break;

            case "down":
                //下フリックされた時の処理
                Debug.Log(Direction);
                break;

            case "right":
                //右フリックされた時の処理
                Debug.Log(Direction);
                break;

            case "left":
                //左フリックされた時の処理
                Debug.Log(Direction);
                break;

            case "touch":
                //タッチされた時の処理
                Debug.Log(Direction);
                break;
        }
        */
    }

    // オブジェクトと接触した時のみ呼ばれるコールバック
    public void OnTriggerEnter2D(Collider2D hit)
    {
        Debug.Log("OnTriggerEnter2D");

        if (players == Players.PLAYER &&
            hit.gameObject.tag == "Player")
        {
            //_playerController.CenterWheelRB2D.velocity;

            // 現在の速度で逆方向に力を加える
            /*
            PlayerController.GetComponent<Rigidbody2D>().velocity =
                new Vector2(
                    PlayerController.GetComponent<Rigidbody2D>().transform.right.x *
                    -PlayerController.GetComponent<Rigidbody2D>().velocity.magnitude, 0);
                    */

            _playerRB2D.velocity =
                new Vector2(_playerPosTF.right.x * -_playerRB2D.velocity.magnitude, 0);

            _playerController.RightWheelRB2D.
                AddTorque(_playerRB2D.velocity.magnitude * 1.25f, ForceMode2D.Impulse);
            _playerController.LeftWheelRB2D.
                AddTorque(_playerRB2D.velocity.magnitude * 1.25f, ForceMode2D.Impulse);

        }
    }
}
