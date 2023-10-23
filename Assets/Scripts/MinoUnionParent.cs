// -------------------------------------------------------------------------------
// MinoUnionParent.cs
//
// �쐬��: 2023/10/18
// �쐬��: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinoUnionParent : MinoModelCreate, IMinoUnionCtrl
{
    #region �ϐ�
    struct SRSPosSave //�X�p���e�̍��W�ۑ��f�[�^
    {
        public Vector3 startPos;
        public Vector3 firstSRSPos;
        public Vector3 secondSRSPos;
    }
    private SRSPosSave _srsPos = new();

    //�X�p���e����p
    const int ANGLE_UP_ID = 0; //�����ID
    const int ANGLE_RIGHT_ID = 1; //�E����ID
    const int ANGLE_DOWN_ID = 2; //������ID
    const int ANGLE_LEFT_ID = 3; //������ID
    const int DIRE_RIGHT_ID = 1; //�E��]����ID
    const int DIRE_LEFT_ID = -1; //����]����ID

    const float ROTATE_VALUE = 90f; //��]�����̉�]�p�x
    const float FALL_TIME = 0.5f; //��������
    const float SOFTDROP_SPEED = 4.5f; //�\�t�g�h���b�v�̔{�����x
    
    private int _nowAngle = 0; //���݂̃~�m�̌���
    private int _moveDire = 0; //��]����
    private bool _needReturn = false; //��]�����߂�����
    private int _srsCnt = 0; //�X�p���e�̉�
    private float _fallTimer = 0; //�����v���^�C�}�[

    private IFieldCtrl _fieldCtrl = default; //�t�B�[���h�Ǘ��V�X�e���̃C���^�[�t�F�C�X
    #endregion

    #region �v���p�e�B

    #endregion

    #region ���\�b�h
    /// <summary>
    /// ����������
    /// </summary>
    void Awake()
    {
        //������
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
        if (MyTrans.childCount == 0) { return; }

        //���Ԍo�ߗ���
        FallMino();
    }

    // �C���^�[�t�F�C�X�p��
    public void Move(int x)
    {
        //�ړ����f
        MyTrans.position += Vector3.right * x;
        //�Փ˔��肪�������ꍇ�͖߂�
        if (CheckMino()) { MyTrans.position -= Vector3.right * x; }
    }

    // �C���^�[�t�F�C�X�p��
    public void Rotate(int angle)
    {
        //��]���f
        MyTrans.eulerAngles -= Vector3.forward * ROTATE_VALUE * angle;

        _nowAngle = (int)(MyTrans.eulerAngles.z / ROTATE_VALUE); //�����擾
        _moveDire = angle; //��]�����擾
        _needReturn = false; //��]�\

        //�X�p���e�񐔏�����
        _srsCnt = 0;
        //�Փ˔��肪�������ꍇ�̓X�[�p�[���[�e�[�V�����V�X�e�������s����
        if (CheckMino())
        {
            Debug.Log("srs");
            if (MyModel == IMinoCreatable.MinoType.minoI) { SRSByFour(); } //�T�C�Y���S���S
            SRSByThree(); //�T�C�Y���R���R
        }

        //�X�p���e���ł��Ȃ��ꍇ�͊p�x��߂�
        if (_needReturn)
        {
            MyTrans.eulerAngles -= Vector3.forward * ROTATE_VALUE * -angle;
            _nowAngle = (int)(MyTrans.eulerAngles.z / ROTATE_VALUE); //�����擾
        }
    }
    
    //�C���^�[�t�F�C�X�p��
    public void HardDrop()
    {
        //�P�񗎉�
        MyTrans.position += Vector3.down;

        //������ɏՓ˔��肪����
        if (CheckMino())
        {
            //���Ƃɖ߂�
            MyTrans.position += Vector3.up;
            //�~�m���t�B�[���h�ɐݒ�
            foreach (IMinoInfo mino in Minos)
            {
                //�e�q�֌W�폜
                mino.DisConnect();
                //�R�~�b�g
                _fieldCtrl.SetMino(mino.MinoX, mino.MinoY);
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

    //�C���^�[�t�F�C�X�p��
    public void SoftDrop()
    {
        //�^�C�}�[�{��
        _fallTimer += Time.deltaTime * SOFTDROP_SPEED;
    }

    private void FallMino()
    {
        _fallTimer += Time.deltaTime; //�^�C�}�[���Z

        //�������ԂɂȂ�����
        if(FALL_TIME < _fallTimer)
        {
            _fallTimer = 0;
            //�~�m���P�}�X����
            MyTrans.position += Vector3.down;

            //������ɏՓ˔��肪����
            if (CheckMino())
            {
                //���Ƃɖ߂�
                MyTrans.position += Vector3.up;
                //�~�m���t�B�[���h�ɐݒ�
                foreach(IMinoInfo mino in Minos)
                {
                    //�e�q�֌W�폜
                    mino.DisConnect();
                    //�R�~�b�g
                    _fieldCtrl.SetMino(mino.MinoX, mino.MinoY);
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
        foreach (IMinoInfo mino in Minos)
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
                _srsPos.startPos = MyTrans.position; //�������W�ۑ�
                if (_nowAngle == ANGLE_RIGHT_ID || (_nowAngle == ANGLE_DOWN_ID && _moveDire == DIRE_LEFT_ID) || (_nowAngle == ANGLE_UP_ID && _moveDire == DIRE_RIGHT_ID))
                {
                    MyTrans.position += Vector3.left;
                    break; //�������I��
                }
                MyTrans.position += Vector3.right;
                break; //�������I��

            case 1: //��Q���� --------------------------------------------------------------------------------------------------------------------------
                //���W�p��
                if (_nowAngle == ANGLE_RIGHT_ID || _nowAngle == ANGLE_LEFT_ID)
                {
                    MyTrans.position += Vector3.up;
                    break; //�������I��
                }
                MyTrans.position += Vector3.down;
                break; //�������I��

            case 2: //��R���� --------------------------------------------------------------------------------------------------------------------------
                MyTrans.position = _srsPos.startPos; //�������W�����߂�
                if (_nowAngle == ANGLE_RIGHT_ID || _nowAngle == ANGLE_LEFT_ID)
                {
                    MyTrans.position += Vector3.down * 2;
                    break; //�������I��
                }
                MyTrans.position += Vector3.up * 2;
                break; //�������I��

            case 3: //��S���� --------------------------------------------------------------------------------------------------------------------------
                //���W�p��
                if (_nowAngle == ANGLE_RIGHT_ID || (_nowAngle == ANGLE_DOWN_ID && _moveDire == DIRE_LEFT_ID) || (_nowAngle == ANGLE_UP_ID && _moveDire == DIRE_RIGHT_ID))
                {
                    MyTrans.position += Vector3.left;
                    break; //�������I��
                }
                MyTrans.position += Vector3.right;
                break; //�������I��

            case 4: //��]�ł��Ȃ� ----------------------------------------------------------------------------------------------------------------------
                MyTrans.position = _srsPos.startPos; //�������W�����߂�
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
                _srsPos.startPos = MyTrans.position; //�������W�ۑ�
                if(_nowAngle == ANGLE_UP_ID)
                {
                    MyTrans.position += Vector3.left * 2 * _moveDire;
                    break; //�������I��
                }
                if(_nowAngle == ANGLE_DOWN_ID)
                {
                    MyTrans.position += Vector3.left * _moveDire;
                    break; //�������I��
                }
                if(_moveDire == DIRE_RIGHT_ID)
                {
                    if(_nowAngle == ANGLE_RIGHT_ID)
                    {
                        MyTrans.position += Vector3.left * 2;
                    }
                    else
                    {
                        MyTrans.position += Vector3.right * 2;
                    }
                }
                else
                {
                    if (_nowAngle == ANGLE_RIGHT_ID)
                    {
                        MyTrans.position += Vector3.right;
                    }
                    else
                    {
                        MyTrans.position += Vector3.left;
                    }
                }
                break; //�������I��

            case 1: //��Q���� --------------------------------------------------------------------------------------------------------------------------
                _srsPos.firstSRSPos = MyTrans.position; //��P�����ʒu�ۑ�
                MyTrans.position = _srsPos.startPos; //�������W�����߂�
                if (_nowAngle == ANGLE_UP_ID)
                {
                    MyTrans.position += Vector3.right * _moveDire;
                    break; //�������I��
                }
                if (_nowAngle == ANGLE_DOWN_ID)
                {
                    MyTrans.position += Vector3.right * 2 * _moveDire;
                    break; //�������I��
                }
                if (_moveDire == DIRE_LEFT_ID)
                {
                    if (_nowAngle == ANGLE_RIGHT_ID)
                    {
                        MyTrans.position += Vector3.left * 2;
                    }
                    else
                    {
                        MyTrans.position += Vector3.right * 2;
                    }
                }
                else
                {
                    if (_nowAngle == ANGLE_RIGHT_ID)
                    {
                        MyTrans.position += Vector3.right;
                    }
                    else
                    {
                        MyTrans.position += Vector3.left;
                    }
                }
                break; //�������I��

            case 2: //��R���� --------------------------------------------------------------------------------------------------------------------------
                _srsPos.secondSRSPos = MyTrans.position; //��Q�����ʒu�ۑ�
                MyTrans.position = _srsPos.firstSRSPos; //��P���W�����߂�
                if(_nowAngle == ANGLE_RIGHT_ID)
                {
                    if(_moveDire == DIRE_RIGHT_ID)
                    {
                        MyTrans.position += Vector3.down;
                    }
                    else
                    {
                        MyTrans.position += Vector3.down * 2;
                    }
                    break; //�������I��
                }
                if(_nowAngle == ANGLE_LEFT_ID)
                {
                    if(_moveDire == DIRE_RIGHT_ID)
                    {
                        MyTrans.position += Vector3.up;
                    }
                    else
                    {
                        MyTrans.position += Vector3.up * 2;
                    }
                    break; //�������I��
                }
                if(_nowAngle == ANGLE_UP_ID)
                {
                    if(_moveDire == DIRE_LEFT_ID)
                    {
                        MyTrans.position += Vector3.down;
                    }
                    else
                    {
                        MyTrans.position += Vector3.up * 2;
                    }
                    break; //�������I��
                }
                if (_moveDire == DIRE_LEFT_ID)
                {
                    MyTrans.position += Vector3.up;
                }
                else
                {
                    MyTrans.position += Vector3.down * 2;
                }
                break; //�������I��

            case 3: //��S���� --------------------------------------------------------------------------------------------------------------------------
                MyTrans.position = _srsPos.secondSRSPos; //��P�������W�����߂�
                if (_nowAngle == ANGLE_RIGHT_ID)
                {
                    if (_moveDire == DIRE_LEFT_ID)
                    {
                        MyTrans.position += Vector3.up;
                    }
                    else
                    {
                        MyTrans.position += Vector3.up * 2;
                    }
                    break; //�������I��
                }
                if (_nowAngle == ANGLE_LEFT_ID)
                {
                    if (_moveDire == DIRE_LEFT_ID)
                    {
                        MyTrans.position += Vector3.down;
                    }
                    else
                    {
                        MyTrans.position += Vector3.down * 2;
                    }
                    break; //�������I��
                }
                if (_nowAngle == ANGLE_UP_ID)
                {
                    if (_moveDire == DIRE_RIGHT_ID)
                    {
                        MyTrans.position += Vector3.up;
                    }
                    else
                    {
                        MyTrans.position += Vector3.down * 2;
                    }
                    break; //�������I��
                }
                if (_moveDire == DIRE_RIGHT_ID)
                {
                    MyTrans.position += Vector3.down;
                }
                else
                {
                    MyTrans.position += Vector3.up * 2;
                }
                break; //�������I��

            case 4: //��]�ł��Ȃ� -----------------------------------------------------------------------------------------------------------------------
                MyTrans.position = _srsPos.startPos; //�������W�����߂�
                _needReturn = true;
                return; //�������I��
        }
        _srsCnt++; //�X�p���e�L�^
        if (CheckMino()) { SRSByFour(); } //�Փ˔��肪�������ꍇ�A�X�p���e�p��
        return;
    }

    // �N���X�p��
    public override void CreateMinoUnit(IMinoInfo[] minoBlocks,IMinoCreatable.MinoType setModel)
    {
        //��ꃁ�\�b�h�g�p
        base.CreateMinoUnit(minoBlocks,setModel);

        //O�~�m��I�~�m�͌`������Ȃ̂ŃX�^�[�g�n�_��ύX����
        if(MyModel == IMinoCreatable.MinoType.minoO) //O�~�m
        {
            MyTrans.position += Vector3.right * IMinoCreatable.EXCEPTION_SHIFT_0_5 + Vector3.up * IMinoCreatable.EXCEPTION_SHIFT_0_5;
            return;
        }
        if(MyModel == IMinoCreatable.MinoType.minoI) //I�~�m
        {
            MyTrans.position += Vector3.right * IMinoCreatable.EXCEPTION_SHIFT_0_5 + Vector3.down * IMinoCreatable.EXCEPTION_SHIFT_0_5;
        }
    }
    #endregion
}
