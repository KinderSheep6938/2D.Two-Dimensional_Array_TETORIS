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
    /// <para>DownMino</para>
    /// <para>ミノを1列分下げます</para>
    /// </summary>
    void DownMino();

    /// <summary>
    /// <para>DeleteMino</para>
    /// <para>ミノを削除します</para>
    /// </summary>
    void DeleteMino();
}
