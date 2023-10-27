// -------------------------------------------------------------------------------
// GameCtrlManager.cs
//
// �쐬��: 2023/10/21
// �쐬��: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameCtrlManager : MonoBehaviour
{
    #region �ϐ�
    private const int GAMEEND_ID = -1; //�Q�[���I��ID
    private const int COMMIT_ID = 1; //���슮��ID
    private int _nowGameStatus = 0; //���݂̃Q�[����
    private bool _isStart = false; //�Q�[���J�n�t���O
    [SerializeField, Header("�Q�[���X�^�[�g�e�L�X�g")]
    private GameObject _gameStartText = default;

    private bool _canRetry = false; //���g���C�\�t���O
    [SerializeField, Header("�Q�[���I�[�o�[�e�L�X�g")]
    private GameObject _gameoverViewObj = default;

    private AudioSource _bgmAudio = default; //BGM��AudioSource
    private MinoFactory _factorySystem = default; //�~�m�����V�X�e��
    private FieldManager _fieldSystem = default; //�t�B�[���h�Ǘ��}�l�[�W���[
    private PlayerInput _playerInput = default; //�v���C���[����}�l�[�W���[
    #endregion

    #region ���\�b�h
    /// <summary>
    /// ����������
    /// </summary>
    void Awake()
    {
        //������
        _bgmAudio = GetComponent<AudioSource>();
        _factorySystem = GetComponent<MinoFactory>();
        _fieldSystem = GetComponent<FieldManager>();
        _playerInput = GetComponentInChildren<PlayerInput>();
    }

    /// <summary>
    /// �X�V����
    /// </summary>
    void Update()
    {
        //�Q�[����
        _nowGameStatus = _fieldSystem.GetPlayStatus();

        //�Q�[���󋵂��R�~�b�g��Ԃł͂Ȃ�
        if(_nowGameStatus != COMMIT_ID)
        {
            //�Q�[����Ԃ��I����Ԃł���
            if(_nowGameStatus == GAMEEND_ID)
            {
                _gameoverViewObj.SetActive(true); //�Q�[���I�[�o�[�\��
                _canRetry = true; //���g���C�\��
                _bgmAudio.Stop(); //BGM��~
                _playerInput.SetStopInput(); //�v���C���[���͕s�\��
            }
            return; //����I��
        }
        
        //�R�~�b�g��Ԃł���ꍇ�͐V�����~�m�𐶐�����
        _factorySystem.CreateMino();
    }


    /// <summary>
    /// <para>OnStart</para>
    /// <para>�J�n�{�^���������ꂽ�Ƃ��ɌĂ΂�܂�</para>
    /// </summary>
    /// <param name="context">�{�^�����</param>
    public void OnStart(InputAction.CallbackContext context)
    {
        //�Q�[���J�n�{�^���������ꂽ ���� �X�^�[�g���Ă��Ȃ�
        if (context.phase == InputActionPhase.Started && !_isStart)
        { //�Q�[���X�^�[�g
            _isStart = true; //�J�n�t���O
            _gameStartText.SetActive(false); //�e�L�X�g��\��
            _bgmAudio.Play(); //BGM�Đ�

            //�l�N�X�g�~�m�Ƒ���~�m�̓�Ƀ~�m�𑗂�Ȃ���΂����Ȃ����߁A�~�m�������Q��s��
            _factorySystem.CreateMino(); //�~�m����
            _factorySystem.CreateMino(); //�~�m����
        }
    }

    /// <summary>
    /// <para>OnRetry</para>
    /// <para>�Ē���{�^���������ꂽ�Ƃ��ɌĂ΂�܂�</para>
    /// </summary>
    /// <param name="context">�{�^�����</param>
    public void OnRetry(InputAction.CallbackContext context)
    {
        //���g���C�{�^���������ꂽ ���� ���g���C�\�ł���
        if (context.phase == InputActionPhase.Started && _canRetry)
        { //���g���C
            //�����V�[���ǂݍ���
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    #endregion
}
