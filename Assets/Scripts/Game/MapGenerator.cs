/**
 * Copyright (C) 2019-2020 CR dot I Co.,Ltd.
 */
/**
 * タイトル：「マップ情報に基づき、マスタークライアント側でマップを同期生成する」スクリプト
 * 
 * 作成情報： DATE:2019/06/13 作成者:中村 鷹広
 * 更新情報： DATE: 作成者:
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapGenerator : MonoBehaviour
{
    public static GameObject _map = null;

    public static bool displayFlg = false;
    public static bool createObstacleFlg = false;

    [SerializeField]
    private int mapsTagCnt = 0;

    [SerializeField]
    private int railGenerationSize = 7;     // マップ線路生成数
    [SerializeField]
    private int railPattern = 5;            // マップ線路パターン

    [SerializeField]
    private int obstacleGenerationCount = 5;        // 障害物生成個数
    [SerializeField]
    private int obstacleGenerationinterval = 3;     // 障害物生成間隔

    [SerializeField]
    private List<Transform> _obstaclePosTF = null;   // 「ObjectPos」タグ取得リスト

    public GameObject obstacleObject;

    private void Start()
    {
        _map = null;
        _obstaclePosTF = null;
        _obstaclePosTF = new List<Transform>();

        displayFlg = false;
        createObstacleFlg = false;
        mapsTagCnt = 0;
    }

    private void Update()
    {
        if (!displayFlg &&
            SceneManager.GetActiveScene().name.Equals("GameScene") &&
            MapDataBase.createMapDataBaseFlg &&
            MapRandomSelectController.selectCompleteFlg)
        {
            railGenerationSize = MapRandomSelectController.selectMap.mapRailGenerationSize;
            railPattern = MapRandomSelectController.selectMap.mapRailGameObjectList.Count - 2;

            // 雪山作成
            if (MapRandomSelectController.selectMap.mapId == MapDataBase.maps[0].mapId)
            {
                CreateMapSnowyMt();
            }
            // 砂漠作成
            /*
            if (MapRandomSelectController.selectMap.mapId == MapDataBase.maps[1].mapId)
            {
                CreateMapDesert();
            }
            */

            // 障害物生成
            CreateObstacle();
        }
    }

    private void CreateMapSnowyMt()
    {
        if (!displayFlg)
        {
            // エフェクト、開始位置、終了位置を読込
            //_map = PhotonNetwork.Instantiate("SnowyMt", Vector3.zero, Quaternion.identity, 0);
            //_map = PhotonNetwork.InstantiateSceneObject("SnowyMt", Vector3.zero, Quaternion.identity, 0, null);
            _map = Instantiate(MapRandomSelectController.selectMap.mapRailGameObjectList[0], Vector3.zero, Quaternion.identity);
            _map.name = "SnowyMt";

            Transform mapGround = _map.transform.Find("Ground");

            //GameObject[] Maps = new GameObject[railGenerationSize + 2]; // スタートとゴールの2本追加
            GameObject[] Maps = new GameObject[railGenerationSize + 2]; // スタートとゴールの2本追加

            //for (int i = 0; i < railGenerationSize + 2; i++)
            for (int i = 0; i < railGenerationSize + 2; i++)
            {
                // ランダムな値を1～railPatternの間で生成
                int RailNo = Random.Range(1, railPattern + 1);

                Debug.Log("RailNo:" + RailNo.ToString().PadLeft(2, '0'));

                if (i == 0)
                {
                    //一番最初のレールは強制的にスタート用レールを設定する
                    //Maps[i] = PhotonNetwork.Instantiate("SnowyMt_Edge", Vector3.zero, Quaternion.identity, 0);
                    //Maps[i] = PhotonNetwork.InstantiateSceneObject("SnowyMt_Edge", Vector3.zero, Quaternion.identity, 0, null);
                    //Maps[i] = PhotonNetwork.InstantiateSceneObject("SnowyMt_Edge_01", Vector3.zero, Quaternion.identity, 0, null);
                    Maps[i] = Instantiate(MapRandomSelectController.selectMap.mapRailGameObjectList[1], Vector3.zero, Quaternion.identity);
                    Maps[i].name = "SnowyMt_Start";

                    Transform _mapTF = Maps[i].transform;

                    GameObject _startOrGoalPos = _mapTF.Find("StartOrGoalPos").gameObject;
                    _startOrGoalPos.tag = "StartMaps";
                    _mapTF.SetParent(mapGround);

                    // Position/StartのPosを取得
                    GameObject SnowyMtStartPos = GameObject.FindGameObjectWithTag("StartPos");
                    Debug.Log("get:SnowyMtStartPos");
                    if (SnowyMtStartPos == null)
                    {
                        Debug.Log("notget:SnowyMtStartPos");
                    }
                    float SnowyMtStartPosX = SnowyMtStartPos.GetComponent<Transform>().position.x;
                    float SnowyMtStartPosY = SnowyMtStartPos.GetComponent<Transform>().position.y;

                    // StartPosを取得
                    GameObject RailStartPos = _mapTF.Find("ConnectPos/LeftConnectPos").gameObject;
                    Debug.Log("get:RailStartPos");
                    if (RailStartPos == null)
                    {
                        Debug.Log("notget:RailStartPos");
                    }
                    float RailStartPosX = RailStartPos.transform.position.x;
                    float RailStartPosY = RailStartPos.transform.position.y;

                    // EndPosを取得
                    GameObject RailEndPos = _mapTF.Find("ConnectPos/RightConnectPos").gameObject;
                    Debug.Log("get:RailEndPos");
                    if (RailEndPos == null)
                    {
                        Debug.Log("notget:RailEndPos");
                    }
                    float RailEndPosX = RailEndPos.transform.position.x;
                    float RailEndPosY = RailEndPos.transform.position.y;

                    // マップの地面を移動
                    float MapsPosX = Maps[i].GetComponent<Transform>().position.x;
                    float MapsPosY = Maps[i].GetComponent<Transform>().position.y;
                    Maps[i].GetComponent<Transform>().position =
                        new Vector3(
                            SnowyMtStartPos.transform.position.x + (MapsPosX - RailStartPosX),
                            SnowyMtStartPos.transform.position.y + (MapsPosY - RailStartPosY),
                            Maps[i].GetComponent<Transform>().position.z
                            );

                    // StartPosを移動
                    RailStartPos.transform.position = new Vector3(SnowyMtStartPosX, SnowyMtStartPosY, RailStartPos.transform.position.z);

                    // EndPosを移動
                    RailEndPos.transform.position =
                        new Vector3(
                            Maps[i].GetComponent<Transform>().position.x + (RailEndPosX - MapsPosX),
                            Maps[i].GetComponent<Transform>().position.y + (RailEndPosY - MapsPosY),
                            RailEndPos.transform.position.z
                            );
                }
                else if (i == railGenerationSize + 1)
                {
                    //Maps[i] = PhotonNetwork.Instantiate("SnowyMt_Edge", Vector3.zero, Quaternion.identity, 0);
                    //Maps[i] = PhotonNetwork.InstantiateSceneObject("SnowyMt_Edge", Vector3.zero, Quaternion.identity, 0, null);
                    //Maps[i] = PhotonNetwork.InstantiateSceneObject("SnowyMt_Edge_01", Vector3.zero, Quaternion.identity, 0, null);
                    Maps[i] = Instantiate(MapRandomSelectController.selectMap.mapRailGameObjectList[1], Vector3.zero, Quaternion.identity);
                    Maps[i].name = "SnowyMt_End";

                    Transform _mapTF = Maps[i].transform;

                    GameObject _startOrGoalPos = _mapTF.Find("StartOrGoalPos").gameObject;
                    _startOrGoalPos.tag = "GoalMaps";
                    _mapTF.SetParent(mapGround);

                    // 前のマップのEndPosを取得
                    GameObject BeforeRailEndPos = Maps[i - 1].transform.Find("ConnectPos/RightConnectPos").gameObject;
                    Debug.Log("get:BeforeRailEndPos");
                    if (BeforeRailEndPos == null)
                    {
                        Debug.Log("notget:BeforeRailEndPos");
                    }
                    float BeforeRailEndPosX = BeforeRailEndPos.transform.position.x;
                    float BeforeRailEndPosY = BeforeRailEndPos.transform.position.y;
                    // StartPosを取得
                    GameObject RailStartPos = _mapTF.Find("ConnectPos/LeftConnectPos").gameObject;
                    Debug.Log("get:RailStartPos");
                    if (RailStartPos == null)
                    {
                        Debug.Log("notget:RailStartPos");
                    }
                    float RailStartPosX = RailStartPos.transform.position.x;
                    float RailStartPosY = RailStartPos.transform.position.y;

                    // EndPosを取得
                    GameObject RailEndPos = _mapTF.Find("ConnectPos/RightConnectPos").gameObject;
                    Debug.Log("get:RailEndPos");
                    if (RailEndPos == null)
                    {
                        Debug.Log("notget:RailEndPos");
                    }
                    float RailEndPosX = RailEndPos.transform.position.x;
                    float RailEndPosY = RailEndPos.transform.position.y;

                    // マップの地面を移動
                    float MapsPosX = Maps[i].GetComponent<Transform>().position.x;
                    float MapsPosY = Maps[i].GetComponent<Transform>().position.y;
                    Maps[i].GetComponent<Transform>().position =
                        new Vector3(
                            BeforeRailEndPosX + (MapsPosX - RailStartPosX),
                            BeforeRailEndPosY + (MapsPosY - RailStartPosY),
                            Maps[i].GetComponent<Transform>().position.z
                            );

                    // StartPosを移動
                    RailStartPos.transform.position = new Vector3(BeforeRailEndPosX, BeforeRailEndPosY, RailStartPos.transform.position.z);

                    // EndPosを移動
                    RailEndPos.transform.position =
                        new Vector3(
                            Maps[i].GetComponent<Transform>().position.x + (RailEndPosX - MapsPosX),
                            Maps[i].GetComponent<Transform>().position.y + (RailEndPosY - MapsPosY),
                            RailEndPos.transform.position.z
                            );
                }
                else
                {
                    //Maps[i] = PhotonNetwork.Instantiate("SnowyMt_" + RailNo.ToString().PadLeft(2, '0'), Vector3.zero, Quaternion.identity, 0);
                    //Maps[i] = PhotonNetwork.InstantiateSceneObject("SnowyMt_" + RailNo.ToString().PadLeft(2, '0'), Vector3.zero, Quaternion.identity, 0, null);
                    //GameObject.Instantiate("SnowyMt_" + RailNo.ToString().PadLeft(2, '0'), Vector3.zero, Quaternion.identity);
                    Maps[i] = Instantiate(MapRandomSelectController.selectMap.mapRailGameObjectList[RailNo + 1], Vector3.zero, Quaternion.identity);
                    Maps[i].name = "SnowyMt_" + RailNo.ToString().PadLeft(2, '0');
                    Maps[i].transform.SetParent(mapGround);

                    // 前のマップのEndPosを取得
                    GameObject BeforeRailEndPos = Maps[i - 1].transform.Find("ConnectPos/RightConnectPos").gameObject;
                    Debug.Log("get:BeforeRailEndPos");
                    if (BeforeRailEndPos == null)
                    {
                        Debug.Log("notget:BeforeRailEndPos");
                    }
                    float BeforeRailEndPosX = BeforeRailEndPos.transform.position.x;
                    float BeforeRailEndPosY = BeforeRailEndPos.transform.position.y;
                    // StartPosを取得
                    GameObject RailStartPos = Maps[i].transform.Find("ConnectPos/LeftConnectPos").gameObject;
                    Debug.Log("get:RailStartPos");
                    if (RailStartPos == null)
                    {
                        Debug.Log("notget:RailStartPos");
                    }
                    float RailStartPosX = RailStartPos.transform.position.x;
                    float RailStartPosY = RailStartPos.transform.position.y;

                    // EndPosを取得
                    GameObject RailEndPos = Maps[i].transform.Find("ConnectPos/RightConnectPos").gameObject;
                    Debug.Log("get:RailEndPos");
                    if (RailEndPos == null)
                    {
                        Debug.Log("notget:RailEndPos");
                    }
                    float RailEndPosX = RailEndPos.transform.position.x;
                    float RailEndPosY = RailEndPos.transform.position.y;

                    // マップの地面を移動
                    float MapsPosX = Maps[i].GetComponent<Transform>().position.x;
                    float MapsPosY = Maps[i].GetComponent<Transform>().position.y;
                    Maps[i].GetComponent<Transform>().position =
                        new Vector3(
                            BeforeRailEndPosX + (MapsPosX - RailStartPosX),
                            BeforeRailEndPosY + (MapsPosY - RailStartPosY),
                            Maps[i].GetComponent<Transform>().position.z
                            );

                    // StartPosを移動
                    RailStartPos.transform.position = new Vector3(BeforeRailEndPosX, BeforeRailEndPosY, RailStartPos.transform.position.z);

                    // EndPosを移動
                    RailEndPos.transform.position =
                        new Vector3(
                            Maps[i].GetComponent<Transform>().position.x + (RailEndPosX - MapsPosX),
                            Maps[i].GetComponent<Transform>().position.y + (RailEndPosY - MapsPosY),
                            RailEndPos.transform.position.z
                            );
                }
            }
            displayFlg = true;
        }
    }

    private void ChangeRelationShipMapSnowyMt()
    {
        if (!displayFlg)
        {
            // Position/StartのPosを取得
            GameObject[] SnowyMtMaps = GameObject.FindGameObjectsWithTag("Maps");

            if (SnowyMtMaps != null)
            {
                if (SnowyMtMaps.Length > 0)
                {
                    _map = GameObject.Find("SnowyMt(Clone)");
                    _map.name = _map.name.Replace("(Clone)", "");
                    Transform mapGround = _map.transform.Find("Ground");

                    for (int i = 0; i < SnowyMtMaps.Length; i++)
                    {
                        if (SnowyMtMaps[i].name.Contains("(Clone)"))
                        {
                            SnowyMtMaps[i].name = SnowyMtMaps[i].name.Replace("(Clone)", "");
                            if (SnowyMtMaps[i].name == "SnowyMt_Edge_01")
                            {
                                Transform _snowyMtMapTF = SnowyMtMaps[i].transform;
                                if (-15.0f < _snowyMtMapTF.position.x && _snowyMtMapTF.position.x < 15.0f)
                                {
                                    SnowyMtMaps[i].name = "SnowyMt_Start";
                                    GameObject _startOrGoalPos = _snowyMtMapTF.Find("StartOrGoalPos").gameObject;
                                    _startOrGoalPos.tag = "StartMaps";
                                }
                                else
                                {
                                    SnowyMtMaps[i].name = "SnowyMt_End";
                                    GameObject _startOrGoalPos = _snowyMtMapTF.Find("StartOrGoalPos").gameObject;
                                    _startOrGoalPos.tag = "GoalMaps";
                                }
                            }
                            SnowyMtMaps[i].transform.SetParent(mapGround);
                            mapsTagCnt++;
                        }
                    }

                    if (0 < mapsTagCnt)
                    {
                        displayFlg = true;
                    }
                }
            }
        }
    }

    private void CreateMapDesert()
    {
        /*
        if (!displayFlg)
        {
            // エフェクト、開始位置、終了位置を読込
            _map = PhotonNetwork.InstantiateSceneObject("Desert", Vector3.zero, Quaternion.identity, 0, null);
            _map.name = "Desert";

            Transform mapGround = _map.transform.Find("Ground");

            GameObject[] Maps = new GameObject[railGenerationSize + 2]; // スタートとゴールの2本追加

            for (int i = 0; i < railGenerationSize + 2; i++)
            {
                // ランダムな値を1～railPatternの間で生成
                int RailNo = Random.Range(1, railPattern + 1);

                Debug.Log("RailNo:" + RailNo.ToString().PadLeft(2, '0'));

                if (i == 0)
                {
                    //一番最初のレールは強制的にスタート用レールを設定する
                    Maps[i] = PhotonNetwork.InstantiateSceneObject("Desert_Edge", Vector3.zero, Quaternion.identity, 0, null);
                    Maps[i].name = "Desert_Start";

                    Transform _mapTF = Maps[i].transform;

                    GameObject _startOrGoalPos = _mapTF.Find("StartOrGoalPos").gameObject;
                    _startOrGoalPos.tag = "StartMaps";
                    _mapTF.SetParent(mapGround);

                    // Position/StartのPosを取得
                    GameObject DesertStartPos = GameObject.FindGameObjectWithTag("StartPos");
                    Debug.Log("get:DesertStartPos");
                    if (DesertStartPos == null)
                    {
                        Debug.Log("notget:DesertStartPos");
                    }
                    float DesertStartPosX = DesertStartPos.GetComponent<Transform>().position.x;
                    float DesertStartPosY = DesertStartPos.GetComponent<Transform>().position.y;

                    // StartPosを取得
                    GameObject RailStartPos = _mapTF.Find("ConnectPos/LeftPos").gameObject;
                    Debug.Log("get:RailStartPos");
                    if (RailStartPos == null)
                    {
                        Debug.Log("notget:RailStartPos");
                    }
                    float RailStartPosX = RailStartPos.transform.position.x;
                    float RailStartPosY = RailStartPos.transform.position.y;

                    // EndPosを取得
                    GameObject RailEndPos = _mapTF.Find("ConnectPos/RightPos").gameObject;
                    Debug.Log("get:RailEndPos");
                    if (RailEndPos == null)
                    {
                        Debug.Log("notget:RailEndPos");
                    }
                    float RailEndPosX = RailEndPos.transform.position.x;
                    float RailEndPosY = RailEndPos.transform.position.y;

                    // マップの地面を移動
                    float MapsPosX = Maps[i].GetComponent<Transform>().position.x;
                    float MapsPosY = Maps[i].GetComponent<Transform>().position.y;
                    Maps[i].GetComponent<Transform>().position =
                        new Vector3(
                            DesertStartPos.transform.position.x + (MapsPosX - RailStartPosX),
                            DesertStartPos.transform.position.y + (MapsPosY - RailStartPosY),
                            Maps[i].GetComponent<Transform>().position.z
                            );

                    // StartPosを移動
                    RailStartPos.transform.position = new Vector3(DesertStartPosX, DesertStartPosY, RailStartPos.transform.position.z);

                    // EndPosを移動
                    RailEndPos.transform.position =
                        new Vector3(
                            Maps[i].GetComponent<Transform>().position.x + (RailEndPosX - MapsPosX),
                            Maps[i].GetComponent<Transform>().position.y + (RailEndPosY - MapsPosY),
                            RailEndPos.transform.position.z
                            );
                }
                else if (i == railGenerationSize + 1)
                {
                    Maps[i] = PhotonNetwork.InstantiateSceneObject("Desert_Edge", Vector3.zero, Quaternion.identity, 0, null);
                    Maps[i].name = "Desert_End";

                    Transform _mapTF = Maps[i].transform;

                    GameObject _startOrGoalPos = _mapTF.Find("StartOrGoalPos").gameObject;
                    _startOrGoalPos.tag = "GoalMaps";
                    _mapTF.SetParent(mapGround);

                    // 前のマップのEndPosを取得
                    GameObject BeforeRailEndPos = Maps[i - 1].transform.Find("ConnectPos/RightPos").gameObject;
                    Debug.Log("get:BeforeRailEndPos");
                    if (BeforeRailEndPos == null)
                    {
                        Debug.Log("notget:BeforeRailEndPos");
                    }
                    float BeforeRailEndPosX = BeforeRailEndPos.transform.position.x;
                    float BeforeRailEndPosY = BeforeRailEndPos.transform.position.y;
                    // StartPosを取得
                    GameObject RailStartPos = _mapTF.Find("ConnectPos/LeftPos").gameObject;
                    Debug.Log("get:RailStartPos");
                    if (RailStartPos == null)
                    {
                        Debug.Log("notget:RailStartPos");
                    }
                    float RailStartPosX = RailStartPos.transform.position.x;
                    float RailStartPosY = RailStartPos.transform.position.y;

                    // EndPosを取得
                    GameObject RailEndPos = _mapTF.Find("ConnectPos/RightPos").gameObject;
                    Debug.Log("get:RailEndPos");
                    if (RailEndPos == null)
                    {
                        Debug.Log("notget:RailEndPos");
                    }
                    float RailEndPosX = RailEndPos.transform.position.x;
                    float RailEndPosY = RailEndPos.transform.position.y;

                    // マップの地面を移動
                    float MapsPosX = Maps[i].GetComponent<Transform>().position.x;
                    float MapsPosY = Maps[i].GetComponent<Transform>().position.y;
                    Maps[i].GetComponent<Transform>().position =
                        new Vector3(
                            BeforeRailEndPosX + (MapsPosX - RailStartPosX),
                            BeforeRailEndPosY + (MapsPosY - RailStartPosY),
                            Maps[i].GetComponent<Transform>().position.z
                            );

                    // StartPosを移動
                    RailStartPos.transform.position = new Vector3(BeforeRailEndPosX, BeforeRailEndPosY, RailStartPos.transform.position.z);

                    // EndPosを移動
                    RailEndPos.transform.position =
                        new Vector3(
                            Maps[i].GetComponent<Transform>().position.x + (RailEndPosX - MapsPosX),
                            Maps[i].GetComponent<Transform>().position.y + (RailEndPosY - MapsPosY),
                            RailEndPos.transform.position.z
                            );
                }
                else
                {
                    Maps[i] = PhotonNetwork.InstantiateSceneObject("Desert_" + RailNo.ToString().PadLeft(2, '0'), Vector3.zero, Quaternion.identity, 0, null);
                    Maps[i].name = "Desert" + RailNo.ToString().PadLeft(2, '0');
                    Maps[i].transform.SetParent(mapGround);

                    // 前のマップのEndPosを取得
                    GameObject BeforeRailEndPos = Maps[i - 1].transform.Find("ConnectPos/RightPos").gameObject;
                    Debug.Log("get:BeforeRailEndPos");
                    if (BeforeRailEndPos == null)
                    {
                        Debug.Log("notget:BeforeRailEndPos");
                    }
                    float BeforeRailEndPosX = BeforeRailEndPos.transform.position.x;
                    float BeforeRailEndPosY = BeforeRailEndPos.transform.position.y;
                    // StartPosを取得
                    GameObject RailStartPos = Maps[i].transform.Find("ConnectPos/LeftPos").gameObject;
                    Debug.Log("get:RailStartPos");
                    if (RailStartPos == null)
                    {
                        Debug.Log("notget:RailStartPos");
                    }
                    float RailStartPosX = RailStartPos.transform.position.x;
                    float RailStartPosY = RailStartPos.transform.position.y;

                    // EndPosを取得
                    GameObject RailEndPos = Maps[i].transform.Find("ConnectPos/RightPos").gameObject;
                    Debug.Log("get:RailEndPos");
                    if (RailEndPos == null)
                    {
                        Debug.Log("notget:RailEndPos");
                    }
                    float RailEndPosX = RailEndPos.transform.position.x;
                    float RailEndPosY = RailEndPos.transform.position.y;

                    // マップの地面を移動
                    float MapsPosX = Maps[i].GetComponent<Transform>().position.x;
                    float MapsPosY = Maps[i].GetComponent<Transform>().position.y;
                    Maps[i].GetComponent<Transform>().position =
                        new Vector3(
                            BeforeRailEndPosX + (MapsPosX - RailStartPosX),
                            BeforeRailEndPosY + (MapsPosY - RailStartPosY),
                            Maps[i].GetComponent<Transform>().position.z
                            );

                    // StartPosを移動
                    RailStartPos.transform.position = new Vector3(BeforeRailEndPosX, BeforeRailEndPosY, RailStartPos.transform.position.z);

                    // EndPosを移動
                    RailEndPos.transform.position =
                        new Vector3(
                            Maps[i].GetComponent<Transform>().position.x + (RailEndPosX - MapsPosX),
                            Maps[i].GetComponent<Transform>().position.y + (RailEndPosY - MapsPosY),
                            RailEndPos.transform.position.z
                            );
                }
            }
            displayFlg = true;
        }
        */
    }

    private void ChangeRelationShipMapDesert()
    {
        if (!displayFlg)
        {
            // Position/StartのPosを取得
            GameObject[] DesertMaps = GameObject.FindGameObjectsWithTag("Maps");

            if (DesertMaps != null)
            {
                if (DesertMaps.Length > 0)
                {
                    _map = GameObject.Find("Desert(Clone)");
                    _map.name = _map.name.Replace("(Clone)", "");
                    Transform mapGround = _map.transform.Find("Ground");

                    for (int i = 0; i < DesertMaps.Length; i++)
                    {
                        if (DesertMaps[i].name.Contains("(Clone)"))
                        {
                            DesertMaps[i].name = DesertMaps[i].name.Replace("(Clone)", "");
                            if (DesertMaps[i].name == "Desert_Edge")
                            {
                                Transform _desertMapTF = DesertMaps[i].transform;
                                if (-15.0f < _desertMapTF.position.x && _desertMapTF.position.x < 15.0f)
                                {
                                    DesertMaps[i].name = "Desert_Start";
                                    GameObject _startOrGoalPos = _desertMapTF.Find("StartOrGoalPos").gameObject;
                                    _startOrGoalPos.tag = "StartMaps";
                                }
                                else
                                {
                                    DesertMaps[i].name = "Desert_End";
                                    GameObject _startOrGoalPos = _desertMapTF.Find("StartOrGoalPos").gameObject;
                                    _startOrGoalPos.tag = "GoalMaps";
                                }
                            }
                            DesertMaps[i].transform.SetParent(mapGround);
                            mapsTagCnt++;
                        }
                    }

                    if (0 < mapsTagCnt)
                    {
                        displayFlg = true;
                    }
                }
            }
        }
    }

    private void CreateObstacle()
    {
        GameObject[] objectPos = GameObject.FindGameObjectsWithTag("ObjectPos");

        List<int> objectPosNoList = new List<int>();

        if (0 < objectPos.Length)
        {
            // ランダムで生成場所を確保
            for (int i = 0; i < obstacleGenerationCount; i++)
            {
                // ランダムな値を0～objectPos.Length - 1の間で生成
                int objectPosNo = Random.Range(0, objectPos.Length);

                if (objectPosNoList.Contains(objectPosNo))
                {
                    continue;
                }
                else
                {
                    _obstaclePosTF.Add(objectPos[objectPosNo].transform);
                }
            }

            // 生成場所に生成
            for (int i = 0; i < _obstaclePosTF.Count; i++)
            {
                Debug.Log("_obstaclePosTF[" + i.ToString() + "] : " + _obstaclePosTF[i].position.ToString());
                // PhotonNetwork.InstantiateSceneObject("SnowyMt", _obstaclePosTF[i].position, Quaternion.identity, 0, null);

                // プレイヤー用障害物生成
                //GameObject ObstacleObjectForPlayer = PhotonNetwork.Instantiate("ObstacleSnowyMt01", _obstaclePosTF[i].position, Quaternion.identity, 0, null);
                //GameObject ObstacleObjectForPlayer = PhotonNetwork.Instantiate("ObstacleSnowyMt01", _obstaclePosTF[i].position, _obstaclePosTF[i].rotation, 0, null);
                GameObject ObstacleObjectForPlayer = Instantiate(obstacleObject, _obstaclePosTF[i].position, _obstaclePosTF[i].rotation);
                ObstacleObjectForPlayer.name = "ObstacleSnowyMt01_Player";
                ObstacleObjectForPlayer.tag = "PlayerObstacle";
                ObstacleObjectForPlayer.layer = 8;                           // 8:Player
                ObstacleController obstacleControllerForPlayer = ObstacleObjectForPlayer.GetComponent<ObstacleController>();
                obstacleControllerForPlayer.SpriteObject.layer = 8;          // 8:Player
                obstacleControllerForPlayer.ArrowsSpriteObject.layer = 8;    // 8:Player
                ObstacleCancellationController _obstacleCancellationControllerForPlayer = obstacleControllerForPlayer.SpriteObject.GetComponent<ObstacleCancellationController>();

                if (i == 0)
                {
                    _obstacleCancellationControllerForPlayer.obstacleCancelDirection = "up";
                }
                else if (i == 1)
                {
                    _obstacleCancellationControllerForPlayer.obstacleCancelDirection = "left";
                }
                else if (i == 2)
                {
                    _obstacleCancellationControllerForPlayer.obstacleCancelDirection = "down";
                }
                else if (i == 3)
                {
                    _obstacleCancellationControllerForPlayer.obstacleCancelDirection = "right";
                }
                else
                {
                    _obstacleCancellationControllerForPlayer.obstacleCancelDirection = "up";
                }
            }

            createObstacleFlg = true;
        }
    }

    /*
    private void CreateObstacleForNonMasterClient()
    {
        // マスタークライアントの生成した障害物のtransformを取得
        _obstaclePosTF = null;
        _obstaclePosTF = new List<Transform>();

        GameObject[] rivalObstacleObjects = GameObject.FindGameObjectsWithTag("RivalObstacle");

        if (rivalObstacleObjects != null)
        {
            if (obstacleGenerationCount == rivalObstacleObjects.Length)
            {
                for (int i = 0; i < rivalObstacleObjects.Length; i++)
                {
                    _obstaclePosTF.Add(rivalObstacleObjects[i].transform);
                }

                bool notExistFlg = false;

                // 生成場所に生成
                for (int i = 0; i < _obstaclePosTF.Count; i++)
                {
                    ObstacleController rivalObstacleController = _obstaclePosTF[i].GetComponent<ObstacleController>();
                    ObstacleCancellationController rivalObstacleCancellationController = rivalObstacleController.SpriteObject.GetComponent<ObstacleCancellationController>();

                    if (!rivalObstacleCancellationController.arrowsRotateFlg)
                    {
                        notExistFlg = true;
                    }
                }

                if (!notExistFlg)
                {
                    createObstacleFlg = true;
                }

                if (createObstacleFlg)
                {

                    // 生成場所に生成
                    for (int i = 0; i < _obstaclePosTF.Count; i++)
                    {
                        ObstacleController rivalObstacleController = _obstaclePosTF[i].GetComponent<ObstacleController>();
                        ObstacleCancellationController rivalObstacleCancellationController = rivalObstacleController.SpriteObject.GetComponent<ObstacleCancellationController>();

                        // プレイヤー用障害物生成
                        //GameObject ObstacleObject = PhotonNetwork.Instantiate("ObstacleSnowyMt01", _obstaclePosTF[i].position, Quaternion.identity, 0, null);
                        GameObject ObstacleObject = PhotonNetwork.Instantiate("ObstacleSnowyMt01", _obstaclePosTF[i].position, _obstaclePosTF[i].rotation, 0, null);
                        ObstacleObject.name = "ObstacleSnowyMt01_Player";
                        ObstacleObject.tag = "PlayerObstacle";
                        ObstacleObject.layer = 8;                           // 8:Player
                        ObstacleController obstacleController = ObstacleObject.GetComponent<ObstacleController>();
                        obstacleController.SpriteObject.layer = 8;          // 8:Player
                        obstacleController.ArrowsSpriteObject.layer = 8;    // 8:Player
                        ObstacleCancellationController _obstacleCancellationController = obstacleController.SpriteObject.GetComponent<ObstacleCancellationController>();

                        _obstacleCancellationController.obstacleCancelDirection = rivalObstacleCancellationController.obstacleCancelDirection;
                    }
                }
            }
        }
    }
    */
}
