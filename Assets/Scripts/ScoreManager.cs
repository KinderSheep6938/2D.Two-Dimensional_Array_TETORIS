// -------------------------------------------------------------------------------
// ScoreManager.cs
//
// �쐬��: 2023/10/24
// �쐬��: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    #region �ϐ�
    private const int LEVELUP_BORDERLINE = 10; //���x���㏸�̂������l
    private const int BASE_SCORE = 100; //��b�X�R�A

    private int _score = 0; //���݃X�R�A
    private int _clearLine = 0; //�폜�������C���̒l


    #endregion

    #region �v���p�e�B
    public int Level { get => _clearLine / LEVELUP_BORDERLINE; }
    public int Score { get => _score; }
    #endregion

    #region ���\�b�h
    /// <summary>
    /// ����������
    /// </summary>
    void Awake()
    {
        
    }

    /// <summary>
    /// <para>AddScore</para>
    /// <para>���������C���ɉ����āA�X�R�A�����Z���܂�</para>
    /// </summary>
    /// <param name="deleteLine">�폜�������C��</param>
    public void AddScore(int deleteLine)
    {
        //�X�R�A���Z
        _score += deleteLine * deleteLine * Level * BASE_SCORE;

        //���������C�������Z
        _clearLine += deleteLine;
    }
    #endregion
}
