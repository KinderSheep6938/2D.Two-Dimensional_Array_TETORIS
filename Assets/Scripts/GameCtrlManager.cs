// -------------------------------------------------------------------------------
// GameCtrlManager.cs
//
// �쐬��: 2023/10/21
// �쐬��: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCtrlManager : MonoBehaviour
{
    #region �ϐ�
    private const int GAMEEND_ID = -1;
    private const int COMMIT_ID = 1;
    private int _nowGameStatus = 0;

    private GameObject _gameoverViewObj = default;

    private MinoFactory _factorySystem = default;
    private FieldManager _fieldSystem = default;

    #endregion

    #region �v���p�e�B

    #endregion

    #region ���\�b�h
    /// <summary>
    /// ����������
    /// </summary>
    void Awake()
    {
        _factorySystem = GetComponent<MinoFactory>();
        _fieldSystem = GetComponent<FieldManager>();
    }

    /// <summary>
    /// �X�V�O����
    /// </summary>
    void Start()
    {
        //�l�N�X�g�~�m�Ƒ���~�m�̓�Ƀ~�m�𑗂�Ȃ���΂����Ȃ����߁A�~�m�������Q��s��
        _factorySystem.CreateMino(); //�~�m����
        _factorySystem.CreateMino(); //�~�m����
    }

    /// <summary>
    /// �X�V����
    /// </summary>
    void Update()
    {
        _nowGameStatus = _fieldSystem.GetPlayStatus();

        //�Q�[���󋵂��R�~�b�g��Ԃł͂Ȃ�
        if(_nowGameStatus != COMMIT_ID)
        {
            //�Q�[����Ԃ��I����Ԃł���
            if(_nowGameStatus == GAMEEND_ID)
            {
                _gameoverViewObj.SetActive(true);
            }
            return; //����I��
        }
        
        //�R�~�b�g��Ԃł���ꍇ�͐V�����~�m�𐶐�����
        _factorySystem.CreateMino();
    }
    #endregion
}
