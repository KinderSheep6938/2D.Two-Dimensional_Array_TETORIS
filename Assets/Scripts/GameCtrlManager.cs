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
    [SerializeField,Tooltip("MinoFactory")] //�~�m�����@�\
    private MinoFactory _factorySystem = default;
    [SerializeField,Tooltip("FieldManager")] //�t�B�[���h�Ǘ��@�\
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
    #endregion
}
