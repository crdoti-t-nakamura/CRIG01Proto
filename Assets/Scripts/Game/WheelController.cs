/**
 * Copyright (C) 2019-2020 CR dot I Co.,Ltd.
 */
/**
 * タイトル：「ホイールの接地判定」スクリプト
 * 
 * 作成情報： DATE:2019/06/17 作成者:中村 鷹広
 * 更新情報： DATE: 作成者:
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    public bool IsGround = false;

    private void Start()
    {
        IsGround = false;
    }

    private void Update()
    {

    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log("OnTriggerStay2D");

        if (collision.gameObject.tag == "Ground")
        {
            IsGround = true;
            //Debug.Log("IsGround:" + IsGround.ToString());
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log("OnTriggerExit2D");

        if (collision.gameObject.tag == "Ground")
        {
            IsGround = false;
            //Debug.Log("IsGround:" + IsGround.ToString());
        }
    }
}
