// -------------------------------------------------------------------------------
// MinoModelGeneration.cs
//
// �쐬��: 2023/10/23
// �쐬��: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinoModelGeneration : MonoBehaviour,IMinoCreatable
{
    #region �ϐ�
    private Vector3 _createStartPos = default; //�~�m�X�^�[�g�ʒu
    private Color _unionColor = default; //�~�m�F
    private Transform _transform = default; //���g��Transform�ݒ�

    private IMinoCreatable.MinoType _myModel = default; //�~�m�`
    private IMinoBlockAccessible[] _minos = new IMinoBlockAccessible[IMinoCreatable.MAX_MINO_CNT]; //�~�m�u���b�N�Ǘ����X�g
    #endregion

    #region �v���p�e�B
    public Transform MyTransform { get => _transform; }
    public IMinoCreatable.MinoType MyModel { get => _myModel; }
    public IMinoBlockAccessible[] Minos { get => _minos; }
    #endregion

    #region ���\�b�h
    // �C���^�[�t�F�C�X�p��
    public virtual void CreateMinoUnit(IMinoBlockAccessible[] minoBlocks, IMinoCreatable.MinoType setModel)
    {
        //�������ݒ���s���Ă��Ȃ�
        if(_transform == default)
        {
            _transform = transform; //���g��Transform�ݒ�
            _createStartPos = _transform.position; //�����ʒu�ݒ�
        }

        //�ʒu�A�p�x������
        _transform.position = _createStartPos;
        _transform.eulerAngles = Vector3.zero;

        //�~�m�u���b�N��ݒ�
        _minos = new List<IMinoBlockAccessible>(minoBlocks).ToArray();
        _myModel = setModel;

        Debug.Log(_transform.name + "|" + _minos.Length);

        //�w�肳�ꂽ���f���ɉ����āA�~�m�u���b�N�̈ʒu�ƐF��ݒ�
        switch (_myModel)
        {
            case IMinoCreatable.MinoType.minoO: //O�~�m
                _minos[0].SetMinoPos(IMinoCreatable.EXCEPTION_SHIFT_0_5, IMinoCreatable.EXCEPTION_SHIFT_0_5, _transform);
                _minos[1].SetMinoPos(-IMinoCreatable.EXCEPTION_SHIFT_0_5, IMinoCreatable.EXCEPTION_SHIFT_0_5, _transform);
                _minos[2].SetMinoPos(-IMinoCreatable.EXCEPTION_SHIFT_0_5, -IMinoCreatable.EXCEPTION_SHIFT_0_5, _transform);
                _minos[3].SetMinoPos(IMinoCreatable.EXCEPTION_SHIFT_0_5, -IMinoCreatable.EXCEPTION_SHIFT_0_5, _transform);
                _unionColor = Color.yellow; //���F
                break;
            case IMinoCreatable.MinoType.minoS: //S�~�m
                _minos[0].SetMinoPos(0, 0, _transform);
                _minos[1].SetMinoPos(-IMinoCreatable.EXCEPTION_SHIFT_1_0, 0, _transform);
                _minos[2].SetMinoPos(0, IMinoCreatable.EXCEPTION_SHIFT_1_0, _transform);
                _minos[3].SetMinoPos(IMinoCreatable.EXCEPTION_SHIFT_1_0, IMinoCreatable.EXCEPTION_SHIFT_1_0, _transform);
                _unionColor = Color.green; //�ΐF
                break;
            case IMinoCreatable.MinoType.minoZ: //Z�~�m
                _minos[0].SetMinoPos(0, 0, _transform);
                _minos[1].SetMinoPos(IMinoCreatable.EXCEPTION_SHIFT_1_0, 0, _transform);
                _minos[2].SetMinoPos(0, IMinoCreatable.EXCEPTION_SHIFT_1_0, _transform);
                _minos[3].SetMinoPos(-IMinoCreatable.EXCEPTION_SHIFT_1_0, IMinoCreatable.EXCEPTION_SHIFT_1_0, _transform);
                _unionColor = Color.red; //�ԐF
                break;
            case IMinoCreatable.MinoType.minoJ: //J�~�m
                _minos[0].SetMinoPos(0, 0, _transform);
                _minos[1].SetMinoPos(IMinoCreatable.EXCEPTION_SHIFT_1_0, 0, _transform);
                _minos[2].SetMinoPos(-IMinoCreatable.EXCEPTION_SHIFT_1_0, 0, _transform);
                _minos[3].SetMinoPos(-IMinoCreatable.EXCEPTION_SHIFT_1_0, IMinoCreatable.EXCEPTION_SHIFT_1_0, _transform);
                _unionColor = Color.blue; //�F
                break;
            case IMinoCreatable.MinoType.minoL: //L�~�m
                _minos[0].SetMinoPos(0, 0, _transform);
                _minos[1].SetMinoPos(-IMinoCreatable.EXCEPTION_SHIFT_1_0, 0, _transform);
                _minos[2].SetMinoPos(IMinoCreatable.EXCEPTION_SHIFT_1_0, 0, _transform);
                _minos[3].SetMinoPos(IMinoCreatable.EXCEPTION_SHIFT_1_0, IMinoCreatable.EXCEPTION_SHIFT_1_0, _transform);
                _unionColor = Color.red + Color.green / 2; //��F
                break;
            case IMinoCreatable.MinoType.minoT: //T�~�m
                _minos[0].SetMinoPos(0, 0, _transform);
                _minos[1].SetMinoPos(-IMinoCreatable.EXCEPTION_SHIFT_1_0, 0, _transform);
                _minos[2].SetMinoPos(IMinoCreatable.EXCEPTION_SHIFT_1_0, 0, _transform);
                _minos[3].SetMinoPos(0, IMinoCreatable.EXCEPTION_SHIFT_1_0, _transform);
                _unionColor = Color.red + Color.blue; //���F
                break;
            case IMinoCreatable.MinoType.minoI: //I�~�m
                _minos[0].SetMinoPos(IMinoCreatable.EXCEPTION_SHIFT_0_5, IMinoCreatable.EXCEPTION_SHIFT_0_5, _transform);
                _minos[1].SetMinoPos(IMinoCreatable.EXCEPTION_SHIFT_1_5, IMinoCreatable.EXCEPTION_SHIFT_0_5, _transform);
                _minos[2].SetMinoPos(-IMinoCreatable.EXCEPTION_SHIFT_0_5, IMinoCreatable.EXCEPTION_SHIFT_0_5, _transform);
                _minos[3].SetMinoPos(-IMinoCreatable.EXCEPTION_SHIFT_1_5, IMinoCreatable.EXCEPTION_SHIFT_0_5, _transform);
                _unionColor = Color.blue + Color.white / 2; //���F
                break;
            default:
                //��O����
                break;
        }

        //�F�ύX
        foreach (IMinoBlockAccessible mino in _minos)
        {
            mino.ChangeColor(_unionColor);
        }
    }
    #endregion
}
