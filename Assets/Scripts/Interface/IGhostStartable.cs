// -------------------------------------------------------------------------------
// IGhostStartable.Interface
//
// 作成日: 2023/10/24
// 作成者: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGhostStartable
{
    /// <summary>
    /// <para>ChangeTransformGhost</para>
    /// <para>操作ミノの位置、角度に変更が生じた時に呼び出されます</para>
    /// </summary>
    /// <param name="playableMino">操作ミノ</param>
    void ChangeTransformGhost(Transform playableMino);

    /// <summary>
    /// <para>ChangeModelGhost</para>
    /// <para>操作ミノのミノ形に変更が生じた時に呼び出されます</para>
    /// </summary>
    /// <param name="playableMino">操作ミノ</param>
    void ChangeModelGhost(IMinoCreatable.MinoType playableMino);
}
