/**
 * Copyright (C) 2019-2020 CR dot I Co.,Ltd.
 */
/**
 * タイトル：「ゲームシーンにて、プレイヤーを制御する」スクリプト
 * 
 * 作成情報： DATE:2019/06/16 作成者:中村 鷹広
 * 更新情報： DATE: 作成者:
 */

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]             // Rigidbody2Dアタッチ確認
public class PlayerController : MonoBehaviour
{
    public enum PlayerStatus
    {
        NONE = -1,
        WAIT,           // 待機
        READY,          // 準備
        RUN,            // 走る
        CONTACT,        // 接触
        GOAL,           // ゴール
        END             // 終了
    }

    public enum ItemPlayStatus
    {
        NONE = -1,
        UNUSED,         // 未使用
        AVAILABLE,      // 使用可能
        USING,          // 使用中
        UNAVAILABLE     // 使用済
    }

    public enum GoalResultStatus
    {
        NONE = -1,
        NONGOAL,        // 未ゴール
        WIN,            // 勝利
        LOSE,           // 敗北
        DRAW            // 引分
    }

    public bool GravityFlg = true;

    public Rigidbody2D RightWheelRB2D;
    public Rigidbody2D CenterWheelRB2D;
    public Rigidbody2D LeftWheelRB2D;

    public WheelJoint2D RightWheel;
    public WheelJoint2D LeftWheel;

    public WheelController RightWheelController;

    public CircleCollider2D RightWheelCollider;
    public CircleCollider2D LeftWheelCollider;
    public Transform RightWheelTF;
    public Transform LeftWheelTF;

    public RunEffectsController runEffectsController;

    private Rigidbody2D playerRB2D = null;

    private JointMotor2D playerJM2DR = new JointMotor2D();
    private JointMotor2D playerJM2DL = new JointMotor2D();

    [Range(1.0f, 8.0f)] public float TorqueExponent = 4.0f; // 大きくするほどトルクカーブが鋭くなる
    [Range(0.0f, 1000.0f)] public float TorqueMagnitudeMax = 100.0f; // 回転押し戻しトルクの最大値
    [Range(0.0f, 180.0f)] public float RotationLimit = 90.0f; // 回転角の限界値

    [SerializeField]
    private PlayerStatus playerStatus = PlayerStatus.NONE;
    [SerializeField]
    private ItemPlayStatus itemPlayStatus = ItemPlayStatus.NONE;

    [SerializeField]
    private GoalResultStatus goalResultStatus = GoalResultStatus.NONE;

    [SerializeField]
    private float addForce = 10.0f;
    [SerializeField]
    private float addTorque = -0.3f;

    [SerializeField]
    private float gravityDistance = 1.0f;

    [SerializeField]
    private float wheelGravityPower = 15.0f;

    [SerializeField]
    private Transform _goalPosTF = null;
    [SerializeField]
    private float goalDistance = 0.0f;

    //[SerializeField]
    //private Item _selectItem = null;
    [SerializeField]
    private Vector2 itemMaxVelocity = new Vector2(0.0f, 0.0f);

    public int playerItemUsableCount = 0;

    private float itemDurationTimer = 0.0f;

    [SerializeField]
    private float seconds = 0.0f;
    private float oldSeconds = 0.0f;

    // 補完スピードを決める
    [SerializeField]
    private float speed = 0.1f;

    public TextMeshProUGUI displayPlayerStatus;
    public TextMeshProUGUI displayPlayerSpeedText;

    [SerializeField]
    private Vector2 playerVelocity = Vector2.zero;
    [SerializeField]
    private Vector2 rightWheelVelocity = Vector2.zero;
    [SerializeField]
    private Vector2 leftWheelVelocity = Vector2.zero;

    private void Awake()
    {
        playerRB2D = GetComponent<Rigidbody2D>();
        playerJM2DR = new JointMotor2D();
        playerJM2DL = new JointMotor2D();
    }

