// -------------------------------------------------------------------------------
// PlayerInput.cs
//
// 作成日: 2023/10/21
// 作成者: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    #region 変数
    private IMinoUnionCtrl _minoUnion = default; //ミノ操作システムのインターフェイス
    private IMinoHoldable _holdSystem = default;
    #endregion

    #region プロパティ

    #endregion

    #region メソッド
    /// <summary>
    /// 初期化処理
    /// </summary>
    void Awake()
    {
        _minoUnion = GetComponent<IMinoUnionCtrl>();
        _holdSystem = FindObjectOfType<HoldMino>().GetComponent<IMinoHoldable>();
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
        PlayerIO();
    }

    /// <summary>
    /// <para>PlayerIO</para>
    /// <para>プレイヤーの入力に応じて、ミノを操作します</para>
    /// </summary>
    private void PlayerIO()
    {
        if (Input.GetKeyDown(KeyCode.A)) { _minoUnion.Move(-1); }
        if (Input.GetKeyDown(KeyCode.D)) { _minoUnion.Move(1); }
        if (Input.GetKeyDown(KeyCode.Q)) { _minoUnion.Rotate(-1); }
        if (Input.GetKeyDown(KeyCode.E)) { _minoUnion.Rotate(1); }
        if (Input.GetKeyDown(KeyCode.W)) { _minoUnion.HardDrop(); }
        if (Input.GetKey(KeyCode.S)) { _minoUnion.SoftDrop(); }
        Debug.Log(_holdSystem);
        if (Input.GetKeyDown(KeyCode.LeftShift)) { _holdSystem.Hold(); }
    }
    #endregion
}
