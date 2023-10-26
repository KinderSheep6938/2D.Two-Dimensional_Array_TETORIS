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
    public int NowX { get => (int)MyTransform.position.x; }
    public int NowY { get => (int)MyTransform.position.y; }
    #endregion

    #region ���\�b�h
    /// <summary>
    /// <para>CheckCollisionByCenter</para>
    /// <para>�~�m���𒆐S�ɏՓ˔�����`�F�b�N���܂�</para>
    /// <para>����������ꍇ�́A���S�{�����̍��W���������܂�</para>
    /// </summary>
    /// <param name="x">��������</param>
    /// <param name="y">�c������</param>
    /// <returns>�Փ˔���</returns>
    public bool CheckCollisionByCenter(int x, int y)
    {
        //�t�B�[���h�Ǘ��V�X�e�����擾���Ă��Ȃ�
        if (_fieldCtrl == default) { _fieldCtrl = FindObjectOfType<FieldManager>().GetComponent<IFieldAccess>(); /*�擾*/}

        //�󔒂ł͂Ȃ�
        if (_fieldCtrl.CheckAlreadyMinoExist(NowX + x, NowY + y)) { return true; }
        //�󔒂ł���
        return false;
    }
    /// <summary>
    /// <para>CheckMino</para>
    /// <para>�~�m�u���b�N�̏Փ˂��`�F�b�N���܂�</para>
    /// </summary>
    /// <returns>�Փ˔���</returns>
    public bool CheckMinoCollision()
    {
        //�t�B�[���h�Ǘ��V�X�e�����擾���Ă��Ȃ�
        if (_fieldCtrl == default) { _fieldCtrl = FindObjectOfType<FieldManager>().GetComponent<IFieldAccess>(); /*�擾*/}

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
    /// <para>CheckMino</para>
    /// <para>�~�m�u���b�N�̏Փ˂��`�F�b�N���܂�</para>
    /// </summary>
    /// <param name="addX">����������</param>
    /// <param name="addY">�c��������</param>
    /// <returns>�Փ˔���</returns>
    public bool CheckMinoCollision(int addX,int addY)
    {   
        //�t�B�[���h�Ǘ��V�X�e�����擾���Ă��Ȃ�
        if(_fieldCtrl == default) { _fieldCtrl = FindObjectOfType<FieldManager>().GetComponent<IFieldAccess>(); /*�擾*/}

        //�~�m�u���b�N���Փ˂��ĂȂ���
        foreach (IMinoBlockAccessible mino in Minos)
        {
            //�󔒂ł͂Ȃ�
            if (_fieldCtrl.CheckAlreadyMinoExist(mino.MinoX + addX, mino.MinoY + addY)) { return true; }
        }
        //�󔒂ł���
        return false;
    }

    /// <summary>
    /// <para>SetMinoForField</para>
    /// <para>�~�m���t�B�[���h�ɃZ�b�g���܂�</para>
    /// </summary>
    public void SetMinoForField()
    {
        foreach (IMinoBlockAccessible mino in Minos)
        {
            //�e�q�֌W�폜
            mino.DisConnect();
            //�R�~�b�g
            _fieldCtrl.SetMino(mino.MinoX, mino.MinoY);
        }
    }

    /// <summary>
    /// <para>NotPlay</para>
    /// <para>�v���C�s�\��Ԃł��邱�Ƃ�ݒ肵�܂�</para>
    /// </summary>
    public void NotPlay()
    {
        //�v���C�s�\��Ԃ�ݒ�
        _fieldCtrl.NotPlayable();
    }

    /// <summary>
    /// <para>SetTSpin</para>
    /// <para>T�X�s�������ݒ肵�܂�</para>
    /// </summary>
    /// <param name="flag"></param>
    public void SetTSpin(bool flag)
    {
        //����ݒ�
        _fieldCtrl.TSpin = flag;
    }
    #endregion
}
