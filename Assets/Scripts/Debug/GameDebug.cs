// -------------------------------------------------------------------------------
// GameDebug.cs
//
// �쐬��: 2023/10/30
// �쐬��: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameDebug : MonoBehaviour
{
    #region �ϐ�
    const int FIELD_I_WIDTH = 9; //�e�g���X�p�t�B�[���h�̉���
    const int FIELD_I_HEIGHT = 4; //�e�g���X�p�t�B�[���h�̉���
    readonly int[] _fieldTWidth = {9,8,9 }; //T�X�s���p�t�B�[���h�̊e����
    readonly Vector2 _fieldTEdge = new Vector2(9, 4); //T�X�s���p�t�B�[���h�̈����|����
    readonly Color _debugMinoColor = Color.white / 2 + Color.black; //�f�o�b�O�p�̃~�m�J���[

    private Transform _transform = default;

    private FieldManager _fieldManager = default;
    private MinoPoolManager _minoPool = default;

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
        _transform = transform;
        _fieldManager = GetComponentInParent<FieldManager>();
        _minoPool = GetComponentInParent<MinoPoolManager>();
    }

    /// <summary>
    /// <para>OnDebugTField</para>
    /// <para>�f�o�b�O�p��T�X�s���t�B�[���h���`�����܂�</para>
    /// </summary>
    /// <param name="context">�{�^�����</param>
    public void OnDebugTField(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && Input.GetKey(KeyCode.LeftControl)) //�����ꂽ
        {
            //�t�B�[���h������
            _fieldManager.RemoveField();
            //�v�[������g�p�\�~�m�擾
            IMinoBlockAccessible useMino;
            int y = 0;
            //�t�B�[���h�`��
            foreach(int maxWidth in _fieldTWidth)
            {
                for(int x = 0;x < maxWidth; x++)
                {
                    useMino = _minoPool.GetMinoByPool();
                    useMino.ChangeColor(_debugMinoColor);
                    useMino.SetMinoPos(x, y, _transform);
                    _fieldManager.SetMino(useMino.MinoX, useMino.MinoY);

                }
                y++;
            }
            //�����|����ݒ�
            useMino = _minoPool.GetMinoByPool();
            useMino.ChangeColor(_debugMinoColor);
            useMino.SetMinoPos(_fieldTEdge.x, _fieldTEdge.y, _transform);
            _fieldManager.SetMino(useMino.MinoX, useMino.MinoY);

            _fieldManager.DeleteCommit(); //�R�~�b�g���Ȃ��悤��
            //�q�t������
            foreach (IMinoBlockAccessible mino in _transform.GetComponentsInChildren<IMinoBlockAccessible>())
            {
                mino.DisConnect();
            }
            return; //�����I��
        }
    }

    /// <summary>
    /// <para>OnDebugIField</para>
    /// <para>�f�o�b�O�p�̃e�g���X�t�B�[���h���`�����܂�</para>
    /// </summary>
    /// <param name="context">�{�^�����</param>
    public void OnDebugIField(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && Input.GetKey(KeyCode.LeftControl)) //�����ꂽ
        {
            //�t�B�[���h������
            _fieldManager.RemoveField();
            //�v�[������g�p�\�~�m�擾
            IMinoBlockAccessible useMino;
            //�t�B�[���h�`��
            for(int y = 0; y < FIELD_I_HEIGHT; y++)
            {
                for(int x = 0; x < FIELD_I_WIDTH; x++)
                {
                    useMino = _minoPool.GetMinoByPool();
                    useMino.ChangeColor(_debugMinoColor);
                    useMino.SetMinoPos(x, y, _transform);
                    _fieldManager.SetMino(useMino.MinoX, useMino.MinoY);

                }
            }
            _fieldManager.DeleteCommit(); //�R�~�b�g���Ȃ��悤��
            //�q�t������
            foreach(IMinoBlockAccessible mino in _transform.GetComponentsInChildren<IMinoBlockAccessible>())
            {
                mino.DisConnect();
            }
            return; //�����I��
        }
    }
    /// <summary>
    /// <para>OnDebugLevelUp</para>
    /// <para>�f�o�b�O�p�̃��x���A�b�v�������s���܂�</para>
    /// </summary>
    /// <param name="context">�{�^�����</param>
    public void OnDebugLevelUp(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && Input.GetKey(KeyCode.LeftControl)) //�����ꂽ
        {
            //���x���A�b�v
            _fieldManager.LevelUp();
            return;
        }
    }
    #endregion
}
