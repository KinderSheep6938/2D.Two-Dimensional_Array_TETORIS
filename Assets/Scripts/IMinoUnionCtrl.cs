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
    /// <param name="angle">回転方向</param>
    void Rotate(int angle);
}
