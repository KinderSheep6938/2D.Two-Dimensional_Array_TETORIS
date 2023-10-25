// -------------------------------------------------------------------------------
// MinoPoolManager.cs
//
// �쐬��: 2023/10/19
// �쐬��: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinoPoolManager : MonoBehaviour
{
    #region �ϐ�
    private const int MAX_MINO_CNT = 4;
    

    [SerializeField,Tooltip("�~�m�u���b�N")]
    private GameObject _minoBlock = default;
    private IMinoBlockAccessible[] _useableMinos = new IMinoBlockAccessible[MAX_MINO_CNT];
    #endregion

    #region �v���p�e�B

    #endregion

    #region ���\�b�h
    /// <summary>
    /// ����������
    /// </summary>
    void Awake()
    {

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

    }

    /// <summary>
    /// <para>GetUseableMino</para>
    /// <para>ObjectPool����g�p�\�ȃ~�m�u���b�N���擾���܂�</para>
    /// </summary>
    /// <returns>�g�p�\�ȃ~�m�u���b�N</returns>
    public IMinoBlockAccessible[] GetUseableMino()
    {
        //���݂�Instantiate

        //�g�p�\�ȃ~�m�u���b�N���擾
        for(int i = 0; i < MAX_MINO_CNT; i++)
        {
            _useableMinos[i] = Instantiate(_minoBlock).GetComponent<IMinoBlockAccessible>();
        }
        //�g�p�\�ȃ~�m�u���b�N�𑗐M
        return _useableMinos;
    }

    /// <summary>
    /// <para>EndUseableMino</para>
    /// <para>ObjectPool�Ɏg�p�I�������~�m�u���b�N��ԋp���܂�</para>
    /// </summary>
    /// <param name="unUseableMino">�g�p�I�������~�m�u���b�N</param>
    public void EndUseableMino(GameObject unUseableMino)
    {
        //���݂�Destroy

        //�I�u�W�F�N�g�j��
        Destroy(unUseableMino);
        return;
    }
    #endregion
}
