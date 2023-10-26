// -------------------------------------------------------------------------------
// IFieldAccess.Interface
//
// 作成日: 2023/10/17
// 作成者: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFieldAccess
{
    bool TSpin { set; }

    /// <summary>
    /// <para>CheckAlreadyMinoExist</para>
    /// <para>指定した座標が空白かどうか検査します</para>
    /// </summary>
    /// <param name="x">検査座標の横軸</param>
    /// <param name="y">検査座標の縦軸</param>
    /// <returns>空白状況</returns>
    bool CheckAlreadyMinoExist(int x, int y);

    /// <summary>
    /// <para>SetMino</para>
    /// <para>指定した座標にミノを設定します</para>
    /// </summary>
    /// <param name="x">設定座標の横軸</param>
    /// <param name="y">設定座標の縦軸</param>
    void SetMino(int x,int y);

    /// <summary>
    /// <para>NotPlayable</para>
    /// <para>このフィールドでプレイが不可能であることを設定します</para>
    /// </summary>
    void NotPlayable();
}
