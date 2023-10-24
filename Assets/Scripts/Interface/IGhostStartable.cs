// -------------------------------------------------------------------------------
// IGhostStartable.Interface
//
// �쐬��: 2023/10/24
// �쐬��: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGhostStartable
{
    /// <summary>
    /// <para>ChangeTransformGhost</para>
    /// <para>����~�m�̈ʒu�A�p�x�ɕύX�����������ɌĂяo����܂�</para>
    /// </summary>
    /// <param name="playableMino">����~�m</param>
    void ChangeTransformGhost(Transform playableMino);

    /// <summary>
    /// <para>ChangeModelGhost</para>
    /// <para>����~�m�̃~�m�`�ɕύX�����������ɌĂяo����܂�</para>
    /// </summary>
    /// <param name="playableMino">����~�m</param>
    void ChangeModelGhost(IMinoCreatable.MinoType playableMino);
}
