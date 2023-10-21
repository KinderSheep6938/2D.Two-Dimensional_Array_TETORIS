// -------------------------------------------------------------------------------
// IMinoUnionCtrl.Interface
//
// �쐬��: 2023/10/19
// �쐬��: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMinoUnionCtrl
{
    /// <summary>
    /// <para>Move</para>
    /// <para>�~�m���ړ������܂�</para>
    /// </summary>
    /// <param name="x">�ړ�����</param>
    void Move(int x);

    /// <summary>
    /// <para>Rotate</para>
    /// <para>�~�m����]�����܂�</para>
    /// </summary>
    /// <param name="angle">��]����</param>
    void Rotate(int angle);
}