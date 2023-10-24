// -------------------------------------------------------------------------------
// HoldMino.cs
//
// �쐬��: 2023/10/22
// �쐬��: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldMino : MinoModelGeneration, IMinoHoldable
{
    #region �ϐ�
    private const string PLAYABLE_MINOCTRL_TAG = "PlayableMino";

    private IMinoCreatable.MinoType _holdModel = default; //�z�[���h�؂�ւ��ޔ�p�ϐ�

    private IMinoBlockAccessible[] _holdMinos = new IMinoBlockAccessible[IMinoCreatable.MAX_MINO_CNT];
    private bool _hasHold = false;

    private IMinoCreatable _playerMino = default;
    private MinoFactory _minoFactory = default;
    #endregion

    #region �v���p�e�B

    #endregion

    #region ���\�b�h
    /// <summary>
    /// ����������
    /// </summary>
    void Awake()
    {
        _playerMino = GameObject.FindGameObjectWithTag(PLAYABLE_MINOCTRL_TAG).GetComponent<IMinoCreatable>();
        _minoFactory = FindObjectOfType<MinoFactory>();
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

    // �N���X�p��
    public override void CreateMinoUnit(IMinoBlockAccessible[] minoBlocks, IMinoCreatable.MinoType setModel)
    {
        //��ꃁ�\�b�h���g�p
        base.CreateMinoUnit(minoBlocks, setModel);
        return;
    }

    //�C���^�[�t�F�C�X�p��
    public void Hold()
    {
        Debug.Log("Hold");
        //�ێ����Ă��郂�f����ޔ�
        _holdModel = MyModel;
        _holdMinos = Minos;
        //�z�[���h�Ώۂ̃~�m�`�ɕύX����
        //�����Ƃ���null��n���Ă���̂͌��ݕێ����Ă���~�m�u���b�N���g���܂킷����
        Debug.Log(_playerMino.MyModel);
        CreateMinoUnit(_playerMino.Minos, _playerMino.MyModel);
        
        //�z�[���h���Ă���~�m���Ȃ�
        if (!_hasHold)
        {
            //�V�����~�m�𐶐�����
            _minoFactory.CreateMino();
            //�z�[���h�t���O�����Ă�
            _hasHold = true;
            return;
        }

        //�z�[���h���Ă���~�m�𑀍�~�m�ɔ��f������
        _playerMino.CreateMinoUnit(_holdMinos, _holdModel);
        return;
    }
    #endregion
}
