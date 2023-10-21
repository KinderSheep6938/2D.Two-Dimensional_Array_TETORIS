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
    [SerializeField,Tooltip("MinoFactory")] //ミノ生成機構
    private MinoFactory _factorySystem = default;
    [SerializeField,Tooltip("FieldManager")] //フィールド管理機構
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

    }

    /// <summary>
    /// 更新前処理
    /// </summary>
    void Start()
    {

    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {

    }
    #endregion
}
