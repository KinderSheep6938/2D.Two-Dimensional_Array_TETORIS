// -------------------------------------------------------------------------------
// MinoFactory.cs
//
// �쐬��: 2023/10/18
// �쐬��: Satou
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinoFactory : MonoBehaviour
{
    #region �ϐ�
    const int MAX_MODLE_CNT = 7;

    IMinoCreatable _minoCreator = default;

    private bool[] _isCreateModels = new bool[MAX_MODLE_CNT];
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
