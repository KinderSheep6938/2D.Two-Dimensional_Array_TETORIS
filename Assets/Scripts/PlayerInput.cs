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
    private const float AUTOREPEAT_TIME = 0.3f; //長押し待機時間
    private float _longPushTimer = 0f; //長押しタイマー

    private const float AUTOREPEAT_MOVETIME = 0.05f; //オートリピートの移動待機時間
    private float _autoRepeatTimer = 0f; //オートリピートタイマー

    private bool _isOnce = false; //一回押し判定

    private IMinoUnionCtrl _minoUnion = default; //ミノ操作システムのインターフェイス
    private IMinoHoldable _holdSystem = default; //ホールドシステムのインターフェイス
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
        //移動
        InputMoveButton();
        //左回転
        if (Input.GetKeyDown(KeyCode.Q)) { _minoUnion.Rotate(-1); }
        //右回転
        if (Input.GetKeyDown(KeyCode.E)) { _minoUnion.Rotate(1); }
        //ハードドロップ
        if (Input.GetKeyDown(KeyCode.W)) { _minoUnion.HardDrop(); }
        //ソフトドロップ
        if (Input.GetKey(KeyCode.S)) { _minoUnion.SoftDrop(); }
        //ホールド
        //まだホールドをしていない場合のみ発動
        if (Input.GetKeyDown(KeyCode.LeftShift) && _minoUnion.CheckHold()) { _holdSystem.Hold(); }
    }

    /// <summary>
    /// <para>InputMoveButton</para>
    /// <para>横移動入力の制御を行います</para>
    /// </summary>
    private void InputMoveButton()
    {
        //左移動
        if (Input.GetKey(KeyCode.A))
        {
            AutoRepeat(-1);
            return;
        }
        //右移動
        if (Input.GetKey(KeyCode.D))
        {
            AutoRepeat(1);
            return;
        }
        AutoRepeatReset();
    }

    /// <summary>
    /// <para>AutoRepeat</para>
    /// <para>横移動の長押し処理を行います</para>
    /// </summary>
    /// <param name="x"></param>
    private void AutoRepeat(int x)
    {
        //長押し待機時間を越していない
        if(_longPushTimer <= AUTOREPEAT_TIME)
        {
            //一回押し判定がない
            if (!_isOnce)
            {
                //移動
                _minoUnion.Move(x);
                //一回押し判定
                _isOnce = true;
            }
        }
        else //長押し
        {
            _autoRepeatTimer += Time.deltaTime; //タイマー加算
            //タイマーが待機時間を越した
            if(AUTOREPEAT_MOVETIME <= _autoRepeatTimer)
            {
                //移動
                _minoUnion.Move(x);
                //タイマー初期化
                _autoRepeatTimer = 0;
            }
        }

        _longPushTimer += Time.deltaTime; //タイマー加算
    }

    /// <summary>
    /// <para>AutoRepeatReset</para>
    /// <para>長押し処理の変数の初期化を行います</para>
    /// </summary>
    private void AutoRepeatReset()
    {
        //初期化
        _autoRepeatTimer = 0;
        _longPushTimer = 0;
        _isOnce = false;
    }
    #endregion
}
