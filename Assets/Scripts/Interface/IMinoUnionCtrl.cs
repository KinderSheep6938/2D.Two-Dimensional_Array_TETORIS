// -------------------------------------------------------------------------------
// IMinoUnionCtrl.Interface
//
// 作成日: 2023/10/19
// 作成者: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMinoUnionCtrl
{
    /// <summary>
    /// <para>Move</para>
    /// <para>ミノを移動させます</para>
    /// </summary>
    /// <param name="x">移動方向</param>
    void Move(int x);

    /// <summary>
    /// <para>Rotate</para>
    /// <para>ミノを回転させます</para>
    /// </summary>
    /// <param name="dire">回転方向</param>
    void Rotate(int dire);

    /// <summary>
    /// <para>HardDrop</para>
    /// <para>ミノを急降下させます</para>
    /// </summary>
    void HardDrop();

    /// <summary>
    /// <para>SoftDrop</para>
    /// <para>ミノを素早く落下させます</para>
    /// </summary>
    void SoftDrop();

    /// <summary>
    /// <para>MinoHold</para>
    /// <para>ミノをホールド設定します</para>
    /// </summary>
    bool CheckHasHold();
}
