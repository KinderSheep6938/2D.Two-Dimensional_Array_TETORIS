// -------------------------------------------------------------------------------
// FieldManager.cs
//
// �쐬��: 2023/10/17
// �쐬��: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour, IFieldAccess
{
    #region �ϐ�
    //���X�g�Ǘ��萔
    private const int FIELD_MAX_WIDTH = 10; //�t�B�[���h�̉���
    private const int FIELD_MAX_HEIGHT = 25; //�t�B�[���h�̏c��
    private const int FIELD_VIEW_HEIGHT = 21; //�t�B�[���h�̍ő�\���c��
    private const int TILE_NONE_ID = 0; //�t�B�[���h�̋�ID
    private const int TILE_MINO_ID = 1; //�~�mID
    private const int MAX_COMMITMINO_CNT = 4;

    [SerializeField]
    private GameObject _fieldTileObj = default; //�󔒃^�C��

    private int[,] _field = new int[FIELD_MAX_HEIGHT,FIELD_MAX_WIDTH]; //�t�B�[���h�ۑ� [�c��:y,����:x]
    private List<int> _deleteLineIndexs = new(); //�������C����Index�ۑ��p
    private bool _isLine = false; //�������C������
    private int _fallValue = 0; //���C���폜�̗�������
    private int _commitCnt = 0; //�ݒu��������~�m�u���b�N�̐�



    private Transform _transform = default;
    #endregion

    #region ���\�b�h
    /// <summary>
    /// ����������
    /// </summary>
    void Awake()
    {
        //�ϐ�������
        _transform = transform;

        //�}�b�v������
        for(int y = 0; y < FIELD_MAX_HEIGHT; y++) //�c��
        {
            for(int x = 0; x < FIELD_MAX_WIDTH; x++) //����
            { 
                //�󔒂ŏ�����
                _field[y, x] = TILE_NONE_ID;
                //�󔒃^�C���ݒu
                if(y < FIELD_VIEW_HEIGHT)
                {
                    //�󔒃^�C��,�����ʒu,�p�x,�Ǘ��p�I�u�W�F
                    Instantiate(
                        _fieldTileObj,
                        Vector3.right * x + Vector3.up * y,
                        Quaternion.identity,
                        _transform
                        );
                }
            }
        }
        //���C������������
        _deleteLineIndexs.Clear();
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
    /// <para>CheckLine</para>
    /// <para>�t�B�[���h�̒��Ń��C�����ł��Ă��邩�������܂�</para>
    /// </summary>
    private void CheckLine()
    {
        //������
        _deleteLineIndexs.Clear();

        //���C������
        for (int y = 0; y < FIELD_MAX_HEIGHT; y++) //�c��
        {
            //���C�����菉����
            _isLine = true;

            //��񌟍�
            for (int x = 0; x < FIELD_MAX_WIDTH; x++) //����
            {
                //���̒��ɋ󔒂����邩
                if(_field[y,x] == TILE_NONE_ID) 
                {
                    //���C���Ƃ��Ċ������Ă��Ȃ�
                    _isLine = false;
                    break;
                }
            }

            //���C�����������Ă��邩
            if (_isLine)
            {
                _deleteLineIndexs.Add(y); //�������Ă��郉�C�����폜�ΏۂƂ��ĕۑ�
            }
        }

        //�폜�Ώۂ���ȏ゠��ꍇ�͍폜�������s��
        if(_deleteLineIndexs.Count != 0) { DeleteLine(); }

        return;
    }

    /// <summary>
    /// <para>DeleteLine</para>
    /// <para>�w�肵�����C�����폜���܂�</para>
    /// <para>�܂��A�폜������������̃~�m�𗎉������܂�</para>
    /// </summary>
    private void DeleteLine()
    {
        //������
        _fallValue = 0;

        //�񎟌��z�񑀍�
        for (int y = 0; y < FIELD_MAX_HEIGHT; y++) //�c��
        {
            //�Y�����������̂ݏ��������s����
            for (int x = 0; x < FIELD_MAX_WIDTH; x++) //����
            {
                //�폜�Ώۂ̃��C���ł���
                if (_deleteLineIndexs.Contains(y))
                {
                    //�󔒂Ŗ��߂�
                    _field[y, x] = TILE_NONE_ID;
                    continue;
                }

                //�����������ݒ肳��Ă���
                if(0 < _fallValue)
                {
                    //���݂̏󋵂𗎉���Ɉړ�������
                    _field[y - _fallValue, x] = _field[y, x];
                    //�����O�̍��W�ɋ󔒂�ݒ肷��
                    _field[y, x] = TILE_NONE_ID;
                    continue;
                }
            }

            //������������
            if (_deleteLineIndexs.Contains(y)) { _fallValue++; }
        }

        //�t�B�[���h�ɑ��݂���~�m�ɍ폜�����Ɨ������������s������
        foreach (ILineMinoCtrl lineMino in FindObjectsOfType<Mino>())
        {
            lineMino.LineCtrl(_deleteLineIndexs);
        }
        Debug.Log("------------");
        return;
    }

    /// <summary>
    /// <para>GetCommitStatus</para>
    /// <para>���쒆�̃~�m���ݒu���ꂽ���ǂ����������܂�</para>
    /// </summary>
    /// <returns>���슮������</returns>
    public bool GetCommitStatus()
    {
        //�ݒu������ �� ����\�ȃ~�m�̍ő吔 ��葽����
        if(MAX_COMMITMINO_CNT <= _commitCnt)
        {
            //�J�E���g���Z�b�g
            _commitCnt = 0;
            //�ݒu���������Ă��邱�Ƃ�Ԃ�
            return true;
        }

        //�܂����삵�Ă���
        return false;
    }
    
    //�C���^�[�t�F�C�X�p��
    public bool CheckAlreadyMinoExist(int x,int y)
    {
        //�t�B�[���h�O(�㉺���E) �܂��� ���Ƀ~�m�����݂���
        if (x < 0 || y < 0 || FIELD_MAX_WIDTH <= x || FIELD_MAX_HEIGHT <= y || _field[y, x] == TILE_MINO_ID) { return true; /*�󔒂ł͂Ȃ�*/ }

        //�󔒂ł���
        return false;
    }

    //�C���^�[�t�F�C�X�p��
    public void SetMino(int x,int y)
    {
        //�~�m�ݒ�
        _field[y, x] = TILE_MINO_ID;
        //�R�~�b�g��������~�m�̐��𑝕�
        _commitCnt++;

        //���앪�̃R�~�b�g�����ꂽ���A���C���`�F�b�N������
        if(MAX_COMMITMINO_CNT <= _commitCnt)
        {
            CheckLine();
        }
        return;
    }
    #endregion
}