    private void Start()
    {
        playerItemUsableCount = 0;
        playerStatus = PlayerStatus.WAIT;
        goalResultStatus = GoalResultStatus.NONGOAL;
        //_selectItem = PlayerInfo.player.playerSelectItem;
        //playerItemUsableCount = _selectItem.itemUsableCount;
        ResetItemMaxVelocity();

        itemPlayStatus = ItemPlayStatus.UNUSED;

        playerJM2DR.motorSpeed = 0.0f;
        playerJM2DL.motorSpeed = 0.0f;

        playerVelocity = Vector2.zero;
        rightWheelVelocity = Vector2.zero;
        leftWheelVelocity = Vector2.zero;
        
        goalDistance = 0.0f;
        itemDurationTimer = 0.0f;
        ResetTimer();
    }

    private void Update()
    {
        if (_goalPosTF == null && GoalController.initializeFlg)
        {
            GameObject goalPos = GameObject.FindGameObjectWithTag("GoalMaps");

            if (goalPos != null)
            {
                _goalPosTF = goalPos.transform;
            }
        }

        // 以降、デバッグ用 ====================================================================
        /*
        if (displayPlayerStatus == null &&
            gameObject.name == "Player")
        */
        if (displayPlayerStatus == null)
        {
            // プレイヤーの場合
            GameObject displayStatusObject = GameObject.FindGameObjectWithTag("PlayerStatus");
            if (displayStatusObject != null)
            {
                displayPlayerStatus = displayStatusObject.GetComponent<TextMeshProUGUI>();
            }
        }
        if (displayPlayerSpeedText == null)
        {
            // プレイヤーの場合
            GameObject displaySpeedText = GameObject.FindGameObjectWithTag("SpeedText");
            if (displaySpeedText != null)
            {
                displayPlayerSpeedText = displaySpeedText.GetComponent<TextMeshProUGUI>();
            }
        }
        /*
        if (displayPlayerStatus == null &&
            gameObject.name == "Rival")
        {
            // ライバルの場合
            GameObject displayStatusObject = GameObject.FindGameObjectWithTag("RivalStatus");
            if (displayStatusObject != null)
            {
                displayPlayerStatus = displayStatusObject.GetComponent<TextMeshProUGUI>();
            }
        }
        */

        if (displayPlayerStatus != null)
        {
            DebugDisplayPlayerStatus();
        }
        // =====================================================================================
        if (displayPlayerSpeedText != null)
        {
            DisplayPlayerSpeed();
        }
    }

