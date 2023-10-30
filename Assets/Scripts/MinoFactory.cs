// -------------------------------------------------------------------------------
// MinoFactory.cs
//
// �쐬��: 2023/10/18
// �쐬��: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MinoPoolManager))]
public class MinoFactory : MonoBehaviour
{
    #region �ϐ�
    private IMinoCreatable _minoCreator = default; //�~�m�̌`�����C���^�[�t�F�C�X
    private MinoPoolManager _minoManager = default; //�~�m�u���b�N�̃I�u�W�F�N�g�v�[��

    //�~�m�`���X�g�����������郊�X�g
    private IMinoCreatable.MinoType[] _InitializeModels =
    {
        IMinoCreatable.MinoType.minoO,
        IMinoCreatable.MinoType.minoS,
        IMinoCreatable.MinoType.minoZ,
        IMinoCreatable.MinoType.minoJ,
        IMinoCreatable.MinoType.minoL,
        IMinoCreatable.MinoType.minoT,
        IMinoCreatable.MinoType.minoI
    };
    //�����\�ȃ~�m�`��ۑ����郊�X�g
    private List<IMinoCreatable.MinoType> _canCreateModels = new();
    private int _createModelIndex = 0; //��������~�m�`�̃C���f�b�N�X

    [SerializeField]
    private bool _onDebug = false;
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
        _minoCreator = FindObjectOfType<NextMino>().GetComponent<IMinoCreatable>();
        _minoManager = GetComponent<MinoPoolManager>();
        _canCreateModels.Clear();
        _canCreateModels.AddRange(_InitializeModels);
    }

    /// <summary>
    /// <para>CreateMino</para>
    /// <para>�~�m�𐶐����܂�</para>
    /// </summary>
    public void CreateMino()
    {
        if (_onDebug) { DebugView(); } //�f�o�b�O�\��
        //�����\�ȃ~�m�`�����݂��Ȃ�
        if(_canCreateModels.Count == 0)
        {
            //���X�g������
            _canCreateModels.Clear();
            _canCreateModels.AddRange(_InitializeModels);
        }
        //�����\�ȃ~�m�`��Index��ݒ�
        _createModelIndex = Random.Range(0, _canCreateModels.Count);
        //�~�m�����@�����F�g�p�\�ȃ~�m�u���b�N,��������~�m�`
        _minoCreator.CreateMinoUnit(_minoManager.GetUseableMinos(), _canCreateModels[_createModelIndex]);
        //���������~�m�`�����X�g���珜�O
        _canCreateModels.RemoveAt(_createModelIndex);
    }

    /// <summary>
    /// <para>DebugView</para>
    /// <para>�f�o�b�O�\��</para>
    /// </summary>
    void DebugView()
    {
        Debug.Log("----------");
        for (int i = 0; i < _canCreateModels.Count; i++)
        {
            Debug.Log(_canCreateModels[i]);
        }
    }
    #endregion
}
