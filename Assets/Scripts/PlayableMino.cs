// -------------------------------------------------------------------------------
// PlayableMino.cs
//
// �쐬��: 2023/10/18
// �쐬��: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableMino : AccessibleToField, IMinoUnionCtrl
{
    #region �ϐ�
    //�X�p���e����p
    private const int ANGLE_UP_ID = 0; //�����ID
    private const int ANGLE_RIGHT_ID = 1; //�E����ID
    private const int ANGLE_DOWN_ID = 2; //������ID
    private const int ANGLE_LEFT_ID = 3; //������ID
    private const int DIRE_RIGHT_ID = 1; //�E��]����ID
    private const int DIRE_LEFT_ID = -1; //����]����ID
    struct SRSPosSave //�X�p���e�̍��W�ۑ��f�[�^
    {
        public Vector3 startPos;
        public Vector3 firstSRSPos;
        public Vector3 secondSRSPos;
    }
    private SRSPosSave _srsPos = new();

    //�v���C���[���쐧��p
    private const float ROTATE_VALUE = 90f; //��]�����̉�]�p�x
    private const float SOFTDROP_SPEED = 4.5f; //�\�t�g�h���b�v�̔{�����x

    //��]�n�� �X�p���e�Ȃ�
    private int _nowAngle = 0; //���݂̃~�m�̌���
    private int _moveDire = 0; //��]����
    private bool _needReturn = false; //��]�����߂�����
    private int _srsCnt = 0; //�X�p���e�̉�

    //���R����
    private float _minoFallTime = 0.8f; //��������
    private float _fallTimer = 0; //�����v���^�C�}�[

    //���b�N�_�E���@�\
    private const float LOCKDOWN_WAIT_TIMER = 0.5f; //�ݒu�ҋ@����
    private float _waitTimer = 0; //�ҋ@�^�C�}�[
    private int _lockDownCancel = 0; //�ݒu���
    private const int MAX_CANCEL_CNT = 15; //�ő���

    //�z�[���h����
    private bool _isHold = false;

    [SerializeField, Tooltip("��]SE")]
    private AudioClip _rotateSE = default;
    [SerializeField, Tooltip("�n�[�h�h���b�vSE")]
    private AudioClip _hardDropSE = default;
    private AudioSource _myAudio = default; //���g��AudioSource
    private IGhostStartable _ghost = default; //�S�[�X�g�V�X�e��
    #endregion

    #region �v���p�e�B
    public float FallTime { set => _minoFallTime = value; }
    #endregion

    #region ���\�b�h
    /// <summary>
    /// ����������
    /// </summary>
    void Awake()
    {
        _ghost = FindObjectOfType<GhostMino>(); //�S�[�X�g�擾
        _myAudio = GetComponent<AudioSource>();
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
        //�~�m�u���b�N���n����Ă���i�q�t������Ă���j
        if (MyTransform.childCount == 0) { return; }

        if (CheckMinoCollision(0, -1))
        {
            //�ݒu����
            LockDown();
        }
        else
        {
            //���Ԍo�ߗ���
            FallMino();
        }
    }

    // �C���^�[�t�F�C�X�p��
    public void Move(int x)
    {
        //�ړ���ɏՓ˔��肪���邩
        if (!CheckMinoCollision(x, 0))
        { 
            //�ړ����f
            MyTransform.position += Vector3.right * x;
            //�X�e�[�^�X���f
            MoveToChangeStatus();
            //��]��������
            _moveDire = 0;
        }
    }

    // �C���^�[�t�F�C�X�p��
    public void Rotate(int angle)
    {
        _myAudio.PlayOneShot(_rotateSE); //���ʉ��Đ�
        //��]���f
        MyTransform.eulerAngles -= Vector3.forward * ROTATE_VALUE * angle;

        _nowAngle = (int)(MyTransform.eulerAngles.z / ROTATE_VALUE); //�����擾
        _moveDire = angle; //��]�����擾
        _needReturn = false; //��]�\

        //�X�p���e�񐔏�����
        _srsCnt = 0;
        //�Փ˔��肪�������ꍇ�̓X�[�p�[���[�e�[�V�����V�X�e�������s����
        if (CheckMinoCollision())
        {
            Debug.Log("srs");
            if (MyModel == IMinoCreatable.MinoType.minoI) { SRSByFour(); } //�T�C�Y���S���S
            SRSByThree(); //�T�C�Y���R���R
        }

        //�X�p���e���ł��Ȃ��ꍇ�͊p�x��߂�
        if (_needReturn)
        {
            MyTransform.eulerAngles -= Vector3.forward * ROTATE_VALUE * -angle; //�p�x�߂�
            _nowAngle = (int)(MyTransform.eulerAngles.z / ROTATE_VALUE); //�����擾
            _moveDire = 0; //��]��������
            return;
        }

        //�X�e�[�^�X���f
        MoveToChangeStatus();
    }
    
    //�C���^�[�t�F�C�X�p��
    public void HardDrop()
    {
        //������ɏՓ˔��肪�Ȃ�
        if (!CheckMinoCollision(0, -1))
        {
            //���ʉ��Đ�
            _myAudio.PlayOneShot(_hardDropSE); 
            //�P�񗎉�
            MyTransform.position += Vector3.down;
            //�ċN�Ăяo��
            HardDrop();
        }
        else //����
        {
            //�R�~�b�g
            Commit();

            return; //�����I��
        }

        return;
    }

    //�C���^�[�t�F�C�X�p��
    public void SoftDrop()
    {
        //�^�C�}�[�{��
        _fallTimer += Time.deltaTime * SOFTDROP_SPEED;
    }

    //�C���^�[�t�F�C�X�p��
    public bool CheckHasHold()
    {
        Debug.Log(_isHold);
        //�܂��z�[���h���Ă��Ȃ�
        if (!_isHold)
        {
            _isHold = true; //�z�[���h���邱�Ƃ��L�^
            return false; //�܂��z�[���h���Ă��Ȃ����Ƃ𑗐M
        }
        else
        {
            return true; //�z�[���h�ς݂ł��邱�Ƃ𑗐M
        }
    }

    /// <summary>
    /// <para>FallMino</para>
    /// <para>�~�m����莞�Ԗ��ɗ��������܂�</para>
    /// </summary>
    private void FallMino()
    {
        _fallTimer += Time.deltaTime; //�^�C�}�[���Z

        //�������ԂɂȂ�����
        if(_minoFallTime < _fallTimer)
        {
            //�~�m���P�}�X����
            MyTransform.position += Vector3.down;

            //�^�C�}�[������
            _fallTimer = 0;
        }
    }

    /// <summary>
    /// <para>LockDown</para>
    /// <para>�~�m��ݒu���邩�̔�����s���܂�</para>
    /// </summary>
    private void LockDown()
    {
        _waitTimer += Time.deltaTime; //�^�C�}�[���Z

        //�ݒu���ԂɂȂ��� �܂��� ��𐔂��ő�l�𒴂��Ă���ꍇ
        if(LOCKDOWN_WAIT_TIMER <= _waitTimer || MAX_CANCEL_CNT < _lockDownCancel )
        {
            //�R�~�b�g
            Commit();
        }
    }

    /// <summary>
    /// <para>MoveToResetStatus</para>
    /// <para>�ړ���ɕύX�����X�e�[�^�X�̏������E���f�����܂�</para>
    /// </summary>
    private void MoveToChangeStatus()
    {
        //�S�[�X�g�ݒ�
        _ghost.ChangeTransformGhost(MyTransform);

        //�ݒu����钼�O�������ꍇ�A��𐔂��J�E���g
        if (0 < _waitTimer) { _lockDownCancel++; }
        //��𐔂��ő�𒴂��Ă��Ȃ��ꍇ�A�ݒu�ҋ@�^�C�}�[��������
        if (_lockDownCancel <= MAX_CANCEL_CNT) { _waitTimer = 0; }
    
    }

    /// <summary>
    /// <para>Commit</para>
    /// <para>������������܂�</para>
    /// </summary>
    private void Commit()
    {
        //T�~�m�ł���ꍇ��T�X�s���������s��
        //�����P : T�~�m�ł��� (1�s�ڍŏ�)
        //�����Q : �Ō�̓��삪��]�ł��� (1�s�ڍŌ�)
        //�����R : ���𒆐S�Ƃ����R���R�̂S�̊p�ɏՓ˔��肪���� (2�s��)
        //���ׂĂ̏��������v�����ꍇ�́AT�X�s�������ݒ肷��
        if(MyModel == IMinoCreatable.MinoType.minoT && _moveDire != 0
            && CheckCollisionByCenter(1,1) && CheckCollisionByCenter(-1, 1) && CheckCollisionByCenter(-1, -1) && CheckCollisionByCenter(1, -1))
        {
            SetTSpin(true); //T�X�s���Ƃ��Đݒ�
        }
        else
        {
            SetTSpin(false); //�ʕ��Ƃ��Đݒ�
        }
        //�~�m�ݒu
        SetMinoForField();
        //��𐔏�����
        _lockDownCancel = 0;
        //�ݒu�ҋ@�^�C�}�[������
        _waitTimer = 0;
        //�^�C�}�[������
        _fallTimer = 0;
        //�z�[���h����
        _isHold = false;
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
                _srsPos.startPos = MyTransform.position; //�������W�ۑ�
                if (_nowAngle == ANGLE_RIGHT_ID || (_nowAngle == ANGLE_DOWN_ID && _moveDire == DIRE_LEFT_ID) || (_nowAngle == ANGLE_UP_ID && _moveDire == DIRE_RIGHT_ID))
                {
                    MyTransform.position += Vector3.left;
                    break; //�������I��
                }
                MyTransform.position += Vector3.right;
                break; //�������I��

            case 1: //��Q���� --------------------------------------------------------------------------------------------------------------------------
                //���W�p��
                if (_nowAngle == ANGLE_RIGHT_ID || _nowAngle == ANGLE_LEFT_ID)
                {
                    MyTransform.position += Vector3.up;
                    break; //�������I��
                }
                MyTransform.position += Vector3.down;
                break; //�������I��

            case 2: //��R���� --------------------------------------------------------------------------------------------------------------------------
                MyTransform.position = _srsPos.startPos; //�������W�����߂�
                if (_nowAngle == ANGLE_RIGHT_ID || _nowAngle == ANGLE_LEFT_ID)
                {
                    MyTransform.position += Vector3.down * 2;
                    break; //�������I��
                }
                MyTransform.position += Vector3.up * 2;
                break; //�������I��

            case 3: //��S���� --------------------------------------------------------------------------------------------------------------------------
                //���W�p��
                if (_nowAngle == ANGLE_RIGHT_ID || (_nowAngle == ANGLE_DOWN_ID && _moveDire == DIRE_LEFT_ID) || (_nowAngle == ANGLE_UP_ID && _moveDire == DIRE_RIGHT_ID))
                {
                    MyTransform.position += Vector3.left;
                    break; //�������I��
                }
                MyTransform.position += Vector3.right;
                break; //�������I��

            case 4: //��]�ł��Ȃ� ----------------------------------------------------------------------------------------------------------------------
                MyTransform.position = _srsPos.startPos; //�������W�����߂�
                _needReturn = true;
                return; //�������I��
        }
        _srsCnt++; //�X�p���e�L�^
        if (CheckMinoCollision()) { SRSByThree(); } //�Փ˔��肪�������ꍇ�A�X�p���e�p��
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
                _srsPos.startPos = MyTransform.position; //�������W�ۑ�
                if(_nowAngle == ANGLE_UP_ID)
                {
                    MyTransform.position += Vector3.left * 2 * _moveDire;
                    break; //�������I��
                }
                if(_nowAngle == ANGLE_DOWN_ID)
                {
                    MyTransform.position += Vector3.left * _moveDire;
                    break; //�������I��
                }
                if(_moveDire == DIRE_RIGHT_ID)
                {
                    if(_nowAngle == ANGLE_RIGHT_ID)
                    {
                        MyTransform.position += Vector3.left * 2;
                    }
                    else
                    {
                        MyTransform.position += Vector3.right * 2;
                    }
                }
                else
                {
                    if (_nowAngle == ANGLE_RIGHT_ID)
                    {
                        MyTransform.position += Vector3.right;
                    }
                    else
                    {
                        MyTransform.position += Vector3.left;
                    }
                }
                break; //�������I��

            case 1: //��Q���� --------------------------------------------------------------------------------------------------------------------------
                _srsPos.firstSRSPos = MyTransform.position; //��P�����ʒu�ۑ�
                MyTransform.position = _srsPos.startPos; //�������W�����߂�
                if (_nowAngle == ANGLE_UP_ID)
                {
                    MyTransform.position += Vector3.right * _moveDire;
                    break; //�������I��
                }
                if (_nowAngle == ANGLE_DOWN_ID)
                {
                    MyTransform.position += Vector3.right * 2 * _moveDire;
                    break; //�������I��
                }
                if (_moveDire == DIRE_LEFT_ID)
                {
                    if (_nowAngle == ANGLE_RIGHT_ID)
                    {
                        MyTransform.position += Vector3.left * 2;
                    }
                    else
                    {
                        MyTransform.position += Vector3.right * 2;
                    }
                }
                else
                {
                    if (_nowAngle == ANGLE_RIGHT_ID)
                    {
                        MyTransform.position += Vector3.right;
                    }
                    else
                    {
                        MyTransform.position += Vector3.left;
                    }
                }
                break; //�������I��

            case 2: //��R���� --------------------------------------------------------------------------------------------------------------------------
                _srsPos.secondSRSPos = MyTransform.position; //��Q�����ʒu�ۑ�
                MyTransform.position = _srsPos.firstSRSPos; //��P���W�����߂�
                if(_nowAngle == ANGLE_RIGHT_ID)
                {
                    if(_moveDire == DIRE_RIGHT_ID)
                    {
                        MyTransform.position += Vector3.down;
                    }
                    else
                    {
                        MyTransform.position += Vector3.down * 2;
                    }
                    break; //�������I��
                }
                if(_nowAngle == ANGLE_LEFT_ID)
                {
                    if(_moveDire == DIRE_RIGHT_ID)
                    {
                        MyTransform.position += Vector3.up;
                    }
                    else
                    {
                        MyTransform.position += Vector3.up * 2;
                    }
                    break; //�������I��
                }
                if(_nowAngle == ANGLE_UP_ID)
                {
                    if(_moveDire == DIRE_LEFT_ID)
                    {
                        MyTransform.position += Vector3.down;
                    }
                    else
                    {
                        MyTransform.position += Vector3.up * 2;
                    }
                    break; //�������I��
                }
                if (_moveDire == DIRE_LEFT_ID)
                {
                    MyTransform.position += Vector3.up;
                }
                else
                {
                    MyTransform.position += Vector3.down * 2;
                }
                break; //�������I��

            case 3: //��S���� --------------------------------------------------------------------------------------------------------------------------
                MyTransform.position = _srsPos.secondSRSPos; //��P�������W�����߂�
                if (_nowAngle == ANGLE_RIGHT_ID)
                {
                    if (_moveDire == DIRE_LEFT_ID)
                    {
                        MyTransform.position += Vector3.up;
                    }
                    else
                    {
                        MyTransform.position += Vector3.up * 2;
                    }
                    break; //�������I��
                }
                if (_nowAngle == ANGLE_LEFT_ID)
                {
                    if (_moveDire == DIRE_LEFT_ID)
                    {
                        MyTransform.position += Vector3.down;
                    }
                    else
                    {
                        MyTransform.position += Vector3.down * 2;
                    }
                    break; //�������I��
                }
                if (_nowAngle == ANGLE_UP_ID)
                {
                    if (_moveDire == DIRE_RIGHT_ID)
                    {
                        MyTransform.position += Vector3.up;
                    }
                    else
                    {
                        MyTransform.position += Vector3.down * 2;
                    }
                    break; //�������I��
                }
                if (_moveDire == DIRE_RIGHT_ID)
                {
                    MyTransform.position += Vector3.down;
                }
                else
                {
                    MyTransform.position += Vector3.up * 2;
                }
                break; //�������I��

            case 4: //��]�ł��Ȃ� -----------------------------------------------------------------------------------------------------------------------
                MyTransform.position = _srsPos.startPos; //�������W�����߂�
                _needReturn = true;
                return; //�������I��
        }
        _srsCnt++; //�X�p���e�L�^
        if (CheckMinoCollision()) { SRSByFour(); } //�Փ˔��肪�������ꍇ�A�X�p���e�p��
        return;
    }

    // �N���X�p��
    public override void CreateMinoUnit(IMinoBlockAccessible[] minoBlocks,IMinoCreatable.MinoType setModel)
    {
        //��ꃁ�\�b�h�g�p
        base.CreateMinoUnit(minoBlocks,setModel);

        //O�~�m��I�~�m�͌`������Ȃ̂ŃX�^�[�g�n�_��ύX����
        if(MyModel == IMinoCreatable.MinoType.minoO) //O�~�m
        {
            MyTransform.position += Vector3.right * IMinoCreatable.EXCEPTION_SHIFT_0_5 + Vector3.up * IMinoCreatable.EXCEPTION_SHIFT_0_5;
        }
        if(MyModel == IMinoCreatable.MinoType.minoI) //I�~�m
        {
            MyTransform.position += Vector3.right * IMinoCreatable.EXCEPTION_SHIFT_0_5 + Vector3.down * IMinoCreatable.EXCEPTION_SHIFT_0_5;
        }

        //�S�[�X�g�ݒ�
        _ghost.ChangeModelGhost(MyModel, MyTransform.position);

        //�����ʒu�Ƀ~�m�����ɂ��邩
        if (CheckMinoCollision())
        {
            Debug.Log("MinoCantPlay");
            //�v���C�s�\
            NotPlay();
        }
    }
    #endregion
}