    private void FixedUpdate()
    {
        switch (playerStatus)
        {
            case PlayerStatus.WAIT:
                // 待機中
                //playerRB2D.velocity = Vector2.zero;
                RightWheel.useMotor = false;
                LeftWheel.useMotor = false;
                break;
            case PlayerStatus.READY:
                // 準備中
                break;
            case PlayerStatus.RUN:
                // 滑走中
                // TODO 仮
                /*
                playerRB2D.AddForce(playerRB2D.velocity * addForce, ForceMode2D.Force);
                RightWheelRB2D.AddTorque(addTorque, ForceMode2D.Force);
                LeftWheelRB2D.AddTorque(addTorque, ForceMode2D.Force);
                if (CenterWheelRB2D != null)
                {
                    CenterWheelRB2D.AddTorque(addTorque, ForceMode2D.Force);
                }
                */
                RightWheel.useMotor = true;
                LeftWheel.useMotor = true;
                playerJM2DR.motorSpeed = RightWheel.motor.motorSpeed;
                playerJM2DR.maxMotorTorque = RightWheel.motor.maxMotorTorque;
                playerJM2DL.motorSpeed = LeftWheel.motor.motorSpeed;
                playerJM2DL.maxMotorTorque = LeftWheel.motor.maxMotorTorque;
                TimeCounter.isPose = false;
                break;
            case PlayerStatus.CONTACT:
                // 接触
                break;
            case PlayerStatus.GOAL:
                // ゴール
                /*
                if (0.0f < RightWheel.motor.motorSpeed)
                {
                    playerJM2DR.motorSpeed = -RightWheel.motor.motorSpeed / 2.0f;
                    RightWheel.motor = playerJM2DR;
                }
                else if (RightWheel.motor.motorSpeed <= 0.0f)
                {
                    playerJM2DR.motorSpeed = 0.0f;
                    RightWheel.motor = playerJM2DR;
                    RightWheel.useMotor = false;
                }
                if (0.0f < LeftWheel.motor.motorSpeed)
                {
                    playerJM2DL.motorSpeed = -LeftWheel.motor.motorSpeed / 2.0f;
                    LeftWheel.motor = playerJM2DL;
                }
                else if (LeftWheel.motor.motorSpeed <= 0.0f)
                {
                    playerJM2DL.motorSpeed = 0.0f;
                    LeftWheel.motor = playerJM2DL;
                    LeftWheel.useMotor = false;
                }
                */
                RightWheel.useMotor = false;
                LeftWheel.useMotor = false;
                TimeCounter.isPose = true;
                if (0.5f < RightWheel.attachedRigidbody.velocity.magnitude)
                {
                    RightWheel.attachedRigidbody.AddForce(Vector2.left * RightWheel.attachedRigidbody.velocity.magnitude * 2.0f);
                }
                else
                {
                    RightWheel.attachedRigidbody.velocity = Vector2.zero;
                }
                if (0.5f < LeftWheel.attachedRigidbody.velocity.magnitude)
                {
                    LeftWheel.attachedRigidbody.AddForce(Vector2.left * LeftWheel.attachedRigidbody.velocity.magnitude * 2.0f);
                }
                else
                {
                    LeftWheel.attachedRigidbody.velocity = Vector2.zero;
                }
                break;
            case PlayerStatus.END:
                // 終了
                RightWheel.useMotor = false;
                LeftWheel.useMotor = false;
                playerRB2D.velocity = Vector2.zero;
                // 
                playerRB2D.simulated = false;
                RightWheelRB2D.simulated = false;
                LeftWheelRB2D.simulated = false;
                // 重力の停止
                playerRB2D.isKinematic = true;
                RightWheelRB2D.isKinematic = true;
                LeftWheelRB2D.isKinematic = true;
                break;
        }

        /*
        if (GravityFlg)
        {
            // 重力調整
            Gravity();
        }

        // velocity限界値調整
        VelocityLimitValueAdjust();
        */

        // 回転制限
        RotationLimitter();

        if (0.0f < itemDurationTimer)
        {
            seconds += Time.deltaTime;

            if (itemDurationTimer <= seconds)
            {
                ResetItemMaxVelocity();

                itemDurationTimer = 0.0f;
                if (0 < playerItemUsableCount)
                {
                    itemPlayStatus = ItemPlayStatus.AVAILABLE;
                }
                else
                {
                    itemPlayStatus = ItemPlayStatus.UNAVAILABLE;
                }

                ResetTimer();
            }
        }

        playerVelocity = playerRB2D.velocity;
        rightWheelVelocity = RightWheelRB2D.velocity;
        leftWheelVelocity = LeftWheelRB2D.velocity;

        if (RightWheelController.IsGround &&
            1.0f <= RightWheelRB2D.velocity.x)
        {
            // レイがコライダーの表面に衝突したワールド座標での地点に
            // 砂埃のようなエフェクトを表示
            runEffectsController.PlayParticle();
        }
        else
        {
            runEffectsController.StopParticle();
        }

        if (_goalPosTF != null)
        {
            // ゴールまでの距離を取得
            GetGoalDistance();
        }
    }

    private void Gravity()
    {
        int layerMask = LayerMask.GetMask(new string[] { "Maps" });
        Ray2D ray = new Ray2D(transform.localPosition, -transform.up);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, gravityDistance, layerMask);

        Ray2D rightWheelRay = new Ray2D(RightWheelTF.position, -transform.up);
        RaycastHit2D rightWheelHit = Physics2D.Raycast(rightWheelRay.origin, rightWheelRay.direction, gravityDistance, layerMask);
        Ray2D leftWheelRay = new Ray2D(LeftWheelTF.position, -transform.up);
        RaycastHit2D leftWheelHit = Physics2D.Raycast(leftWheelRay.origin, leftWheelRay.direction, gravityDistance, layerMask);
        
