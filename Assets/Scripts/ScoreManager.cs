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
    private const int MINFALLTIME_LEVEL = 20; //�ő��������Ԃ̃��x��
    private const int BASE_SCORE = 100; //��b�X�R�A
    private const float BASE_FALLTIME = 0.8f; //��b��������
    private const float MAXLEVEL_FALLTIME = 0.03f; //�ő僌�x����������
    //���x���㏸���Ɍ������闎������
    //�������� = (��b�������� - �ő僌�x����������) / �ő僌�x��
    private readonly float _levelUpRatio = (BASE_FALLTIME - MAXLEVEL_FALLTIME) / MINFALLTIME_LEVEL;

    private int _score = 0; //���݃X�R�A
    private int _clearLine = 0; //�폜�������C���̒l
    private int _oldLevel = 0; //���x���㏸�����p


    [SerializeField, Header("����~�m")]
    private PlayableMino _playableMino = default;
    #endregion

    #region �v���p�e�B
    public int Level { get => 1 + (_clearLine / LEVELUP_BORDERLINE); }
    public int Score { get => _score; }

    public int LevelBorder { get => LEVELUP_BORDERLINE; }
    #endregion

    #region ���\�b�h
    /// <summary>
    /// ����������
    /// </summary>
    void Awake()
    {
        //�������x�ݒ�
        _playableMino.FallTime = LevelOfFallTime();
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

        //���x�����ύX���ꂽ �܂��� ���x�����ő��������Ԃ̃��x����艺�ł���
        if(_oldLevel != Level || Level <= MINFALLTIME_LEVEL)
        {
            _oldLevel = Level; //���݂̃��x����ۑ�
            _playableMino.FallTime = LevelOfFallTime(); //���x�ݒ�
        }
    }

    /// <summary>
    /// <para>LevelOfFallTime</para>
    /// <para>���x���ɉ������������Ԃ��擾����</para>
    /// </summary>
    /// <returns>���x���ɑΉ�������������</returns>
    private float LevelOfFallTime()
    {
        //���x�����ŏ��������ԃ��x���ȉ��ł���
        if(Level < MINFALLTIME_LEVEL)
        {
            //�Ή��������x���̗������Ԃ�Ԃ�
            //�Ή������������� �� ��b�������� - (���x�����̌������ԗ� * ���݂̃��x��)
            return BASE_FALLTIME - _levelUpRatio * Level;
        }

        //�ŏ��������Ԃ�Ԃ�
        return MAXLEVEL_FALLTIME;
    }
    #endregion
}
