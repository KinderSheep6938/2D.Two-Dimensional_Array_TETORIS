// -------------------------------------------------------------------------------
// ScoreUIManager.cs
//
// �쐬��: 2023/10/25
// �쐬��: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUIManager : MonoBehaviour
{
    #region �ϐ�
    [SerializeField,Header("�X�R�A�\���e�L�X�g")]
    private Text _scoreText = default;
    [SerializeField,Header("���x���\���e�L�X�g")]
    private Text _levelText = default;

    private ScoreManager _scoreManager = default; //�X�R�A�Ǘ��N���X
    #endregion

    #region ���\�b�h
    /// <summary>
    /// ����������
    /// </summary>
    void Awake()
    {
        _scoreManager = FindObjectOfType<ScoreManager>();
    }

    /// <summary>
    /// �X�V����
    /// </summary>
    void Update()
    {
        //�X�R�A�\��
        SetTextToScore();
    }

    /// <summary>
    /// <para>OutputDisplay</para>
    /// <para>�X�R�A��\������Ă���e�L�X�g�ɐݒ肵�܂�</para>
    /// </summary>
    private void SetTextToScore()
    {
        _scoreText.text = _scoreManager.Score.ToString();
        _levelText.text = _scoreManager.Level.ToString();
    }

    
    #endregion
}