        if (rightWheelHit.collider != null)
        {
            /*
            if (RightWheelController.IsGround &&
                0.5f <= RightWheelRB2D.velocity.x)
            {
                // レイがコライダーの表面に衝突したワールド座標での地点に
                // 砂埃のようなエフェクトを表示
                runEffectsController.PlayParticle();
            }
            else
            {
                runEffectsController.StopParticle();
            }
            */
        }

        //Rayの表示時間
        float RAY_DISPLAY_TIME = 3;

        //衝突時のRayを画面に表示
        if (hit.collider)
        {
            Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.blue, RAY_DISPLAY_TIME, false);
        }
        //非衝突時のRayを画面に表示
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * gravityDistance, Color.green, RAY_DISPLAY_TIME, false);
        }

        //衝突時のRayを画面に表示
        if (rightWheelHit.collider)
        {
            Debug.DrawRay(rightWheelRay.origin, rightWheelHit.point - rightWheelRay.origin, Color.blue, RAY_DISPLAY_TIME, false);
        }
        //非衝突時のRayを画面に表示
        else
        {
            Debug.DrawRay(rightWheelRay.origin, rightWheelRay.direction * gravityDistance, Color.green, RAY_DISPLAY_TIME, false);
        }

        //衝突時のRayを画面に表示
        if (leftWheelHit.collider)
        {
            Debug.DrawRay(leftWheelRay.origin, leftWheelHit.point - leftWheelRay.origin, Color.blue, RAY_DISPLAY_TIME, false);
        }
        //非衝突時のRayを画面に表示
        else
        {
            Debug.DrawRay(leftWheelRay.origin, leftWheelRay.direction * gravityDistance, Color.green, RAY_DISPLAY_TIME, false);
        }
    }

    private void VelocityLimitValueAdjust()
    {
        /*
        float maxVelocityX = maxVelocity.x + itemMaxVelocity.x;
        float maxVelocityY = maxVelocity.y + itemMaxVelocity.y;

        if (maxVelocityX < playerRB2D.velocity.x)
        {
            playerRB2D.velocity = new Vector2(maxVelocityX, playerRB2D.velocity.y);
        }
        if (maxVelocityY < playerRB2D.velocity.y)
        {
            playerRB2D.velocity = new Vector2(playerRB2D.velocity.x, maxVelocityY);
        }
        if (playerRB2D.velocity.x < minVelocity.x)
        {
            playerRB2D.velocity = new Vector2(minVelocity.x, playerRB2D.velocity.y);
        }
        if (playerRB2D.velocity.y < minVelocity.y)
        {
            playerRB2D.velocity = new Vector2(playerRB2D.velocity.x, minVelocity.y);
        }

        if (maxVelocityX < RightWheelRB2D.velocity.x)
        {
            RightWheelRB2D.velocity = new Vector2(maxVelocityX, RightWheelRB2D.velocity.y);
        }
        if (maxVelocityY < RightWheelRB2D.velocity.y)
        {
            RightWheelRB2D.velocity = new Vector2(RightWheelRB2D.velocity.x, maxVelocityY);
        }
        if (RightWheelRB2D.velocity.x < minVelocity.x)
        {
            RightWheelRB2D.velocity = new Vector2(minVelocity.x, RightWheelRB2D.velocity.y);
        }
        if (RightWheelRB2D.velocity.y < minVelocity.y)
        {
            RightWheelRB2D.velocity = new Vector2(RightWheelRB2D.velocity.x, minVelocity.y);
        }

        if (CenterWheelRB2D != null)
        {
            if (maxVelocityX < CenterWheelRB2D.velocity.x)
            {
                CenterWheelRB2D.velocity = new Vector2(maxVelocityX, CenterWheelRB2D.velocity.y);
            }
            if (maxVelocityY < RightWheelRB2D.velocity.y)
            {
                CenterWheelRB2D.velocity = new Vector2(CenterWheelRB2D.velocity.x, maxVelocityY);
            }
            if (CenterWheelRB2D.velocity.x < minVelocity.x)
            {
                CenterWheelRB2D.velocity = new Vector2(minVelocity.x, CenterWheelRB2D.velocity.y);
            }
            if (CenterWheelRB2D.velocity.y < minVelocity.y)
            {
                CenterWheelRB2D.velocity = new Vector2(CenterWheelRB2D.velocity.x, minVelocity.y);
            }
        }

        if (maxVelocityX < LeftWheelRB2D.velocity.x)
        {
            LeftWheelRB2D.velocity = new Vector2(maxVelocityX, LeftWheelRB2D.velocity.y);
        }
        if (maxVelocityY < LeftWheelRB2D.velocity.y)
        {
            LeftWheelRB2D.velocity = new Vector2(LeftWheelRB2D.velocity.x, maxVelocityY);
        }
        if (LeftWheelRB2D.velocity.x < minVelocity.x)
        {
            LeftWheelRB2D.velocity = new Vector2(minVelocity.x, LeftWheelRB2D.velocity.y);
        }
        if (LeftWheelRB2D.velocity.y < minVelocity.y)
        {
            LeftWheelRB2D.velocity = new Vector2(LeftWheelRB2D.velocity.x, minVelocity.y);
        }
        */
    }

    private void GetGoalDistance()
    {
        // ゴールまでの距離を取得
        Vector2 pos = transform.position;
        Vector2 goalPos = _goalPosTF.position;
        goalDistance = Vector2.Distance(pos, goalPos);
    }

    private void RotationLimitter()
    {
        // 現在の回転の大きさと向き
        var rotationMagnitude = playerRB2D.rotation;
        var rotationSign = Mathf.Sign(rotationMagnitude);
        rotationMagnitude *= rotationSign;

        // 回転が限界角に近づくほど1に近づくカーブを作る
        //
        // factor
        // 1|                 *********
        //  |                 *
        //  |                *
        //  |               *
        //  |             **
        //  |          ***
        //  |      ****
        //  |******
        // 0+-------------------------- rotationMagnitude
        //  0                 90
        var factor = Mathf.Pow(Mathf.InverseLerp(0.0f, this.RotationLimit, rotationMagnitude), this.TorqueExponent);

        // 回転と逆向きにトルクをかける
        var torque = -rotationSign * this.TorqueMagnitudeMax * factor;
        playerRB2D.AddTorque(torque);
    }

    public void ResetVelocity()
    {
        playerRB2D.velocity = Vector2.zero;
        RightWheelRB2D.velocity = Vector2.zero;
        LeftWheelRB2D.velocity = Vector2.zero;
        if (CenterWheelRB2D != null)
        {
            CenterWheelRB2D.velocity = Vector2.zero;
        }
    }

    public void ChangePlayerStatusReady()
    {
        playerStatus = PlayerStatus.READY;
    }
    public void ChangePlayerStatusRun()
    {
        playerStatus = PlayerStatus.RUN;
    }
    public void ChangePlayerStatusContact()
    {
        playerStatus = PlayerStatus.CONTACT;
    }
    public void ChangePlayerStatusGoal()
    {
        playerStatus = PlayerStatus.GOAL;
    }

    public PlayerStatus GetPlayerStatus()
    {
        return playerStatus;
    }

    public void ChangeGoalResultStatusWin()
    {
        goalResultStatus = GoalResultStatus.WIN;
    }
    public void ChangeGoalResultStatusLose()
    {
        goalResultStatus = GoalResultStatus.LOSE;
    }
    public void ChangeGoalResultStatusDraw()
    {
        goalResultStatus = GoalResultStatus.DRAW;
    }

    public GoalResultStatus GetGoalResultStatus()
    {
        return goalResultStatus;
    }

    private void DebugDisplayPlayerStatus()
    {
        string velocityChenterWheelRB2DText = "";
        string goalDistanceText = "";
        string selectItemText = "";

        if (CenterWheelRB2D != null)
        {
            velocityChenterWheelRB2DText = "\n" + "velocity(CenterWheel) : " + CenterWheelRB2D.velocity.ToString();
        }
        if (goalDistance != 0.0f)
        {
            goalDistanceText = "\n" + "goalDistance : " + goalDistance.ToString();
        }
        /*
        if (_selectItem != null)
        {
            selectItemText = "\n" + "_selectItem : " + _selectItem.itemName.ToString();
        }
        */

        displayPlayerStatus.text =
            "addTorque : " + addTorque.ToString() + "\n" +
            "playerStatus : " + playerStatus.ToString() + "\n" +
            "goalResultStatus : " + goalResultStatus.ToString() + "\n" +
            "velocity(Body) : " + playerRB2D.velocity.ToString() + "\n" +
            "velocity(RightWheel) : " + RightWheelRB2D.velocity.ToString() + "\n" +
            "velocity(LeftWheel) : " + LeftWheelRB2D.velocity.ToString() + "\n" +
            "itemPlayStatus : " + itemPlayStatus.ToString() +
            velocityChenterWheelRB2DText +
            goalDistanceText +
            selectItemText
            ;
    }

    private void DisplayPlayerSpeed()
    {
        displayPlayerSpeedText.text = playerRB2D.velocity.magnitude.ToString("f1");
    }

    public void SetItemDurationTimer(float itemDurationTimer)
    {
        this.itemDurationTimer = itemDurationTimer;
        itemPlayStatus = ItemPlayStatus.USING;
    }
    public void SetItemMaxVelocity(float itemMaxVelocityX, float itemMaxVelocityY)
    {
        this.itemMaxVelocity = new Vector2(itemMaxVelocityX, itemMaxVelocityY);
    }
    private void ResetItemMaxVelocity()
    {
        itemMaxVelocity = new Vector2(0.0f, 0.0f);
    }
    public void SetItemVelocityXImpulse(float itemMaxVelocityX)
    {
        RightWheelRB2D.AddForce(RightWheelRB2D.velocity * 150.0f, ForceMode2D.Impulse);
        LeftWheelRB2D.AddForce(LeftWheelRB2D.velocity * 150.0f, ForceMode2D.Impulse);

        //RightWheelRB2D.AddForce(transform.right * 150.0f, ForceMode2D.Impulse);
        //LeftWheelRB2D.AddForce(transform.right * 150.0f, ForceMode2D.Impulse);
        //RightWheelRB2D.AddForce(-transform.up * 2.5f, ForceMode2D.Impulse);
        //LeftWheelRB2D.AddForce(-transform.up * 2.5f, ForceMode2D.Impulse);
        RightWheelRB2D.AddTorque(-itemMaxVelocityX * 15.0f, ForceMode2D.Impulse);
        LeftWheelRB2D.AddTorque(-itemMaxVelocityX * 15.0f, ForceMode2D.Impulse);
        //playerRB2D.AddForce(-transform.up * 1.5f, ForceMode2D.Impulse);
        //playerRB2D.AddForce(transform.right * 150.0f, ForceMode2D.Impulse);
    }
    public void SetItemVelocityYImpulse(float itemMaxVelocityY)
    {
        playerRB2D.AddForce(transform.up * itemMaxVelocityY, ForceMode2D.Impulse);
        this.itemMaxVelocity = new Vector2(itemMaxVelocityY / 20.0f, 0.0f);
        playerRB2D.AddTorque(itemMaxVelocityY / 7.5f, ForceMode2D.Impulse);
    }
    public void ResetTimer()
    {
        seconds = 0.0f;
        oldSeconds = 0.0f;
    }

    /*
    public void UseVisualDisturbance(float itemDuration)
    {
        // RPCメソッドの引数 object[] の配列にする
        object[] args = new object[]{
            itemDuration,           // 第1引数 : float itemDuration
        };

        photonView.RPC("UsedByOpponentVisualDisturbance", PhotonTargets.Others, args);
    }

    [PunRPC]
    private void UsedByOpponentVisualDisturbance(float itemDuration)
    {
        VisualDisturbanceController.playFlg = true;
        VisualDisturbanceController.playTimer = itemDuration;
    }
    */
}
