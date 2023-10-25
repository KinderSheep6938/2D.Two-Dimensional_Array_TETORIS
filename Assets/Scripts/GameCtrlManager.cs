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
    private const int PLAYING_ID = 0;

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
        //ゲーム終了
        if(_fieldSystem.GetPlayStatus() == GAMEEND_ID) { }

        //操作中か
        if (_fieldSystem.GetPlayStatus() == PLAYING_ID) { return; /*操作中*/ }
        
        //操作が終了している
        _factorySystem.CreateMino(); //ミノ生成
    }
    #endregion
}
