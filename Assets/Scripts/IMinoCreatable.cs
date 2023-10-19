// -------------------------------------------------------------------------------
// IMinoUnionCtrl.Interface
//
// 作成日: 2023/10/18
// 作成者: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMinoCreatable
{
    enum MinoType
    {
        minoO,
        minoS,
        minoZ,
        minoJ,
        minoL,
        minoT,
        minoI
    }
    MinoType MyModel { get; set; }

    /// <summary>
    /// <para>CreateMinoUnit</para>
    /// <para>指定されたミノ形にミノブロックを設定します</para>
    /// </summary>
    /// <param name="setModel">変形するミノ形</param>
    void CreateMinoUnit(IMinoInfo[] minoBlocks,MinoType setModel);

    /// <summary>
    /// <para>SetUnionPlayable</para>
    /// <para>ミノユニットを操作可能にします</para>
    /// </summary>
    void SetUnionPlayable();
}
