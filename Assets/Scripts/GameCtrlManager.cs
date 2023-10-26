// -------------------------------------------------------------------------------
// GameCtrlManager.cs
//
// 作成日: 2023/10/21
// 作成者: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCtrlManager : MonoBehaviour
{
    #region 変数
    private const int GAMEEND_ID = -1;
    private const int COMMIT_ID = 1;
    private int _nowGameStatus = 0;

    private GameObject _gameoverViewObj = default;

    private MinoFactory _factorySystem = default;
    private FieldManager _fieldSystem = default;

    #endregion

    #region プロパティ

    #endregion

    #region メソッド
    /// <summary>
    /// 初期化処理
    /// </summary>
    void Awake()
    {
        _factorySystem = GetComponent<MinoFactory>();
        _fieldSystem = GetComponent<FieldManager>();
    }

    /// <summary>
    /// 更新前処理
    /// </summary>
    void Start()
    {
        //ネクストミノと操作ミノの二つにミノを送らなければいかないため、ミノ生成を２回行う
        _factorySystem.CreateMino(); //ミノ生成
        _factorySystem.CreateMino(); //ミノ生成
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        _nowGameStatus = _fieldSystem.GetPlayStatus();

        //ゲーム状況がコミット状態ではない
        if(_nowGameStatus != COMMIT_ID)
        {
            //ゲーム状態が終了状態である
            if(_nowGameStatus == GAMEEND_ID)
            {
                _gameoverViewObj.SetActive(true);
            }
            return; //操作終了
        }
        
        //コミット状態である場合は新しくミノを生成する
        _factorySystem.CreateMino();
    }
    #endregion
}
