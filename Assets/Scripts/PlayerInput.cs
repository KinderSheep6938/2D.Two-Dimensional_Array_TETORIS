// -------------------------------------------------------------------------------
// PlayerInput.cs
//
// �쐬��: 2023/10/21
// �쐬��: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    #region �ϐ�
    //�I�[�g���s�[�g�@�\�i�������ړ��j
    private bool _onAutoRepeat = false; //�I�[�g���s�[�g�J�n����
    private int _autoRepeatDire = 1; //�I�[�g���s�[�g����
    private const int RIGHT_DIREID = 1; //�E����ID
    private const int LEFT_DIREID = -1; //������ID
    private const float AUTOREPEAT_MOVETIME = 0.05f; //�I�[�g���s�[�g�̈ړ��ҋ@����
    private float _autoRepeatTimer = 0f; //�I�[�g���s�[�g�^�C�}�[

    private bool _isSoft = false; //�\�t�g�h���b�v�J�n����

    private bool _isStart = false; //�Q�[���J�n�t���O
    private bool _canGame = false; //�Q�[���v���C�\�t���O

    private IMinoUnionCtrl _minoUnion = default; //�~�m����V�X�e���̃C���^�[�t�F�C�X
    private IMinoHoldable _holdSystem = default; //�z�[���h�V�X�e���̃C���^�[�t�F�C�X
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
    /// �X�V����
    /// </summary>
    void Update()
    {
        //�I�[�g���s�[�g���肪����
        if (_onAutoRepeat) { AutoRepeat(_autoRepeatDire); }
        //�\�t�g�h���b�v���肪����
        if (_isSoft) { _minoUnion.SoftDrop(); }
    }

    /// <summary>
    /// <para>OnMoveLeft</para>
    /// <para>���ړ��{�^���������ꂽ�Ƃ��ɌĂ΂�܂�</para>
    /// </summary>
    /// <param name="context">�{�^�����</param>
    public void OnMoveLeft(InputAction.CallbackContext context)
    {
        if (!_canGame) { return; /*�܂��J�n���Ă��Ȃ�*/ }

        if (context.phase == InputActionPhase.Started) //�����ꂽ
        {
            _minoUnion.Move(LEFT_DIREID);
            _onAutoRepeat = false;
            AutoRepeatReset();
        }

        if (context.phase == InputActionPhase.Performed) //������Ă���w�肵�����Ԃ�������
        {
            _onAutoRepeat = true;
            _autoRepeatDire = LEFT_DIREID;
        }

        if (context.phase == InputActionPhase.Canceled) //���ꂽ
        {
            _onAutoRepeat = false;
            AutoRepeatReset();
        }
    }

    /// <summary>
    /// <para>OnMoveRight</para>
    /// <para>�E�ړ��{�^���������ꂽ�Ƃ��ɌĂ΂�܂�</para>
    /// </summary>
    /// <param name="context">�{�^�����</param>
    public void OnMoveRight(InputAction.CallbackContext context)
    {
        if (!_canGame) { return; /*�܂��J�n���Ă��Ȃ�*/ }

        if (context.phase == InputActionPhase.Started) //�����ꂽ
        {
            _minoUnion.Move(RIGHT_DIREID);
            _onAutoRepeat = false;
            AutoRepeatReset();
        }

        if (context.phase == InputActionPhase.Performed) //������Ă���w�肵�����Ԃ�������
        {
            _onAutoRepeat = true;
            _autoRepeatDire = RIGHT_DIREID;
        }

        if (context.phase == InputActionPhase.Canceled) //���ꂽ
        {
            _onAutoRepeat = false;
            AutoRepeatReset();
        }
    }

    /// <summary>
    /// <para>OnRotateLeft</para>
    /// <para>����]�{�^���������ꂽ�Ƃ��ɌĂ΂�܂�</para>
    /// </summary>
    /// <param name="context">�{�^�����</param>
    public void OnRotateLeft(InputAction.CallbackContext context)
    {
        if (!_canGame) { return; /*�܂��J�n���Ă��Ȃ�*/ }

        if (context.phase == InputActionPhase.Performed) //�����ꂽ
        {
            _minoUnion.Rotate(LEFT_DIREID);
        }
    }

    /// <summary>
    /// <para>OnRotateRight</para>
    /// <para>�E��]�{�^���������ꂽ�Ƃ��ɌĂ΂�܂�</para>
    /// </summary>
    /// <param name="context">�{�^�����</param>
    public void OnRotateRight(InputAction.CallbackContext context)
    {
        if (!_canGame) { return; /*�܂��J�n���Ă��Ȃ�*/ }

        if (context.phase == InputActionPhase.Performed) //�����ꂽ
        {
            _minoUnion.Rotate(RIGHT_DIREID);
        }
    }

    /// <summary>
    /// <para>OnHardDrop</para>
    /// <para>�n�[�h�h���b�v�{�^���������ꂽ�Ƃ��ɌĂ΂�܂�</para>
    /// </summary>
    /// <param name="context">�{�^�����</param>
    public void OnHardDrop(InputAction.CallbackContext context)
    {
        if (!_canGame) { return; /*�܂��J�n���Ă��Ȃ�*/ }

        if (context.phase == InputActionPhase.Performed) //�����ꂽ
        {
            _minoUnion.HardDrop();
        }
    }

    /// <summary>
    /// <para>OnSoftDrop</para>
    /// <para>�\�t�g�h���b�v�{�^���������ꂽ�Ƃ��ɌĂ΂�܂�</para>
    /// </summary>
    /// <param name="context">�{�^�����</param>
    public void OnSoftDrop(InputAction.CallbackContext context)
    {
        if (!_canGame) { return; /*�܂��J�n���Ă��Ȃ�*/ }

        if (context.phase == InputActionPhase.Started) //�����ꂽ
        {
            _isSoft = true;
        }

        if (context.phase == InputActionPhase.Canceled) //���ꂽ
        {
            _isSoft = false;
        }
    }

    /// <summary>
    /// <para>OnHold</para>
    /// <para>�z�[���h�{�^���������ꂽ�Ƃ��ɌĂ΂�܂�</para>
    /// </summary>
    /// <param name="context">�{�^�����</param>
    public void OnHold(InputAction.CallbackContext context)
    {
        if (!_canGame) { return; /*�܂��J�n���Ă��Ȃ�*/ }

        if (context.phase == InputActionPhase.Performed)
        {
            //�z�[���h���Ȃ��ꍇ�́A�z�[���h����
            if (!_minoUnion.CheckHasHold()) { _holdSystem.Hold(); }
        }
    }

    /// <summary>
    /// <para>OnStart</para>
    /// <para>�J�n�{�^���������ꂽ�Ƃ��ɌĂ΂�܂�</para>
    /// </summary>
    /// <param name="context">�{�^�����</param>
    public void OnStart(InputAction.CallbackContext context)
    {
        //�܂��Q�[�����X�^�[�g���Ă��Ȃ�
        if (context.phase == InputActionPhase.Performed && !_isStart) //�����ꂽ
        {
            _canGame = true; //�v���C�\��
            _isStart = true; //�Q�[���J�n
        }
    }

    /// <summary>
    /// <para>AutoRepeat</para>
    /// <para>���ړ��̒������������s���܂�</para>
    /// </summary>
    /// <param name="x"></param>
    private void AutoRepeat(int x)
    {
        _autoRepeatTimer += Time.deltaTime; //�^�C�}�[���Z
                                            //�^�C�}�[���ҋ@���Ԃ��z����
        if (AUTOREPEAT_MOVETIME <= _autoRepeatTimer)
        {
            //�ړ�
            _minoUnion.Move(x);
            //�^�C�}�[������
            _autoRepeatTimer = 0;
        }
    }

    /// <summary>
    /// <para>AutoRepeatReset</para>
    /// <para>�����������̕ϐ��̏��������s���܂�</para>
    /// </summary>
    private void AutoRepeatReset()
    {
        //������
        _autoRepeatTimer = 0;
        _autoRepeatDire = 0;
    }

    /// <summary>
    /// <para>SetStopInput</para>
    /// <para>�v���C���[���͂��󂯕t���Ȃ��悤�ɂ��܂�</para>
    /// </summary>
    public void SetStopInput()
    {
        _canGame = false; //�v���C�s�\��
    }
    #endregion
}
