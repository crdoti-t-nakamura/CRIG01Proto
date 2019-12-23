/**
 * Copyright (C) 2019-2020 CR dot I Co.,Ltd.
 */
/**
 * タイトル：「プレイヤーのアイテム使用時の制御」スクリプト
 * 
 * 作成情報： DATE:2019/06/19 作成者:中村 鷹広
 * 更新情報： DATE: 作成者:
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]                  // Buttonアタッチ確認
public class PlayerItemController : MonoBehaviour
{
    [SerializeField]
    private Button _itemButton;

    /*
    public Image _itemIconImage;
    public GameObject _itemNoItemText;
    public Sprite _itemNoItemFrameSprite;
    */

    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private PlayerController _playerController;

    /*
    [SerializeField]
    private Item _selectItem = null;
    */

    private void Awake()
    {
        _itemButton = null;
        _itemButton = GetComponent<Button>();
    }

    /*
    private void Start()
    {
        _selectItem = null;
        _selectItem = PlayerInfo.player.playerSelectItem;

        switch (_selectItem.itemID)
        {
            case 0:     // ロケットエンジン
            case 1:     // スーパーロケットエンジン
            case 2:     // スプリング
            case 3:     // 遠視ゴーグル
            case 4:     // けむり玉
            case 5:     // 果たし状
                _itemIconImage.sprite = _selectItem.itemIcon;
                _itemNoItemText.SetActive(false);
                break;
            default:    // アイテムなし
                _itemButton.image.sprite = _itemNoItemFrameSprite;
                _itemButton.image.color = Color.gray;
                _itemButton.enabled = false;
                _itemIconImage.gameObject.SetActive(false);
                break;
        };
    }

    private void Update()
    {
        if (GamePhotonManager.playerInstanceCompleteFlg &&
            _player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        if (_playerController == null && _player != null)
        {
            _playerController = _player.GetComponent<PlayerController>();
        }
    }

    public void OnButtonDown()
    {
        if (_playerController != null)
        {
            if (0 < _playerController.playerItemUsableCount)
            {
                _playerController.playerItemUsableCount--;
                if (_playerController.playerItemUsableCount <= 0)
                {
                    _itemButton.image.color = Color.gray;
                    _itemButton.enabled = false;
                }

                // 持続時間設定
                _playerController.SetItemDurationTimer(_selectItem.itemDuration);
                _playerController.ResetTimer();

                // 強さ設定
                switch (_selectItem.itemID)
                {
                    case 0:     // ロケットエンジン
                    case 1:     // スーパーロケットエンジン
                                //_playerController.SetItemMaxVelocity(_selectItem.itemPower / 2.0f, 0.0f);
                        _playerController.SetItemMaxVelocity(_selectItem.itemPower, 0.0f);
                        _playerController.SetItemVelocityXImpulse(_selectItem.itemPower);
                        break;
                    case 2:     // スプリング
                        _playerController.SetItemVelocityYImpulse(_selectItem.itemPower);
                        break;
                    case 3:     // 遠視ゴーグル
                        break;
                    case 4:     // けむり玉
                        _playerController.SetItemMaxVelocity(_selectItem.itemPower, 0.0f);
                        _playerController.SetItemVelocityXImpulse(_selectItem.itemPower);
                        _playerController.UseVisualDisturbance(_selectItem.itemDuration);
                        break;
                    case 5:     // 果たし状
                        break;
                };
            }
        }
    }
    */

    private void Update()
    {
        if (GameManager.playerGetObjectCompleteFlg &&
            _player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        if (_playerController == null && _player != null)
        {
            _playerController = _player.GetComponent<PlayerController>();
        }
    }

    public void OnButtonDown()
    {
        if (_playerController != null)
        {
            _playerController.SetItemVelocityYImpulse(62.5f);
        }
    }
}
