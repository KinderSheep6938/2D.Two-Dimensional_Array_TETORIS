// -------------------------------------------------------------------------------
// NextMinoView.cs
//
// �쐬��: 2023/10/24
// �쐬��: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextMino : MinoModelGeneration
{
    #region �ϐ�
    private IMinoCreatable _playableMino = default;
    private bool isSet = false;
    #endregion

    #region ���\�b�h
    /// <summary>
    /// ����������
    /// </summary>
    void Awake()
    {
        //������
        _playableMino = transform.parent.GetComponentInChildren<PlayableMino>().GetComponent<IMinoCreatable>();
    }

    //�N���X�p��
    public override void CreateMinoUnit(IMinoBlockAccessible[] minoBlocks, IMinoCreatable.MinoType setModel)
    {
        if(isSet)
        {
            _playableMino.CreateMinoUnit(Minos, MyModel);
        }

        base.CreateMinoUnit(minoBlocks, setModel);
        isSet = true;
    }
    #endregion
}
