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
    private const int PLAYING_ID = 0;

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
        //�Q�[���I��
        if(_fieldSystem.GetPlayStatus() == GAMEEND_ID) { }

        //���쒆��
        if (_fieldSystem.GetPlayStatus() == PLAYING_ID) { return; /*���쒆*/ }
        
        //���삪�I�����Ă���
        _factorySystem.CreateMino(); //�~�m����
    }
    #endregion
}
