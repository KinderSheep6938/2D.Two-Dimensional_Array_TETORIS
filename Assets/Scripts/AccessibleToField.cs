// -------------------------------------------------------------------------------
// AccessibleToField.cs
//
// 作成日: 2023/10/24
// 作成者: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessibleToField : MinoModelGeneration
{
    #region 変数
    private IFieldAccess _fieldCtrl = default; //フィールド管理システムのインターフェイス
    #endregion

    #region プロパティ

    #endregion

    #region メソッド
    /// <summary>
    /// <para>CheckMino</para>
    /// <para>ミノブロックの衝突をチェックします</para>
    /// </summary>
    /// <returns>衝突判定</returns>
    public bool CheckMinoCollision()
    {
        //フィールド管理システムを取得していない
        if (_fieldCtrl == default) { _fieldCtrl = FindObjectOfType<FieldManager>().GetComponent<IFieldAccess>(); /*取得*/}

        //ミノブロックが衝突してないか
        foreach (IMinoBlockAccessible mino in Minos)
        {
            //空白ではない
            if (_fieldCtrl.CheckAlreadyMinoExist(mino.MinoX, mino.MinoY)) { return true; }
        }
        //空白である
        return false;
    }
    /// <summary>
    /// <para>CheckMino</para>
    /// <para>ミノブロックの衝突をチェックします</para>
    /// </summary>
    /// <param name="addX">横軸増加分</param>
    /// <param name="addY">縦軸増加分</param>
    /// <returns>衝突判定</returns>
    public bool CheckMinoCollision(int addX,int addY)
    {   
        //フィールド管理システムを取得していない
        if(_fieldCtrl == default) { _fieldCtrl = FindObjectOfType<FieldManager>().GetComponent<IFieldAccess>(); /*取得*/}

        //ミノブロックが衝突してないか
        foreach (IMinoBlockAccessible mino in Minos)
        {
            //空白ではない
            if (_fieldCtrl.CheckAlreadyMinoExist(mino.MinoX + addX, mino.MinoY + addY)) { return true; }
        }
        //空白である
        return false;
    }


    /// <summary>
    /// <para>SetMinoForField</para>
    /// <para>ミノをフィールドにセットします</para>
    /// </summary>
    public void SetMinoForField()
    {
        foreach (IMinoBlockAccessible mino in Minos)
        {
            //親子関係削除
            mino.DisConnect();
            //コミット
            _fieldCtrl.SetMino(mino.MinoX, mino.MinoY);
        }
    }

    /// <summary>
    /// <para>NotPlay</para>
    /// <para>プレイ不可能状態であることを設定します</para>
    /// </summary>
    public void NotPlay()
    {
        //プレイ不可能状態を設定
        _fieldCtrl.NotPlayable();
    }
    #endregion
}
