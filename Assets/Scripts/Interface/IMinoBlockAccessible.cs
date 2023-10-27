// -------------------------------------------------------------------------------
// IMinoInfo.Interface
//
// 作成日: 2023/10/17
// 作成者: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMinoBlockAccessible
{

    int MinoX { get; } //自身の横軸座標

    int MinoY { get; } //自身の縦軸座標

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
    void DisConnect();

    /// <summary>
    /// <para>SetMinoView</para>
    /// <para>ミノブロックの表示状態を設定します</para>
    /// </summary>
    /// <param name="canShow">表示設定</param>
    void SetMinoView(bool canShow);
}
