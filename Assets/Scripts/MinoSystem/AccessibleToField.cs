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
    private int NowX { get => (int)MyTransform.position.x; }
    private int NowY { get => (int)MyTransform.position.y; }
    #endregion

    #region メソッド
    /// <summary>
    /// <para>GetFieldManager</para>
    /// <para>フィールドマネージャーを取得します</para>
    /// <para>既に取得している場合は何もしません</para>
    /// </summary>
    private void GetFieldManager()
    {
        //設定されている場合は、処理終了
        if(_fieldCtrl != default) { return; }
        //取得
        _fieldCtrl = FindObjectOfType<FieldManager>().GetComponent<IFieldAccess>();
    }

    /// <summary>
    /// <para>CheckCollisionByCenter</para>
    /// <para>ミノ軸を中心に衝突判定をチェックします</para>
    /// <para>増分がある場合は、中心＋増分の座標を検査します</para>
    /// </summary>
    /// <param name="x">横軸増分</param>
    /// <param name="y">縦軸増分</param>
    /// <returns>衝突判定</returns>
    public bool CheckCollisionByCenter(int x, int y)
    {
        GetFieldManager(); //マネージャー確認

        //空白ではない
        if (_fieldCtrl.CheckAlreadyMinoExist(NowX + x, NowY + y)) { return true; }
        //空白である
        return false;
    }
    /// <summary>
    /// <para>CheckMino</para>
    /// <para>ミノブロックの衝突をチェックします</para>
    /// </summary>
    /// <returns>衝突判定</returns>
    public bool CheckMinoCollision()
    {
        GetFieldManager(); //マネージャー確認

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
        GetFieldManager(); //マネージャー確認

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
        GetFieldManager(); //マネージャー確認

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

    /// <summary>
    /// <para>SetTSpin</para>
    /// <para>Tスピン判定を設定します</para>
    /// </summary>
    /// <param name="flag"></param>
    public void SetTSpin(bool flag)
    {
        //判定設定
        _fieldCtrl.TSpin = flag;
    }
    #endregion
}
