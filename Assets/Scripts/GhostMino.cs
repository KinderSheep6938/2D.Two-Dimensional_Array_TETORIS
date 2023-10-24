// -------------------------------------------------------------------------------
// GhostMino.cs
//
// �쐬��: 2023/10/24
// �쐬��: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GhostMino : AccessibleToField, IGhostStartable
{
    #region �ϐ�

    #endregion

    #region �v���p�e�B

    #endregion

    #region ���\�b�h
    //�C���^�[�t�F�C�X�p��
    public void ChangeTransformGhost(Transform playableMino)
    {
        //���쒆�̃~�m�̈ʒu�A�p�x���S�[�X�g�ɔ��f������
        MyTransform.position = playableMino.position;
        MyTransform.rotation = playableMino.rotation;
        //�S�[�X�g�𗎂Ƃ�
        Drop();
    }

    //�C���^�[�t�F�C�X�p��
    public void ChangeModelGhost(IMinoCreatable.MinoType playableMino)
    {
        //���쒆�̃~�m�̌`���S�[�X�g�ɔ��f������
        if(Minos.Length != 0) //�S�[�X�g�̃~�m���ݒ肳��Ă��邩
        {
            //�S�[�X�g�`��
            CreateMinoUnit(Minos, playableMino);
        }
        else //�ݒ肳��Ă��Ȃ�
        {
            //�q�t������Ă���~�m�u���b�N��ݒ肵�A�S�[�X�g�`��
            CreateMinoUnit(GetComponentsInChildren<IMinoBlockAccessible>(), playableMino);
        }

        //�S�[�X�g���Ƃ�
        Drop();
    }

    /// <summary>
    /// <para>Drop</para>
    /// <para>�~�m���}�~�������܂�</para>
    /// </summary>
    private void Drop()
    {
        //�ċA����

        //1�}�X���Ƃ�
        MyTransform.position += Vector3.down;

        //�Փ˔��肪���邩
        if (CheckMino())
        {
            //�P�}�X�߂�
            MyTransform.position += Vector3.up;
            return; //�ċA�����I��
        }

        //�Փ˔��肪����܂ōċA����
        Drop(); //�ċA
        return;
    }

    public override void CreateMinoUnit(IMinoBlockAccessible[] minoBlocks, IMinoCreatable.MinoType setModel)
    {
        base.CreateMinoUnit(minoBlocks, setModel);

        foreach(IMinoBlockAccessible mino in minoBlocks)
        {
            
        }
    }
    #endregion
}
