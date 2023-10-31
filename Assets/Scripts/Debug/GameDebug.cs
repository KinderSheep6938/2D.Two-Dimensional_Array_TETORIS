// -------------------------------------------------------------------------------
// GameDebug.cs
//
// 作成日: 2023/10/30
// 作成者: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameDebug : MonoBehaviour
{
    #region 変数
    const int FIELD_I_WIDTH = 9; //テトリス用フィールドの横幅
    const int FIELD_I_HEIGHT = 4; //テトリス用フィールドの横幅
    readonly int[] _fieldTWidth = {9,8,9 }; //Tスピン用フィールドの各横幅
    readonly Vector2 _fieldTEdge = new Vector2(9, 4); //Tスピン用フィールドの引っ掛かり
    readonly Color _debugMinoColor = Color.white / 2 + Color.black; //デバッグ用のミノカラー

    private Transform _transform = default;

    private FieldManager _fieldManager = default;
    private MinoPoolManager _minoPool = default;

    #endregion

    #region プロパティ

    #endregion

    #region メソッド
    /// <summary>
    /// 初期化処理
    /// </summary>
    void Awake()
    {
        //初期化
        _transform = transform;
        _fieldManager = GetComponentInParent<FieldManager>();
        _minoPool = GetComponentInParent<MinoPoolManager>();
    }

    /// <summary>
    /// <para>OnDebugTField</para>
    /// <para>デバッグ用のTスピンフィールドを形成します</para>
    /// </summary>
    /// <param name="context">ボタン状態</param>
    public void OnDebugTField(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && Input.GetKey(KeyCode.LeftControl)) //押された
        {
            //フィールド初期化
            _fieldManager.RemoveField();
            //プールから使用可能ミノ取得
            IMinoBlockAccessible useMino;
            int y = 0;
            //フィールド形成
            foreach(int maxWidth in _fieldTWidth)
            {
                for(int x = 0;x < maxWidth; x++)
                {
                    useMino = _minoPool.GetMinoByPool();
                    useMino.ChangeColor(_debugMinoColor);
                    useMino.SetMinoPos(x, y, _transform);
                    _fieldManager.SetMino(useMino.MinoX, useMino.MinoY);

                }
                y++;
            }
            //引っ掛かり設定
            useMino = _minoPool.GetMinoByPool();
            useMino.ChangeColor(_debugMinoColor);
            useMino.SetMinoPos(_fieldTEdge.x, _fieldTEdge.y, _transform);
            _fieldManager.SetMino(useMino.MinoX, useMino.MinoY);

            _fieldManager.DeleteCommit(); //コミットしないように
            //子付け解除
            foreach (IMinoBlockAccessible mino in _transform.GetComponentsInChildren<IMinoBlockAccessible>())
            {
                mino.DisConnect();
            }
            return; //生成終了
        }
    }

    /// <summary>
    /// <para>OnDebugIField</para>
    /// <para>デバッグ用のテトリスフィールドを形成します</para>
    /// </summary>
    /// <param name="context">ボタン状態</param>
    public void OnDebugIField(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && Input.GetKey(KeyCode.LeftControl)) //押された
        {
            //フィールド初期化
            _fieldManager.RemoveField();
            //プールから使用可能ミノ取得
            IMinoBlockAccessible useMino;
            //フィールド形成
            for(int y = 0; y < FIELD_I_HEIGHT; y++)
            {
                for(int x = 0; x < FIELD_I_WIDTH; x++)
                {
                    useMino = _minoPool.GetMinoByPool();
                    useMino.ChangeColor(_debugMinoColor);
                    useMino.SetMinoPos(x, y, _transform);
                    _fieldManager.SetMino(useMino.MinoX, useMino.MinoY);

                }
            }
            _fieldManager.DeleteCommit(); //コミットしないように
            //子付け解除
            foreach(IMinoBlockAccessible mino in _transform.GetComponentsInChildren<IMinoBlockAccessible>())
            {
                mino.DisConnect();
            }
            return; //生成終了
        }
    }
    /// <summary>
    /// <para>OnDebugLevelUp</para>
    /// <para>デバッグ用のレベルアップ処理を行います</para>
    /// </summary>
    /// <param name="context">ボタン状態</param>
    public void OnDebugLevelUp(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && Input.GetKey(KeyCode.LeftControl)) //押された
        {
            //レベルアップ
            _fieldManager.LevelUp();
            return;
        }
    }
    #endregion
}
