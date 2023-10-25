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

    private int _nowAngle = 0; //���݂̃~�m�̌���
    private int _moveDire = 0; //��]����
    private bool _needReturn = false; //��]�����߂�����
    private int _srsCnt = 0; //�X�p���e�̉�
    private float _fallTime = 0.8f; //��������
    private float _timer = 0; //�����v���^�C�}�[

    [SerializeField, Tooltip("��]SE")]
    private AudioClip _rotateSE = default;
    [SerializeField, Tooltip("�n�[�h�h���b�vSE")]
    private AudioClip _hardDropSE = default;
    private AudioSource _myAudio = default; //���g��AudioSource
    private IGhostStartable _ghost = default; //�S�[�X�g�V�X�e��
    #endregion

    #region �v���p�e�B
    public float FallTime { set => _fallTime = value; }
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
        if (MyTransform.childCount == 0) { return; }

        //���Ԍo�ߗ���
        FallMino();
    }

    // �C���^�[�t�F�C�X�p��
    public void Move(int x)
    {
        //�ړ����f
        MyTransform.position += Vector3.right * x;
        //�Փ˔��肪���邩
        if (CheckMino())
        { 
            //����ꍇ�͌��ɖ߂�
            MyTransform.position -= Vector3.right * x;
            return;
        }
        //�S�[�X�g�ݒ�
        _ghost.ChangeTransformGhost(MyTransform);
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
        if (CheckMino())
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
            return;
        }
        //�S�[�X�g�ݒ�
        _ghost.ChangeTransformGhost(MyTransform);
    }
    
    //�C���^�[�t�F�C�X�p��
    public void HardDrop()
    {
        _myAudio.PlayOneShot(_hardDropSE); //���ʉ��Đ�
        //�P�񗎉�
        MyTransform.position += Vector3.down;

        //������ɏՓ˔��肪����
        if (CheckMino())
        {
            //���Ƃɖ߂�
            MyTransform.position += Vector3.up;
            //�~�m���t�B�[���h�ɐݒ�
            SetMinoForField();
            //�����^�C�}�[������
            _timer = 0;

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
        _timer += Time.deltaTime * SOFTDROP_SPEED;
    }

    private void FallMino()
    {
        _timer += Time.deltaTime; //�^�C�}�[���Z

        //�������ԂɂȂ�����
        if(_fallTime < _timer)
        {
            _timer = 0;
            //�~�m���P�}�X����
            MyTransform.position += Vector3.down;

            //������ɏՓ˔��肪����
            if (CheckMino())
            {
                //���Ƃɖ߂�
                MyTransform.position += Vector3.up;
                //�~�m���t�B�[���h�ɐݒ�
                SetMinoForField();
            }
        }
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
        if (CheckMino()) { SRSByFour(); } //�Փ˔��肪�������ꍇ�A�X�p���e�p��
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
        if (CheckMino())
        {

        }
    }
    #endregion
}
