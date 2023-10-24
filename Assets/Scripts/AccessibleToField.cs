// -------------------------------------------------------------------------------
// AccessibleToField.cs
//
// �쐬��: 2023/10/24
// �쐬��: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessibleToField : MinoModelGeneration
{
    #region �ϐ�
    private IFieldAccess _fieldCtrl = default; //�t�B�[���h�Ǘ��V�X�e���̃C���^�[�t�F�C�X
    #endregion

    #region �v���p�e�B

    #endregion

    #region ���\�b�h
    /// <summary>
    /// <para>CheckMino</para>
    /// <para>�~�m�u���b�N�̏Փ˂��`�F�b�N���܂�</para>
    /// </summary>
    /// <returns>�Փ˔���</returns>
    public virtual bool CheckMino()
    {   
        //�t�B�[���h�Ǘ��V�X�e�����擾���Ă��Ȃ�
        if(_fieldCtrl == default) { _fieldCtrl = FindObjectOfType<FieldManager>().GetComponent<IFieldAccess>(); /*�擾*/}

        //�~�m�u���b�N���Փ˂��ĂȂ���
        foreach (IMinoBlockAccessible mino in Minos)
        {
            //�󔒂ł͂Ȃ�
            if (_fieldCtrl.CheckAlreadyMinoExist(mino.MinoX, mino.MinoY)) { return true; }
        }
        //�󔒂ł���
        return false;
    }

    /// <summary>
    /// <para>SetMinoForField</para>
    /// <para>�~�m���t�B�[���h�ɃZ�b�g���܂�</para>
    /// </summary>
    public virtual void SetMinoForField()
    {
        foreach (IMinoBlockAccessible mino in Minos)
        {
            //�e�q�֌W�폜
            mino.DisConnect();
            //�R�~�b�g
            _fieldCtrl.SetMino(mino.MinoX, mino.MinoY);
        }
    }
    #endregion
}
