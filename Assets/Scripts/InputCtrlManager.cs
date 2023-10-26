// -------------------------------------------------------------------------------
// InputCtrlManager.cs
//
// 作成日: 2023/10/26
// 作成者: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputCtrlManager : MonoBehaviour
{
    #region 変数

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

    void GetToInput()
    {
        if(Gamepad.current == null) { return; }

        
    }
    #endregion
}
