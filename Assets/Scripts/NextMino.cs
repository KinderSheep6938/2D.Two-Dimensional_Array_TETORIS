// -------------------------------------------------------------------------------
// NextMinoView.cs
//
// �쐬��: 2023/10/24
// �쐬��: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextMino : MinoModelGeneration
{
    #region �ϐ�
    private const string PLAYABLEMINO_TAG = "PlayableMino";

    private IMinoCreatable _playableMino = default;
    private bool isSet = false;
    #endregion

    #region �v���p�e�B

    #endregion

    #region ���\�b�h
    /// <summary>
    /// ����������
    /// </summary>
    void Awake()
    {
        _playableMino = GameObject.FindGameObjectWithTag(PLAYABLEMINO_TAG).GetComponent<IMinoCreatable>();
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

    //�N���X�p��
    public override void CreateMinoUnit(IMinoBlockAccessible[] minoBlocks, IMinoCreatable.MinoType setModel)
    {
        if(isSet)
        {
            _playableMino.CreateMinoUnit(Minos, MyModel);
        }

        base.CreateMinoUnit(minoBlocks, setModel);
        isSet = true;
    }
    #endregion
}
