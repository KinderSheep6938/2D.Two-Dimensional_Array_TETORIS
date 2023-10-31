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

    //T�X�s������p
    private const int TSPIN_JUDGE_CNT = 3; //T�X�s��������l
    private const int MINITSPIN_JUDGE_VALUE270 = 270; //�~�jT�X�s������p�p�x�P
    private const int MINITSPIN_JUDGE_VALUE180 = 180; //�~�jT�X�s������p�p�x�Q
    private const int MINITSPIN_JUDGE_SRSCNT = 4; //�~�jT�X�s��

    //�v���C���[���쐧��p
    private const int ROTATE_VALUE = 90; //��]�����̉�]�p�x

    //��]�n�� �X�p���e�Ȃ�
    private int _nowAngleID = 0; //���݂̃~�m�̌���
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

    //�\�t�g�h���b�v�@�\
    private const float SOFTDROP_FALLTIME = 0.05f; //�\�t�g�h���b�v�̎���
    private float _softDropTimer = 0f; //�\�t�g�h���b�v�^�C�}�[

    //�z�[���h����
    private bool _isHold = false; //�z�[���h�g�p����

    //�T�E���h�֘A
    [SerializeField, Header("��]SE")]
    private AudioClip _rotateSE = default;
    [SerializeField, Header("T�X�s��SE")]
    private AudioClip _tSpinSE = default;
    [SerializeField, Header("�n�[�h�h���b�vSE")]
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
        //������
        _ghost = transform.parent.GetComponentInChildren<GhostMino>();
        _myAudio = GetComponent<AudioSource>();
    }

    /// <summary>
    /// �X�V����
    /// </summary>
    void Update()
    {
        //�~�m��������Ă��Ȃ�
        if(MyTransform == default) { return; }

        //�~�m�u���b�N���n����Ă��Ȃ��i�q�t������Ă��Ȃ��j
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
    public void Rotate(int dire)
    {
        //��]���f
        MyTransform.eulerAngles -= Vector3.forward * ROTATE_VALUE * dire;
        
        //���K���擾
        _nowAngleID = GetAngleID(Mathf.FloorToInt(MyTransform.eulerAngles.z)); //�����擾
        _moveDire = dire; //��]�����擾
        _needReturn = false; //��]�\

        //�X�p���e�񐔏�����
        _srsCnt = 0;
        //�Փ˔��肪�������ꍇ�̓X�[�p�[���[�e�[�V�����V�X�e�������s����
        if (CheckMinoCollision())
        {
            if (MyModel == IMinoCreatable.MinoType.minoI) { SRSByFour(); } //�T�C�Y���S���S
            SRSByThree(); //�T�C�Y���R���R

        }

        //�X�p���e���ł��Ȃ��ꍇ�͊p�x��߂�
        if (_needReturn)
        {
            //�p�x��߂�
            MyTransform.eulerAngles -= Vector3.forward * ROTATE_VALUE * -dire; 
            _nowAngleID = GetAngleID(Mathf.FloorToInt(MyTransform.eulerAngles.z)); //�����擾
            _moveDire = 0; //��]��������
            return;
        }

        //T�X�s�����肪����ꍇ�́A���ʉ���ς���
        if(MyModel == IMinoCreatable.MinoType.minoT && EdgeCollisionByTSpin())
        {
            _myAudio.PlayOneShot(_tSpinSE); //T�X�s�����ʉ��Đ�
        }
        else
        {
            _myAudio.PlayOneShot(_rotateSE); //�ʏ���ʉ��Đ�
        }

        //�X�e�[�^�X���f
        MoveToChangeStatus();
    }

    /// <summary>
    /// <para>GetAngleID</para>
    /// <para>�p�x����p�xID���擾���܂�</para>
    /// </summary>
    /// <param name="angle">����~�m�̊p�x</param>
    /// <returns>�p�xID</returns>
    public int GetAngleID(int angle)
    {
        //�p�x�͑S�ă}�C�i�X�����ɒ���������Ԃɂ���
        //0�̏ꍇ��0��Ԃ�
        if(angle == 0) { return 0; }
        //�p�x���v���X�ł���
        if (0 < angle)
        {
            return (angle - 360) / ROTATE_VALUE * -1;
        }
        return angle / ROTATE_VALUE * -1;
    }
    
    //�C���^�[�t�F�C�X�p��
    public void HardDrop()
    {
        //������ɏՓ˔��肪�Ȃ�
        if (!CheckMinoCollision(0, -1))
        {
            //��]��������
            _moveDire = 0;
            //�P�񗎉�
            MyTransform.position += Vector3.down;
            //�ċN�Ăяo��
            HardDrop();
        }
        else //����
        {
            //���ʉ��Đ�
            _myAudio.PlayOneShot(_hardDropSE);
            //�R�~�b�g
            Commit();

            return; //�����I��
        }

        return;
    }

    //�C���^�[�t�F�C�X�p��
    public void SoftDrop()
    {
        //�����󔒂ł͂Ȃ�
        if (!CheckMinoCollision(0, -1))
        {
            _softDropTimer += Time.deltaTime; //�^�C�}�[���Z
            //�������Ԉȏ�o����
            if(SOFTDROP_FALLTIME <= _softDropTimer)
            {
                MyTransform.position += Vector3.down; //1�}�X����
                _softDropTimer = 0; //�^�C�}�[������
            }
            _fallTimer = 0; //���R�������~�i�O�ŃX�g�b�v�j
        }
    }

    //�C���^�[�t�F�C�X�p��
    public bool CheckHasHold()
    {
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
            //��]��������
            _moveDire = 0;

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
        //T�X�s���������s��
        //�~�jT�X�s���͍���̏ꍇ��T�X�s���ł͂Ȃ����̂Ƃ��Ĉ���
        if(JudgeTSpin())
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
    /// <para>JudgeTSpin</para>
    /// <para>T�X�s�������肵�܂�</para>
    /// </summary>
    /// <returns>T�X�s������</returns>
    private bool JudgeTSpin()
    {
        //�O��FT�~�m�ł͂Ȃ�
        if(MyModel != IMinoCreatable.MinoType.minoT) { return false; }

        //�����P�F���𒆐S�Ƃ����R���R�̂S�̋��ɏՓ˔��肪����
        //����̂���������T�X�s������l��菬�����ꍇ�́AT�X�s���ł͂Ȃ�
        if (!EdgeCollisionByTSpin()) { return false; }

        //�����Q�F�Ō�̓��삪��]�ł���
        //��]�łȂ��ꍇ�́AT�X�s���ł͂Ȃ�
        if (_moveDire == 0) { return false; }

        //��������̓~�jT�X�s��������s��
        //�����̏����ɍ��v�����ꍇ�́A�~�jT�X�s���̂���false�ƕԂ�
        //�����P�F�ʂ̗��[�̂ǂ��炩�ɏՓ˔��肪�Ȃ�
        if(!(CheckCollisionByCenter(TSpinCos(_nowAngleID), TSpinSin(_nowAngleID)) && CheckCollisionByCenter(TSpinSin(_nowAngleID), -TSpinCos(_nowAngleID))))
        {
            //�����Q�F�X�p���e�̉�]�␳�̑�l�����łȂ�����
            if (_srsCnt != MINITSPIN_JUDGE_SRSCNT) { return false; } //�~�jT�X�s���ł���
        }

        //���ׂĂ̏����ɍ��v����ꍇ�́AT�X�s���ł���
        return true;

    }
    
    /// <summary>
    /// <para>EdgeCollisionByTSpin</para>
    /// <para>���̏Փ˔��肪3�ȏ゠��ꍇ�ATrue��Ԃ��܂�</para>
    /// </summary>
    /// <returns></returns>
    private bool EdgeCollisionByTSpin()
    {
        //���Փ˔���J�E���g�p
        int collisionCnt = 0;
        //������
        if (CheckCollisionByCenter(1, 1)) { collisionCnt++; }
        if (CheckCollisionByCenter(-1, 1)) { collisionCnt++; }
        if (CheckCollisionByCenter(1, -1)) { collisionCnt++; }
        if (CheckCollisionByCenter(-1, -1)) { collisionCnt++; }
        //3�ȏ゠��ꍇ��True
        if (TSPIN_JUDGE_CNT <= collisionCnt) { return true; }


        return false; //3�ɖ������Ȃ�
    }

    /// <summary>
    /// <para>TSpinCos</para>
    /// <para>�~�jT�X�s���̃A���S���Y�����i</para>
    /// <para>�^����ꂽ�l��180�x�ȏ�ł����-1</para>
    /// <para>�ȉ��ł����1��Ԃ��܂�</para>
    /// </summary>
    /// <param name="angleID">�p�x</param>
    /// <returns></returns>
    private int TSpinSin(float angleID)
    {
        if(MINITSPIN_JUDGE_VALUE180 <= angleID * ROTATE_VALUE)
        {
            return -1;
        }
        return 1;
    }

    /// <summary>
    /// <para>TSpinCos</para>
    /// <para>�~�jT�X�s���̃A���S���Y�����i</para>
    /// <para>�^����ꂽ�l��270�x�܂���0�x�̎���-1</para>
    /// <para>����ȊO�ł����1��Ԃ��܂�</para>
    /// </summary>
    /// <param name="angleID">�p�x</param>
    /// <returns></returns>
    private int TSpinCos(float angleID)
    {
        if (angleID * ROTATE_VALUE % MINITSPIN_JUDGE_VALUE270 == 0)
        {
            return -1;
        }
        return 1;
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
                if (_nowAngleID == ANGLE_RIGHT_ID || (_nowAngleID == ANGLE_DOWN_ID && _moveDire == DIRE_LEFT_ID) || (_nowAngleID == ANGLE_UP_ID && _moveDire == DIRE_RIGHT_ID))
                {
                    MyTransform.position += Vector3.left;
                    break; //�������I��
                }
                MyTransform.position += Vector3.right;
                break; //�������I��

            case 1: //��Q���� --------------------------------------------------------------------------------------------------------------------------
                //���W�p��
                if (_nowAngleID == ANGLE_RIGHT_ID || _nowAngleID == ANGLE_LEFT_ID)
                {
                    MyTransform.position += Vector3.up;
                    break; //�������I��
                }
                MyTransform.position += Vector3.down;
                break; //�������I��

            case 2: //��R���� --------------------------------------------------------------------------------------------------------------------------
                MyTransform.position = _srsPos.startPos; //�������W�����߂�
                if (_nowAngleID == ANGLE_RIGHT_ID || _nowAngleID == ANGLE_LEFT_ID)
                {
                    MyTransform.position += Vector3.down * 2;
                    break; //�������I��
                }
                MyTransform.position += Vector3.up * 2;
                break; //�������I��

            case 3: //��S���� --------------------------------------------------------------------------------------------------------------------------
                //���W�p��
                if (_nowAngleID == ANGLE_RIGHT_ID || (_nowAngleID == ANGLE_DOWN_ID && _moveDire == DIRE_LEFT_ID) || (_nowAngleID == ANGLE_UP_ID && _moveDire == DIRE_RIGHT_ID))
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
                if(_nowAngleID == ANGLE_UP_ID)
                {
                    MyTransform.position += Vector3.left * 2 * _moveDire;
                    break; //�������I��
                }
                if(_nowAngleID == ANGLE_DOWN_ID)
                {
                    MyTransform.position += Vector3.left * _moveDire;
                    break; //�������I��
                }
                if(_moveDire == DIRE_RIGHT_ID)
                {
                    if(_nowAngleID == ANGLE_RIGHT_ID)
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
                    if (_nowAngleID == ANGLE_RIGHT_ID)
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
                if (_nowAngleID == ANGLE_UP_ID)
                {
                    MyTransform.position += Vector3.right * _moveDire;
                    break; //�������I��
                }
                if (_nowAngleID == ANGLE_DOWN_ID)
                {
                    MyTransform.position += Vector3.right * 2 * _moveDire;
                    break; //�������I��
                }
                if (_moveDire == DIRE_LEFT_ID)
                {
                    if (_nowAngleID == ANGLE_RIGHT_ID)
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
                    if (_nowAngleID == ANGLE_RIGHT_ID)
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
                if(_nowAngleID == ANGLE_RIGHT_ID)
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
                if(_nowAngleID == ANGLE_LEFT_ID)
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
                if(_nowAngleID == ANGLE_UP_ID)
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
                if (_nowAngleID == ANGLE_RIGHT_ID)
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
                if (_nowAngleID == ANGLE_LEFT_ID)
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
                if (_nowAngleID == ANGLE_UP_ID)
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
            //�v���C�s�\
            NotPlay();
        }
    }
    #endregion
}
