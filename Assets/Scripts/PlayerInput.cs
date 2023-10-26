// -------------------------------------------------------------------------------
// PlayerInput.cs
//
// �쐬��: 2023/10/21
// �쐬��: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    #region �ϐ�
    private const float AUTOREPEAT_TIME = 0.3f; //�������ҋ@����
    private float _longPushTimer = 0f; //�������^�C�}�[

    private const float AUTOREPEAT_MOVETIME = 0.05f; //�I�[�g���s�[�g�̈ړ��ҋ@����
    private float _autoRepeatTimer = 0f; //�I�[�g���s�[�g�^�C�}�[

    private bool _isOnce = false; //��񉟂�����

    private IMinoUnionCtrl _minoUnion = default; //�~�m����V�X�e���̃C���^�[�t�F�C�X
    private IMinoHoldable _holdSystem = default; //�z�[���h�V�X�e���̃C���^�[�t�F�C�X
    #endregion

    #region �v���p�e�B

    #endregion

    #region ���\�b�h
    /// <summary>
    /// ����������
    /// </summary>
    void Awake()
    {
        _minoUnion = GetComponent<IMinoUnionCtrl>();
        _holdSystem = FindObjectOfType<HoldMino>().GetComponent<IMinoHoldable>();
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
        PlayerIO();
    }

    /// <summary>
    /// <para>PlayerIO</para>
    /// <para>�v���C���[�̓��͂ɉ����āA�~�m�𑀍삵�܂�</para>
    /// </summary>
    private void PlayerIO()
    {
        //�ړ�
        InputMoveButton();
        //����]
        if (Input.GetKeyDown(KeyCode.Q)) { _minoUnion.Rotate(-1); }
        //�E��]
        if (Input.GetKeyDown(KeyCode.E)) { _minoUnion.Rotate(1); }
        //�n�[�h�h���b�v
        if (Input.GetKeyDown(KeyCode.W)) { _minoUnion.HardDrop(); }
        //�\�t�g�h���b�v
        if (Input.GetKey(KeyCode.S)) { _minoUnion.SoftDrop(); }
        //�z�[���h
        //�܂��z�[���h�����Ă��Ȃ��ꍇ�̂ݔ���
        if (Input.GetKeyDown(KeyCode.LeftShift) && _minoUnion.CheckHold()) { _holdSystem.Hold(); }
    }

    /// <summary>
    /// <para>InputMoveButton</para>
    /// <para>���ړ����͂̐�����s���܂�</para>
    /// </summary>
    private void InputMoveButton()
    {
        //���ړ�
        if (Input.GetKey(KeyCode.A))
        {
            AutoRepeat(-1);
            return;
        }
        //�E�ړ�
        if (Input.GetKey(KeyCode.D))
        {
            AutoRepeat(1);
            return;
        }
        AutoRepeatReset();
    }

    /// <summary>
    /// <para>AutoRepeat</para>
    /// <para>���ړ��̒������������s���܂�</para>
    /// </summary>
    /// <param name="x"></param>
    private void AutoRepeat(int x)
    {
        //�������ҋ@���Ԃ��z���Ă��Ȃ�
        if(_longPushTimer <= AUTOREPEAT_TIME)
        {
            //��񉟂����肪�Ȃ�
            if (!_isOnce)
            {
                //�ړ�
                _minoUnion.Move(x);
                //��񉟂�����
                _isOnce = true;
            }
        }
        else //������
        {
            _autoRepeatTimer += Time.deltaTime; //�^�C�}�[���Z
            //�^�C�}�[���ҋ@���Ԃ��z����
            if(AUTOREPEAT_MOVETIME <= _autoRepeatTimer)
            {
                //�ړ�
                _minoUnion.Move(x);
                //�^�C�}�[������
                _autoRepeatTimer = 0;
            }
        }

        _longPushTimer += Time.deltaTime; //�^�C�}�[���Z
    }

    /// <summary>
    /// <para>AutoRepeatReset</para>
    /// <para>�����������̕ϐ��̏��������s���܂�</para>
    /// </summary>
    private void AutoRepeatReset()
    {
        //������
        _autoRepeatTimer = 0;
        _longPushTimer = 0;
        _isOnce = false;
    }
    #endregion
}
