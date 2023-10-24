// -------------------------------------------------------------------------------
// IFieldAccess.Interface
//
// �쐬��: 2023/10/17
// �쐬��: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFieldAccess
{
    /// <summary>
    /// <para>CheckAlreadyMinoExist</para>
    /// <para>�w�肵�����W���󔒂��ǂ����������܂�</para>
    /// </summary>
    /// <param name="x">�������W�̉���</param>
    /// <param name="y">�������W�̏c��</param>
    /// <returns>�󔒏�</returns>
    bool CheckAlreadyMinoExist(int x, int y);

    /// <summary>
    /// <para>SetMino</para>
    /// <para>�w�肵�����W�Ƀ~�m��ݒ肵�܂�</para>
    /// </summary>
    /// <param name="x">�ݒ���W�̉���</param>
    /// <param name="y">�ݒ���W�̏c��</param>
    void SetMino(int x,int y);
}
