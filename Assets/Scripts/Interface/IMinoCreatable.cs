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
    public const int MAX_MINO_CNT = 4; //�ő�~�m�u���b�N��
    public const float EXCEPTION_SHIFT_0_5 = 0.5f; //�~�m�`�����p0.5����
    public const float EXCEPTION_SHIFT_1_0 = 1.0f; //�~�m�`�����p1.0����
    public const float EXCEPTION_SHIFT_1_5 = 1.5f; //�~�m�`�����p1.5����

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

    MinoType MyModel { get; } //���g�̃~�m�`

    IMinoBlockAccessible[] Minos { get; } //���g�̃~�m�u���b�N

    /// <summary>
    /// <para>CreateMinoUnit</para>
    /// <para>�w�肳�ꂽ�~�m�`�Ƀ~�m�u���b�N��ݒ肵�܂�</para>
    /// </summary>
    /// <param name="minoBlocks">�g�p����~�m</param>
    /// <param name="setModel">�ό`����~�m�`</param>
    void CreateMinoUnit(IMinoBlockAccessible[] minoBlocks,MinoType setModel);
}
