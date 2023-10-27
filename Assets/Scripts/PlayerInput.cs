// -------------------------------------------------------------------------------
// PlayerInput.cs
//
// 作成日: 2023/10/21
// 作成者: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    #region 変数
    //オートリピート機構（長押し移動）
    private bool _onAutoRepeat = false; //オートリピート開始判定
    private int _autoRepeatDire = 1; //オートリピート方向
    private const int RIGHT_DIREID = 1; //右方向ID
    private const int LEFT_DIREID = -1; //左方向ID
    private const float AUTOREPEAT_MOVETIME = 0.05f; //オートリピートの移動待機時間
    private float _autoRepeatTimer = 0f; //オートリピートタイマー

    private bool _isSoft = false; //ソフトドロップ開始判定

    private bool _isStart = false; //ゲーム開始フラグ
    private bool _canGame = false; //ゲームプレイ可能フラグ

    private IMinoUnionCtrl _minoUnion = default; //ミノ操作システムのインターフェイス
    private IMinoHoldable _holdSystem = default; //ホールドシステムのインターフェイス
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
    /// 更新処理
    /// </summary>
    void Update()
    {
        //オートリピート判定がある
        if (_onAutoRepeat) { AutoRepeat(_autoRepeatDire); }
        //ソフトドロップ判定がある
        if (_isSoft) { _minoUnion.SoftDrop(); }
    }

    /// <summary>
    /// <para>OnMoveLeft</para>
    /// <para>左移動ボタンが押されたときに呼ばれます</para>
    /// </summary>
    /// <param name="context">ボタン状態</param>
    public void OnMoveLeft(InputAction.CallbackContext context)
    {
        if (!_canGame) { return; /*まだ開始していない*/ }

        if (context.phase == InputActionPhase.Started) //押された
        {
            _minoUnion.Move(LEFT_DIREID);
            _onAutoRepeat = false;
            AutoRepeatReset();
        }

        if (context.phase == InputActionPhase.Performed) //押されてから指定した時間がたった
        {
            _onAutoRepeat = true;
            _autoRepeatDire = LEFT_DIREID;
        }

        if (context.phase == InputActionPhase.Canceled) //離れた
        {
            _onAutoRepeat = false;
            AutoRepeatReset();
        }
    }

    /// <summary>
    /// <para>OnMoveRight</para>
    /// <para>右移動ボタンが押されたときに呼ばれます</para>
    /// </summary>
    /// <param name="context">ボタン状態</param>
    public void OnMoveRight(InputAction.CallbackContext context)
    {
        if (!_canGame) { return; /*まだ開始していない*/ }

        if (context.phase == InputActionPhase.Started) //押された
        {
            _minoUnion.Move(RIGHT_DIREID);
            _onAutoRepeat = false;
            AutoRepeatReset();
        }

        if (context.phase == InputActionPhase.Performed) //押されてから指定した時間がたった
        {
            _onAutoRepeat = true;
            _autoRepeatDire = RIGHT_DIREID;
        }

        if (context.phase == InputActionPhase.Canceled) //離れた
        {
            _onAutoRepeat = false;
            AutoRepeatReset();
        }
    }

    /// <summary>
    /// <para>OnRotateLeft</para>
    /// <para>左回転ボタンが押されたときに呼ばれます</para>
    /// </summary>
    /// <param name="context">ボタン状態</param>
    public void OnRotateLeft(InputAction.CallbackContext context)
    {
        if (!_canGame) { return; /*まだ開始していない*/ }

        if (context.phase == InputActionPhase.Performed) //押された
        {
            _minoUnion.Rotate(LEFT_DIREID);
        }
    }

    /// <summary>
    /// <para>OnRotateRight</para>
    /// <para>右回転ボタンが押されたときに呼ばれます</para>
    /// </summary>
    /// <param name="context">ボタン状態</param>
    public void OnRotateRight(InputAction.CallbackContext context)
    {
        if (!_canGame) { return; /*まだ開始していない*/ }

        if (context.phase == InputActionPhase.Performed) //押された
        {
            _minoUnion.Rotate(RIGHT_DIREID);
        }
    }

    /// <summary>
    /// <para>OnHardDrop</para>
    /// <para>ハードドロップボタンが押されたときに呼ばれます</para>
    /// </summary>
    /// <param name="context">ボタン状態</param>
    public void OnHardDrop(InputAction.CallbackContext context)
    {
        if (!_canGame) { return; /*まだ開始していない*/ }

        if (context.phase == InputActionPhase.Performed) //押された
        {
            _minoUnion.HardDrop();
        }
    }

    /// <summary>
    /// <para>OnSoftDrop</para>
    /// <para>ソフトドロップボタンが押されたときに呼ばれます</para>
    /// </summary>
    /// <param name="context">ボタン状態</param>
    public void OnSoftDrop(InputAction.CallbackContext context)
    {
        if (!_canGame) { return; /*まだ開始していない*/ }

        if (context.phase == InputActionPhase.Started) //押された
        {
            _isSoft = true;
        }

        if (context.phase == InputActionPhase.Canceled) //離れた
        {
            _isSoft = false;
        }
    }

    /// <summary>
    /// <para>OnHold</para>
    /// <para>ホールドボタンが押されたときに呼ばれます</para>
    /// </summary>
    /// <param name="context">ボタン状態</param>
    public void OnHold(InputAction.CallbackContext context)
    {
        if (!_canGame) { return; /*まだ開始していない*/ }

        if (context.phase == InputActionPhase.Performed)
        {
            //ホールドがない場合は、ホールドする
            if (!_minoUnion.CheckHasHold()) { _holdSystem.Hold(); }
        }
    }

    /// <summary>
    /// <para>OnStart</para>
    /// <para>開始ボタンが押されたときに呼ばれます</para>
    /// </summary>
    /// <param name="context">ボタン状態</param>
    public void OnStart(InputAction.CallbackContext context)
    {
        //まだゲームがスタートしていない
        if (context.phase == InputActionPhase.Performed && !_isStart) //押された
        {
            _canGame = true; //プレイ可能に
            _isStart = true; //ゲーム開始
        }
    }

    /// <summary>
    /// <para>AutoRepeat</para>
    /// <para>横移動の長押し処理を行います</para>
    /// </summary>
    /// <param name="x"></param>
    private void AutoRepeat(int x)
    {
        _autoRepeatTimer += Time.deltaTime; //タイマー加算
                                            //タイマーが待機時間を越した
        if (AUTOREPEAT_MOVETIME <= _autoRepeatTimer)
        {
            //移動
            _minoUnion.Move(x);
            //タイマー初期化
            _autoRepeatTimer = 0;
        }
    }

    /// <summary>
    /// <para>AutoRepeatReset</para>
    /// <para>長押し処理の変数の初期化を行います</para>
    /// </summary>
    private void AutoRepeatReset()
    {
        //初期化
        _autoRepeatTimer = 0;
        _autoRepeatDire = 0;
    }

    /// <summary>
    /// <para>SetStopInput</para>
    /// <para>プレイヤー入力を受け付けないようにします</para>
    /// </summary>
    public void SetStopInput()
    {
        _canGame = false; //プレイ不可能に
    }
    #endregion
}
