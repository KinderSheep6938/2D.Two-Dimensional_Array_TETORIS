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
    //�t�B�[���h�Ǘ��萔
    private const int FIELD_MAX_WIDTH = 10; //�t�B�[���h�̉���
    private const int FIELD_MAX_HEIGHT = 25; //�t�B�[���h�̏c��
    private const int FIELD_VIEW_HEIGHT = 21; //�t�B�[���h�̍ő�\���c��
    private const int TILE_NONE_ID = 0; //�t�B�[���h�̋�ID
    private const int TILE_MINO_ID = 1; //�~�mID
    //�Q�[���t���[����p
    private const int MAX_COMMITMINO_CNT = 4; //����~�m�̐�
    private const int GAMEEND_ID = -1; //�Q�[���s�\���ID
    private const int PLAYING_ID = 0; //�v���C�����ID
    private const int COMMIT_ID = 1; //���슮�����ID

    //���̑�
    private const int TETRIS_LINE = 4; //�e�g���X���胉�C����
    private const int TSPIN_SCORE_RATIO = 2; //T�X�s������̃X�R�A�{����

    private int[,] _field = new int[FIELD_MAX_HEIGHT,FIELD_MAX_WIDTH]; //�t�B�[���h�ۑ� [�c��:y,����:x]
    private List<int> _deleteLineIndexs = new(); //�������C����Index�ۑ��p
    private bool _isLine = false; //�������C������
    private int _fallValue = 0; //���C���폜�̗�������
    private int _commitCnt = 0; //�ݒu��������~�m�u���b�N�̐�
    private bool _canPlay = true; //�t�B�[���h�ł̃v���C�۔���
    private bool _tSpin = false; //T�X�s������

    [SerializeField, Header("�󔒃^�C��")]
    private GameObject _fieldTileObj = default; //�󔒃^�C��
    [SerializeField, Header("�t�B�[���h�Ǘ��I�u�W�F�N�g")]
    private Transform _fieldParent = default; //�t�B�[���h�Ǘ��I�u�W�F

    [SerializeField, Header("���C�������G�t�F�N�g")]
    private LineEffect[] _deleteLineEfe = default;
    [SerializeField, Header("�e�g���X�e�L�X�g")]
    private TextEffect _tetrisText = default;
    [SerializeField, Header("T�X�s���e�L�X�g")]
    private TextEffect _tSpinText = default;
    [SerializeField, Header("���C������SE 0:�ʏ� 1:�e�g���X")]
    private AudioClip[] _deleteLineSE = default;
    private AudioSource _myAudio = default; //���g��AudioSource

    private ScoreManager _scoreManager = default; //�X�R�A�Ǘ��}�l�[�W���[
    #endregion

    #region �v���p�e�B
    public bool TSpin { set => _tSpin = value; }
    #endregion

    #region ���\�b�h
    /// <summary>
    /// ����������
    /// </summary>
    void Awake()
    {
        _canPlay = true;
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
                        _fieldParent
                        );
                }
            }
        }
        //���C������������
        _deleteLineIndexs.Clear();

        _myAudio = GetComponent<AudioSource>();
        _scoreManager = GetComponent<ScoreManager>();
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
    /// <para>�܂��A���C�����ł��Ă����ꍇ�A�X�R�A�����Z���܂�</para>
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

        //�폜�Ώۃ��C��������
        if(_deleteLineIndexs.Count != 0)
        {
            //�폜�������s��
            DeleteLine();

            //���ʉ�
            _myAudio.PlayOneShot(_deleteLineSE[_deleteLineIndexs.Count / TETRIS_LINE]);

            //�񂲂ƂɃG�t�F�N�g
            for(int i = 0; i < _deleteLineIndexs.Count; i++) { _deleteLineEfe[i].SetEffect(_deleteLineIndexs[i]); }
            //4��̏ꍇ�̓e�g���X�G�t�F�N�g (�����Ƃ��Ă͈�ԏ�̗��n��)
            if (_deleteLineIndexs.Count == TETRIS_LINE) { _tetrisText.SetView(); }
            //T�X�s������̏ꍇ�� T�X�s���G�t�F�N�g �� �X�R�A�{��
            if (_tSpin)
            {
                _tSpinText.SetView(); //�G�t�F�N�g
                _scoreManager.AddScore(_deleteLineIndexs.Count * TSPIN_SCORE_RATIO); //�X�R�A�{��
                return;
                
            }
            //�X�R�A���Z
            _scoreManager.AddScore(_deleteLineIndexs.Count);
        }
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
        return;
    }

    /// <summary>
    /// <para>GetPlayStatus</para>
    /// <para>����~�m�̑���󋵂��������܂�</para>
    /// </summary>
    /// <returns>���씻�� -1:�v���C�s�\ 0:���쒆 1:����I��</returns>
    public int GetPlayStatus()
    {
        //�v���C���\�ł͂Ȃ�
        if (!_canPlay)
        {
            //�v���C���s�\�ł��邱�Ƃ�Ԃ�
            return GAMEEND_ID;
        }

        //�ݒu������ �� ����\�ȃ~�m�̍ő吔 ��葽����
        if(MAX_COMMITMINO_CNT <= _commitCnt)
        {
            //�J�E���g���Z�b�g
            _commitCnt = 0;
            //�ݒu���������Ă��邱�Ƃ�Ԃ�
            return COMMIT_ID;
        }

        //�܂����삵�Ă���
        return PLAYING_ID;
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

    //�C���^�[�t�F�C�X�p��
    public void NotPlayable()
    {
        _canPlay = false;
        return;
    }
    #endregion
}
