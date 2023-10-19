// -------------------------------------------------------------------------------
// IMinoInfo.Interface
//
// �쐬��: 2023/10/17
// �쐬��: Shizuku
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
    /// <para>�~�m�̐F��ݒ肵�܂�</para>
    /// </summary>
    /// <param name="minoColor">�ݒ肳���F</param>
    void ChangeColor(Color minoColor);

    /// <summary>
    /// <para>SetMinoPos</para>
    /// <para>�~�m�u���b�N���w�肵���l���ړ����܂�</para>
    /// <para>�܂��A�w�肵��Transform��e�i�~�m���j�Ƃ��Đݒ肵�܂�</para>
    /// </summary>
    /// <param name="x">�ړ����鉡��</param>
    /// <param name="y">�ړ�����c��</param>
    /// <param name="parent">�~�m��</param>
    void SetMinoPos(float x, float y, Transform parent);

    /// <summary>
    /// <para>DisConnectParent</para>
    /// <para>�~�m���Ƃ̐ڑ���ؒf���܂�</para>
    /// </summary>
    void DisConnectParent();
}
