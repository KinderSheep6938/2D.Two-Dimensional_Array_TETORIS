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
    public const int MAX_MINO_CNT = 4; //最大ミノブロック数
    public const float EXCEPTION_SHIFT_0_5 = 0.5f; //ミノ形生成用0.5差分
    public const float EXCEPTION_SHIFT_1_0 = 1.0f; //ミノ形生成用1.0差分
    public const float EXCEPTION_SHIFT_1_5 = 1.5f; //ミノ形生成用1.5差分

    enum MinoType //ミノ形
    {
        minoO,
        minoS,
        minoZ,
        minoJ,
        minoL,
        minoT,
        minoI
    }

    MinoType MyModel { get; } //自身のミノ形

    IMinoBlockAccessible[] Minos { get; } //自身のミノブロック

    /// <summary>
    /// <para>CreateMinoUnit</para>
    /// <para>指定されたミノ形にミノブロックを設定します</para>
    /// </summary>
    /// <param name="minoBlocks">使用するミノ</param>
    /// <param name="setModel">変形するミノ形</param>
    void CreateMinoUnit(IMinoBlockAccessible[] minoBlocks,MinoType setModel);
}
