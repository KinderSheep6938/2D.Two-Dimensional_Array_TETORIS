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
    enum MinoType
    {
        minoO,
        minoS,
        minoZ,
        minoJ,
        minoL,
        minoT,
        minoI
    }
    MinoType MyModel { get; set; }

    /// <summary>
    /// <para>CreateMinoUnit</para>
    /// <para>�w�肳�ꂽ�~�m�`�Ƀ~�m�u���b�N��ݒ肵�܂�</para>
    /// </summary>
    /// <param name="setModel">�ό`����~�m�`</param>
    void CreateMinoUnit(IMinoInfo[] minoBlocks,MinoType setModel);

    /// <summary>
    /// <para>SetUnionPlayable</para>
    /// <para>�~�m���j�b�g�𑀍�\�ɂ��܂�</para>
    /// </summary>
    void SetUnionPlayable();
}
