// -------------------------------------------------------------------------------
// MinoFactory.cs
//
// 作成日: 2023/10/18
// 作成者: Satou
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinoFactory : MonoBehaviour
{
    #region 変数
    const int MAX_MODLE_CNT = 7;

    IMinoCreatable _minoCreator = default;

    private bool[] _isCreateModels = new bool[MAX_MODLE_CNT];
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
