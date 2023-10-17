// -------------------------------------------------------------------------------
// FieldManager.cs
//
// �쐬��: 2023/10/17
// �쐬��: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour, IFieldCtrl
{
    #region �ϐ�
    //���X�g�Ǘ��萔
    const int FIELD_MAX_WIDTH = 10; //�t�B�[���h�̉���
    const int FIELD_MAX_HEIGHT = 20; //�t�B�[���h�̏c��
    const int TILE_NONE_ID = 0; //�t�B�[���h�̋�ID
    const int TILE_MINO_ID = 1; //�~�mID

    private int[,] _field = new int[FIELD_MAX_HEIGHT,FIELD_MAX_WIDTH]; //�t�B�[���h�ۑ� [�c��:y,����:x]

    #endregion

    #region ���\�b�h
    /// <summary>
    /// ����������
    /// </summary>
    void Awake()
    {
        //�}�b�v������
        for(int y = 0; y < FIELD_MAX_HEIGHT; y++) //�c��
        { 
            for(int x = 0; x < FIELD_MAX_WIDTH; x++) //����
            { 
                //�󔒂ŏ�����
                _field[y, x] = TILE_NONE_ID;
            }
        }
    }

    /// <summary>
    /// �X�V�O����
    /// </summary>
    void Start()
    {
        
    }

    /// <summary>
    /// �X�V����
    /// </summary>
    void Update()
    {
        
    }
    
    /// <summary>
    /// <para>CheckAlreadyMinoExist</para>
    /// <para>�w�肵�����W���󔒂��ǂ����������܂�</para>
    /// </summary>
    /// <param name="x">�������W�̉���</param>
    /// <param name="y">�������W�̏c��</param>
    /// <returns>�󔒏�</returns>
    public bool CheckAlreadyMinoExist(int x,int y)
    {
        //�t�B�[���h�O(�㉺���E) �܂��� ���Ƀ~�m�����݂���
        if (x < 0 || y < 0 || FIELD_MAX_WIDTH <= x || FIELD_MAX_HEIGHT <= y || _field[y, x] == TILE_MINO_ID) { return true; /*�󔒂ł͂Ȃ�*/ }

        //�󔒂ł���
        return false;
    }

    /// <summary>
    /// <para>SetMino</para>
    /// <para>�w�肵�����W�Ƀ~�m��ݒ肵�܂�</para>
    /// </summary>
    /// <param name="x">�ݒ���W�̉���</param>
    /// <param name="y">�ݒ���W�̏c��</param>
    public void SetMino(int x,int y)
    {
        //�~�m�ݒ�
        _field[y, x] = TILE_MINO_ID;
        return;
    }
    #endregion
}
