// -------------------------------------------------------------------------------
// IMinoUnionCtrl.Interface
//
// �쐬��: 2023/10/18
// �쐬��: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMinoCreatable
{
    const int MAX_MINO_CNT = 4; //�ő�~�m�u���b�N��
    const float EXCEPTION_SHIFT_0_5 = 0.5f; //�~�m�`�����p0.5����
    const float EXCEPTION_SHIFT_1_0 = 1.0f; //�~�m�`�����p1.0����
    const float EXCEPTION_SHIFT_1_5 = 1.5f; //�~�m�`�����p1.5����

    enum MinoType //�~�m�`
    {
        minoO,
        minoS,
        minoZ,
        minoJ,
        minoL,
        minoT,
        minoI
    }

    MinoType MyModel { get; }

    IMinoBlockAccessible[] Minos { get; }

    /// <summary>
    /// <para>CreateMinoUnit</para>
    /// <para>�w�肳�ꂽ�~�m�`�Ƀ~�m�u���b�N��ݒ肵�܂�</para>
    /// </summary>
    /// <param name="setModel">�ό`����~�m�`</param>
    void CreateMinoUnit(IMinoBlockAccessible[] minoBlocks,MinoType setModel);
}
