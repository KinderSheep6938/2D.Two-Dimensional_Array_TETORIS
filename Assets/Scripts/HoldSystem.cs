// -------------------------------------------------------------------------------
// HoldSystem.cs
//
// �쐬��: 2023/10/22
// �쐬��: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldSystem : MonoBehaviour, IMinoHoldable, IMinoCreatable
{
    #region �ϐ�
    const float EXCEPTION_MINO_0_5_SHIFT = 0.5f; //�~�m�`�����p0.5����
    const float EXCEPTION_MINO_1_0_SHIFT = 1.0f; //�~�m�`�����p1.0����
    const float EXCEPTION_MINO_1_5_SHIFT = 1.5f; //�~�m�`�����p1.5����
    const string PLAYABLE_MINOCTRL_TAG = "PlayableMino";

    private IMinoCreatable.MinoType _myModel = default; //�~�m�`

    private List<IMinoInfo> _minos = new();
    private Vector3 _createStartPos = default;
    private Color _unionColor = default;

    private Transform _myTrans = default;
    private IMinoCreatable _playerMino = default;
    #endregion

    #region �v���p�e�B
    public IMinoCreatable.MinoType MyModel { get => _myModel; set => _myModel = value; }
    #endregion

    #region ���\�b�h
    /// <summary>
    /// ����������
    /// </summary>
    void Awake()
    {
        //������
        _minos.Clear();
        _minos.AddRange(GetComponentsInChildren<IMinoInfo>());

        _playerMino = GameObject.FindGameObjectWithTag(PLAYABLE_MINOCTRL_TAG).GetComponent<IMinoCreatable>();
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

    // �C���^�[�t�F�C�X�p��
    public void CreateMinoUnit(IMinoInfo[] minoBlocks, IMinoCreatable.MinoType setModel)
    {
        //�ʒu�A�p�x������
        _myTrans.position = _createStartPos;

        //�w�肳�ꂽ���f���ɉ����āA�~�m�u���b�N�̈ʒu�ƐF��ݒ�
        switch (setModel)
        {
            case IMinoCreatable.MinoType.minoO: //O�~�m
                _minos[0].SetMinoPos(EXCEPTION_MINO_0_5_SHIFT, EXCEPTION_MINO_0_5_SHIFT, _myTrans);
                _minos[1].SetMinoPos(-EXCEPTION_MINO_0_5_SHIFT, EXCEPTION_MINO_0_5_SHIFT, _myTrans);
                _minos[2].SetMinoPos(-EXCEPTION_MINO_0_5_SHIFT, -EXCEPTION_MINO_0_5_SHIFT, _myTrans);
                _minos[3].SetMinoPos(EXCEPTION_MINO_0_5_SHIFT, -EXCEPTION_MINO_0_5_SHIFT, _myTrans);
                _unionColor = Color.yellow; //���F
                _myTrans.position += Vector3.right * EXCEPTION_MINO_0_5_SHIFT + Vector3.up * EXCEPTION_MINO_0_5_SHIFT; //O�~�m�̏����ݒ�
                break;
            case IMinoCreatable.MinoType.minoS: //S�~�m
                _minos[0].SetMinoPos(0, 0, _myTrans);
                _minos[1].SetMinoPos(-EXCEPTION_MINO_1_0_SHIFT, 0, _myTrans);
                _minos[2].SetMinoPos(0, EXCEPTION_MINO_1_0_SHIFT, _myTrans);
                _minos[3].SetMinoPos(EXCEPTION_MINO_1_0_SHIFT, EXCEPTION_MINO_1_0_SHIFT, _myTrans);
                _unionColor = Color.green; //�ΐF
                break;
            case IMinoCreatable.MinoType.minoZ: //Z�~�m
                _minos[0].SetMinoPos(0, 0, _myTrans);
                _minos[1].SetMinoPos(EXCEPTION_MINO_1_0_SHIFT, 0, _myTrans);
                _minos[2].SetMinoPos(0, EXCEPTION_MINO_1_0_SHIFT, _myTrans);
                _minos[3].SetMinoPos(-EXCEPTION_MINO_1_0_SHIFT, EXCEPTION_MINO_1_0_SHIFT, _myTrans);
                _unionColor = Color.red; //�ԐF
                break;
            case IMinoCreatable.MinoType.minoJ: //J�~�m
                _minos[0].SetMinoPos(0, 0, _myTrans);
                _minos[1].SetMinoPos(EXCEPTION_MINO_1_0_SHIFT, 0, _myTrans);
                _minos[2].SetMinoPos(-EXCEPTION_MINO_1_0_SHIFT, 0, _myTrans);
                _minos[3].SetMinoPos(-EXCEPTION_MINO_1_0_SHIFT, EXCEPTION_MINO_1_0_SHIFT, _myTrans);
                _unionColor = Color.blue; //�F
                break;
            case IMinoCreatable.MinoType.minoL: //L�~�m
                _minos[0].SetMinoPos(0, 0, _myTrans);
                _minos[1].SetMinoPos(-EXCEPTION_MINO_1_0_SHIFT, 0, _myTrans);
                _minos[2].SetMinoPos(EXCEPTION_MINO_1_0_SHIFT, 0, _myTrans);
                _minos[3].SetMinoPos(EXCEPTION_MINO_1_0_SHIFT, EXCEPTION_MINO_1_0_SHIFT, _myTrans);
                _unionColor = Color.red + Color.green / 2; //��F
                break;
            case IMinoCreatable.MinoType.minoT: //T�~�m
                _minos[0].SetMinoPos(0, 0, _myTrans);
                _minos[1].SetMinoPos(-EXCEPTION_MINO_1_0_SHIFT, 0, _myTrans);
                _minos[2].SetMinoPos(EXCEPTION_MINO_1_0_SHIFT, 0, _myTrans);
                _minos[3].SetMinoPos(0, EXCEPTION_MINO_1_0_SHIFT, _myTrans);
                _unionColor = Color.red + Color.blue; //���F
                break;
            case IMinoCreatable.MinoType.minoI: //I�~�m
                _minos[0].SetMinoPos(EXCEPTION_MINO_0_5_SHIFT, EXCEPTION_MINO_0_5_SHIFT, _myTrans);
                _minos[1].SetMinoPos(EXCEPTION_MINO_1_5_SHIFT, EXCEPTION_MINO_0_5_SHIFT, _myTrans);
                _minos[2].SetMinoPos(-EXCEPTION_MINO_0_5_SHIFT, EXCEPTION_MINO_0_5_SHIFT, _myTrans);
                _minos[3].SetMinoPos(-EXCEPTION_MINO_1_5_SHIFT, EXCEPTION_MINO_0_5_SHIFT, _myTrans);
                _unionColor = Color.blue + Color.white / 2; //���F
                _myTrans.position += Vector3.right * EXCEPTION_MINO_0_5_SHIFT + Vector3.down * EXCEPTION_MINO_0_5_SHIFT; //I�~�m�̏����ݒ�
                break;
            default:
                //��O����
                break;
        }

        //�F�ύX
        foreach (IMinoInfo mino in _minos)
        {
            mino.ChangeColor(_unionColor);
        }
    }

    //�C���^�[�t�F�C�X�p��
    public void Hold()
    {

    }
    #endregion
}
