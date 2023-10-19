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
    const int FIELD_MAX_HEIGHT = 25; //�t�B�[���h�̏c��
    const int FIELD_VIEW_HEIGHT = 20; //�t�B�[���h�̍ő�\���c��
    const int TILE_NONE_ID = 0; //�t�B�[���h�̋�ID
    const int TILE_MINO_ID = 1; //�~�mID

    [SerializeField]
    private GameObject _fieldTileObj = default; //�󔒃^�C��

    private int[,] _field = new int[FIELD_MAX_HEIGHT,FIELD_MAX_WIDTH]; //�t�B�[���h�ۑ� [�c��:y,����:x]
    private bool[] _lines = new bool[FIELD_MAX_HEIGHT]; //�t�B�[���h�̃��C���������X�g [�c��:y]

    private Transform _myTrans = default;
    #endregion

    #region ���\�b�h
    /// <summary>
    /// ����������
    /// </summary>
    void Awake()
    {
        //�ϐ�������
        _myTrans = transform;

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
                        _myTrans
                        );
                }
            }
            //���C������������
            _lines[y] = false;

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
    /// <para>CheckLine</para>
    /// <para>�t�B�[���h�̒��Ń��C�����ł��Ă��邩�������܂�</para>
    /// </summary>
    private void CheckLine()
    {
        //���C������   
    }

    /// <summary>
    /// <para>DeleteLine</para>
    /// <para>�w�肵�����C�����폜���܂�</para>
    /// </summary>
    private void DeleteLine()
    {
        //���C������
    }
    

    public bool CheckAlreadyMinoExist(int x,int y)
    {
        //�t�B�[���h�O(�㉺���E) �܂��� ���Ƀ~�m�����݂���
        if (x < 0 || y < 0 || FIELD_MAX_WIDTH <= x || FIELD_MAX_HEIGHT <= y || _field[y, x] == TILE_MINO_ID) { return true; /*�󔒂ł͂Ȃ�*/ }

        //�󔒂ł���
        return false;
    }


    public void SetMino(int x,int y)
    {
        //�~�m�ݒ�
        _field[y, x] = TILE_MINO_ID;
        return;
    }
    #endregion
}
