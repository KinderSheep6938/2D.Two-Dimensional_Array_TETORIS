// -------------------------------------------------------------------------------
// LineEffect.cs
//
// �쐬��: 2023/10/25
// �쐬��: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineEffect : MonoBehaviour
{
    #region �ϐ�
    private readonly Vector3 _effectWidthPos = new Vector3(4.5f, 0, 0); //�G�t�F�N�g�����ʒu�̉���

    private Transform _transform = default; //���g��Transform
    private ParticleSystem _particleSystem = default; //���g��ParticleSystem
    #endregion

    #region ���\�b�h
    /// <summary>
    /// ����������
    /// </summary>
    void Awake()
    {
        _transform = transform;
        _particleSystem = _transform.GetComponent<ParticleSystem>();
    }

    /// <summary>
    /// <para>SetEffect</para>
    /// <para>�w�肵����ɃG�t�F�N�g�𔭐������܂�</para>
    /// </summary>
    /// <param name="line"></param>
    public void SetEffect(int line)
    {
        //���W�ݒ�
        _transform.position = _effectWidthPos + Vector3.up * line;
        //�p�[�e�B�N���J�n
        _particleSystem.Play();
    }
    #endregion
}
