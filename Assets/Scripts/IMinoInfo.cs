// -------------------------------------------------------------------------------
// IMinoInfo.Interface
//
// 作成日: 2023/10/17
// 作成者: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMinoInfo
{
    int MinoX { get; }
    int MinoY { get; }

    /// <summary>
    /// <para>ChangeColor</para>
    /// <para>ミノの色を設定します</para>
    /// </summary>
    /// <param name="minoColor">設定される色</param>
    void ChangeColor(Color minoColor);

    /// <summary>
    /// <para>SetMinoPos</para>
    /// <para>ミノブロックを指定した値分移動します</para>
    /// <para>また、指定したTransformを親（ミノ軸）として設定します</para>
    /// </summary>
    /// <param name="x">移動する横軸</param>
    /// <param name="y">移動する縦軸</param>
    /// <param name="parent">ミノ軸</param>
    void SetMinoPos(float x, float y, Transform parent);

    /// <summary>
    /// <para>DisConnectParent</para>
    /// <para>ミノ軸との接続を切断します</para>
    /// </summary>
    void DisConnectParent();
}
