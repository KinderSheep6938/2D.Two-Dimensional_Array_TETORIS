// -------------------------------------------------------------------------------
// ILineMinoCtrl.Interface
//
// 作成日: 2023/10/17
// 作成者: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILineMinoCtrl
{
    /// <summary>
    /// <para>LineCtrl</para>
    /// <para>削除対象のラインに応じて、削除処理・落下処理を行います</para>
    /// </summary>
    /// <param name="deleteLineHeights">削除対処のラインのリスト</param>
    void LineCtrl(List<int> deleteLineHeights);
}
