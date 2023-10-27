// -------------------------------------------------------------------------------
// TetrisEffect.cs
//
// �쐬��: 2023/10/26
// �쐬��: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextEffect : MonoBehaviour
{
    #region �ϐ�
    const float MAX_VIEW_TIME = 2.5f; //�e�L�X�g�̕\������

    private float _viewTimer = 0f; //�\���J�E���g�^�C�}�[
    private bool _nowShow = false; //�\������t���O
    private Text _viewText = default; //�\���e�L�X�g
    #endregion

    #region ���\�b�h
    /// <summary>
    /// ����������
    /// </summary>
    void Awake()
    {
        _viewText = GetComponent<Text>();
    }

    /// <summary>
    /// �X�V����
    /// </summary>
    void Update()
    {
        //�\�����肪����ꍇ�́A�\���Ǘ����s��
        if (_nowShow) { ViewCtrl(); }
    }

    /// <summary>
    /// <para>ViewCtrl</para>
    /// <para>�\���Ǘ������܂�</para>
    /// </summary>
    private void ViewCtrl()
    {
        //�^�C�}�[���Z
        _viewTimer += Time.deltaTime;

        //�\�����Ԃ𖞂�����
        if (MAX_VIEW_TIME <= _viewTimer)
        {
            //�\�������s����
            _nowShow = false;
            //��\���ɐݒ�
            _viewText.enabled = false;

        }
    }

    /// <summary>
    /// <para>SetView</para>
    /// <para>�\����Ԃɐݒ肵�܂�</para>
    /// </summary>
    public void SetView()
    {
        //�\�����������
        _nowShow = true;

        //�\���ɐݒ�
        _viewText.enabled = true;
        //�^�C�}�[������
        _viewTimer = 0;
    }
    #endregion
}
