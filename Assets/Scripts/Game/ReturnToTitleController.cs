/**
 * Copyright (C) 2019-2020 CR dot I Co.,Ltd.
 */
/**
 * タイトル：「ゲーム結果表示後、タイトルシーンへ遷移する」スクリプト
 * 
 * 作成情報： DATE:2019/06/18 作成者:中村 鷹広
 * 更新情報： DATE: 作成者:
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToTitleController : MonoBehaviour
{
    public void OnButtonDown()
    {
        Debug.Log("OnButtonDown");
        GameManager.gameStatus = GameManager.GameStatus.END;

        // シーン切り替え
        StartCoroutine(LoadScene("TitleScene", LoadSceneMode.Single));
    }

    private IEnumerator LoadScene(string sceneName, LoadSceneMode mode)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, mode);
    }
}
