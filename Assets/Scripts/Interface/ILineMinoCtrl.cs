// -------------------------------------------------------------------------------
// ILineMinoCtrl.Interface
//
// �쐬��: 2023/10/17
// �쐬��: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILineMinoCtrl
{
    /// <summary>
    /// <para>LineCtrl</para>
    /// <para>�폜�Ώۂ̃��C���ɉ����āA�폜�����E�����������s���܂�</para>
    /// </summary>
    /// <param name="deleteLineHeights">�폜�Ώ��̃��C���̃��X�g</param>
    void LineCtrl(List<int> deleteLineHeights);
}
