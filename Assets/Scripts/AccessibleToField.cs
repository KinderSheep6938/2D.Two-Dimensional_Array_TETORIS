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
    public virtual bool CheckMino()
    {   
        //フィールド管理システムを取得していない
        if(_fieldCtrl == default) { _fieldCtrl = FindObjectOfType<FieldManager>().GetComponent<IFieldAccess>(); /*取得*/}

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
    /// <para>SetMinoForField</para>
    /// <para>ミノをフィールドにセットします</para>
    /// </summary>
    public virtual void SetMinoForField()
    {
        foreach (IMinoBlockAccessible mino in Minos)
        {
            //親子関係削除
            mino.DisConnect();
            //コミット
            _fieldCtrl.SetMino(mino.MinoX, mino.MinoY);
        }
    }
    #endregion
}
