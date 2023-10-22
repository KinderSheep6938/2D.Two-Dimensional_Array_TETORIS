// -------------------------------------------------------------------------------
// MinoUnionParent.cs
//
// �쐬��: 2023/10/18
// �쐬��: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinoUnionParent : MonoBehaviour, IMinoCreatable, IMinoUnionCtrl
{
    #region �ϐ�
    struct SRSPosSave //�X�p���e�̍��W�ۑ��f�[�^
    {
        public Vector3 startPos;
        public Vector3 firstSRSPos;
        public Vector3 secondSRSPos;
    }
    private SRSPosSave _srsPos = new();

    const float EXCEPTION_MINO_0_5_SHIFT = 0.5f; //�~�m�`�����p0.5����
    const float EXCEPTION_MINO_1_0_SHIFT = 1.0f; //�~�m�`�����p1.0����
    const float EXCEPTION_MINO_1_5_SHIFT = 1.5f; //�~�m�`�����p1.5����
    const int ANGLE_UP_ID = 0; //�����ID
    const int ANGLE_RIGHT_ID = 1; //�E����ID
    const int ANGLE_DOWN_ID = 2; //������ID
    const int ANGLE_LEFT_ID = 3; //������ID
    const int DIRE_RIGHT_ID = 1; //������ID
    const int DIRE_LEFT_ID = -1; //������ID
    const float ROTATE_VALUE = 90f; //��]�����̉�]�p�x
    const float FALL_TIME = 0.5f; //��������
    
    private Vector3 _createStartPos = default; //�~�m�X�^�[�g�ʒu
    private int _nowAngle = 0; //���݂̃~�m�̌���
    private int _moveDire = 0; //��]����
    private bool _needReturn = false; //��]�����߂�����
    private int _srsCnt = 0; //�X�p���e�̉�
    private List<IMinoInfo> _minos = new(); //�~�m�u���b�N�Ǘ����X�g
    private float _fallTimer = 0; //�����v���^�C�}�[
    private Color _unionColor = default; //�~�m�F
    private IMinoCreatable.MinoType _myModel = default; //�~�m�`

    private IFieldCtrl _fieldCtrl = default; //�t�B�[���h�Ǘ��V�X�e���̃C���^�[�t�F�C�X
    private Transform _myTrans = default; //���g��Transform
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
        _myTrans = transform;
        _createStartPos = _myTrans.position;
        _fieldCtrl = FindObjectOfType<FieldManager>().GetComponent<IFieldCtrl>();
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
        if (_myTrans.childCount == 0) { return; }

        //���Ԍo�ߗ���
        FallMino();
    }

    // �C���^�[�t�F�C�X�p��
    public void Move(int x)
    {
        //�ړ����f
        _myTrans.position += Vector3.right * x;
        //�Փ˔��肪�������ꍇ�͖߂�
        if (CheckMino()) { _myTrans.position -= Vector3.right * x; }
    }

    // �C���^�[�t�F�C�X�p��
    public void Rotate(int angle)
    {
        //��]���f
        _myTrans.eulerAngles -= Vector3.forward * ROTATE_VALUE * angle;

        _nowAngle = (int)(_myTrans.eulerAngles.z / ROTATE_VALUE); //�����擾
        _moveDire = angle; //��]�����擾
        _needReturn = false; //��]�\

        //�X�p���e�񐔏�����
        _srsCnt = 0;
        //�Փ˔��肪�������ꍇ�̓X�[�p�[���[�e�[�V�����V�X�e�������s����
        if (CheckMino())
        {
            Debug.Log("srs");
            if (_myModel == IMinoCreatable.MinoType.minoI) { SRSByFour(); } //�T�C�Y���S���S
            SRSByThree(); //�T�C�Y���R���R
        }

        //�X�p���e���ł��Ȃ��ꍇ�͊p�x��߂�
        if (_needReturn)
        {
            _myTrans.eulerAngles -= Vector3.forward * ROTATE_VALUE * -angle;
            _nowAngle = (int)(_myTrans.eulerAngles.z / ROTATE_VALUE); //�����擾
        }
    }
    
    //�C���^�[�t�F�C�X�p��
    public void HardDrop()
    {
        //�P�񗎉�
        _myTrans.position += Vector3.down;

        //������ɏՓ˔��肪����
        if (CheckMino())
        {
            //���Ƃɖ߂�
            _myTrans.position += Vector3.up;
            //�~�m���t�B�[���h�ɐݒ�
            foreach (IMinoInfo mino in _minos)
            {
                //�R�~�b�g
                _fieldCtrl.SetMino(mino.MinoX, mino.MinoY);
                //�e�q�֌W�폜
                mino.DisConnect();
            }
            //�����^�C�}�[������
            _fallTimer = 0;

            return; //�����I��
        }
        else //�Փ˔��肪�Ȃ�
        {
            //�ċN�Ăяo��
            HardDrop();
        }
        return;
    }

    private void FallMino()
    {
        _fallTimer += Time.deltaTime; //�^�C�}�[���Z

        //�������ԂɂȂ�����
        if(FALL_TIME < _fallTimer)
        {
            _fallTimer = 0;
            //�~�m���P�}�X����
            _myTrans.position += Vector3.down;

            //������ɏՓ˔��肪����
            if (CheckMino())
            {
                //���Ƃɖ߂�
                _myTrans.position += Vector3.up;
                //�~�m���t�B�[���h�ɐݒ�
                foreach(IMinoInfo mino in _minos)
                {
                    //�R�~�b�g
                    _fieldCtrl.SetMino(mino.MinoX, mino.MinoY);
                    //�e�q�֌W�폜
                    mino.DisConnect();

                }

            }
        }
    }

    /// <summary>
    /// <para>CheckMino</para>
    /// <para>�~�m�u���b�N�̏Փ˂��`�F�b�N���܂�</para>
    /// </summary>
    /// <returns>�Փ˔���</returns>
    private bool CheckMino()
    {
        //�~�m�u���b�N���Փ˂��ĂȂ���
        foreach (IMinoInfo mino in _minos)
        {
            //�󔒂ł͂Ȃ�
            if (_fieldCtrl.CheckAlreadyMinoExist(mino.MinoX, mino.MinoY)) { return true; }
        }
        //�󔒂ł���
        return false;
    }

    /// <summary>
    /// <para>SRSByThree</para>
    /// <para>�T�C�Y���R�̃X�[�p�[���[�e�[�V�����V�X�e��</para>
    /// </summary>
    private void SRSByThree()
    {
        switch (_srsCnt)
        {
            case 0: //��P���� --------------------------------------------------------------------------------------------------------------------------
                _srsPos.startPos = _myTrans.position; //�������W�ۑ�
                if (_nowAngle == ANGLE_RIGHT_ID || (_nowAngle == ANGLE_DOWN_ID && _moveDire == DIRE_LEFT_ID) || (_nowAngle == ANGLE_UP_ID && _moveDire == DIRE_RIGHT_ID))
                {
                    _myTrans.position += Vector3.left;
                    break; //�������I��
                }
                _myTrans.position += Vector3.right;
                break; //�������I��

            case 1: //��Q���� --------------------------------------------------------------------------------------------------------------------------
                //���W�p��
                if (_nowAngle == ANGLE_RIGHT_ID || _nowAngle == ANGLE_LEFT_ID)
                {
                    _myTrans.position += Vector3.up;
                    break; //�������I��
                }
                _myTrans.position += Vector3.down;
                break; //�������I��

            case 2: //��R���� --------------------------------------------------------------------------------------------------------------------------
                _myTrans.position = _srsPos.startPos; //�������W�����߂�
                if (_nowAngle == ANGLE_RIGHT_ID || _nowAngle == ANGLE_LEFT_ID)
                {
                    _myTrans.position += Vector3.down * 2;
                    break; //�������I��
                }
                _myTrans.position += Vector3.up * 2;
                break; //�������I��

            case 3: //��S���� --------------------------------------------------------------------------------------------------------------------------
                //���W�p��
                if (_nowAngle == ANGLE_RIGHT_ID || (_nowAngle == ANGLE_DOWN_ID && _moveDire == DIRE_LEFT_ID) || (_nowAngle == ANGLE_UP_ID && _moveDire == DIRE_RIGHT_ID))
                {
                    _myTrans.position += Vector3.left;
                    break; //�������I��
                }
                _myTrans.position += Vector3.right;
                break; //�������I��

            case 4: //��]�ł��Ȃ� ----------------------------------------------------------------------------------------------------------------------
                _myTrans.position = _srsPos.startPos; //�������W�����߂�
                _needReturn = true;
                return; //�������I��
        }
        _srsCnt++; //�X�p���e�L�^
        if (CheckMino()) { SRSByThree(); } //�Փ˔��肪�������ꍇ�A�X�p���e�p��
        return;
    }

    /// <summary>
    /// <para>SRSByFour</para>
    /// <para>�T�C�Y���S�̃X�[�p�[���[�e�[�V�����V�X�e��</para>
    /// </summary>
    private void SRSByFour()
    {
        switch (_srsCnt)
        {
            case 0: //��P���� --------------------------------------------------------------------------------------------------------------------------
                _srsPos.startPos = _myTrans.position; //�������W�ۑ�
                if(_nowAngle == ANGLE_UP_ID)
                {
                    _myTrans.position += Vector3.left * 2 * _moveDire;
                    break; //�������I��
                }
                if(_nowAngle == ANGLE_DOWN_ID)
                {
                    _myTrans.position += Vector3.left * _moveDire;
                    break; //�������I��
                }
                if(_moveDire == DIRE_RIGHT_ID)
                {
                    if(_nowAngle == ANGLE_RIGHT_ID)
                    {
                        _myTrans.position += Vector3.left * 2;
                    }
                    else
                    {
                        _myTrans.position += Vector3.right * 2;
                    }
                }
                else
                {
                    if (_nowAngle == ANGLE_RIGHT_ID)
                    {
                        _myTrans.position += Vector3.right;
                    }
                    else
                    {
                        _myTrans.position += Vector3.left;
                    }
                }
                break; //�������I��

            case 1: //��Q���� --------------------------------------------------------------------------------------------------------------------------
                _srsPos.firstSRSPos = _myTrans.position; //��P�����ʒu�ۑ�
                _myTrans.position = _srsPos.startPos; //�������W�����߂�
                if (_nowAngle == ANGLE_UP_ID)
                {
                    _myTrans.position += Vector3.right * _moveDire;
                    break; //�������I��
                }
                if (_nowAngle == ANGLE_DOWN_ID)
                {
                    _myTrans.position += Vector3.right * 2 * _moveDire;
                    break; //�������I��
                }
                if (_moveDire == DIRE_LEFT_ID)
                {
                    if (_nowAngle == ANGLE_RIGHT_ID)
                    {
                        _myTrans.position += Vector3.left * 2;
                    }
                    else
                    {
                        _myTrans.position += Vector3.right * 2;
                    }
                }
                else
                {
                    if (_nowAngle == ANGLE_RIGHT_ID)
                    {
                        _myTrans.position += Vector3.right;
                    }
                    else
                    {
                        _myTrans.position += Vector3.left;
                    }
                }
                break; //�������I��

            case 2: //��R���� --------------------------------------------------------------------------------------------------------------------------
                _srsPos.secondSRSPos = _myTrans.position; //��Q�����ʒu�ۑ�
                _myTrans.position = _srsPos.firstSRSPos; //��P���W�����߂�
                if(_nowAngle == ANGLE_RIGHT_ID)
                {
                    if(_moveDire == DIRE_RIGHT_ID)
                    {
                        _myTrans.position += Vector3.down;
                    }
                    else
                    {
                        _myTrans.position += Vector3.down * 2;
                    }
                    break; //�������I��
                }
                if(_nowAngle == ANGLE_LEFT_ID)
                {
                    if(_moveDire == DIRE_RIGHT_ID)
                    {
                        _myTrans.position += Vector3.up;
                    }
                    else
                    {
                        _myTrans.position += Vector3.up * 2;
                    }
                    break; //�������I��
                }
                if(_nowAngle == ANGLE_UP_ID)
                {
                    if(_moveDire == DIRE_LEFT_ID)
                    {
                        _myTrans.position += Vector3.down;
                    }
                    else
                    {
                        _myTrans.position += Vector3.up * 2;
                    }
                    break; //�������I��
                }
                if (_moveDire == DIRE_LEFT_ID)
                {
                    _myTrans.position += Vector3.up;
                }
                else
                {
                    _myTrans.position += Vector3.down * 2;
                }
                break; //�������I��

            case 3: //��S���� --------------------------------------------------------------------------------------------------------------------------
                _myTrans.position = _srsPos.secondSRSPos; //��P�������W�����߂�
                if (_nowAngle == ANGLE_RIGHT_ID)
                {
                    if (_moveDire == DIRE_LEFT_ID)
                    {
                        _myTrans.position += Vector3.up;
                    }
                    else
                    {
                        _myTrans.position += Vector3.up * 2;
                    }
                    break; //�������I��
                }
                if (_nowAngle == ANGLE_LEFT_ID)
                {
                    if (_moveDire == DIRE_LEFT_ID)
                    {
                        _myTrans.position += Vector3.down;
                    }
                    else
                    {
                        _myTrans.position += Vector3.down * 2;
                    }
                    break; //�������I��
                }
                if (_nowAngle == ANGLE_UP_ID)
                {
                    if (_moveDire == DIRE_RIGHT_ID)
                    {
                        _myTrans.position += Vector3.up;
                    }
                    else
                    {
                        _myTrans.position += Vector3.down * 2;
                    }
                    break; //�������I��
                }
                if (_moveDire == DIRE_RIGHT_ID)
                {
                    _myTrans.position += Vector3.down;
                }
                else
                {
                    _myTrans.position += Vector3.up * 2;
                }
                break; //�������I��

            case 4: //��]�ł��Ȃ� -----------------------------------------------------------------------------------------------------------------------
                _myTrans.position = _srsPos.startPos; //�������W�����߂�
                _needReturn = true;
                return; //�������I��
        }
        _srsCnt++; //�X�p���e�L�^
        if (CheckMino()) { SRSByFour(); } //�Փ˔��肪�������ꍇ�A�X�p���e�p��
        return;
    }

    // �C���^�[�t�F�C�X�p��
    public void CreateMinoUnit(IMinoInfo[] minoBlocks,IMinoCreatable.MinoType setModel)
    {
        //�ʒu�A�p�x������
        _myTrans.position = _createStartPos;
        _myTrans.eulerAngles = Vector3.zero;

        //�~�m�u���b�N��ݒ�
        _minos.Clear();
        _minos.AddRange(minoBlocks);

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
                _minos[0].SetMinoPos(0,0, _myTrans);
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
        foreach(IMinoInfo mino in _minos)
        {
            mino.ChangeColor(_unionColor);
        }
    }
    #endregion
}
