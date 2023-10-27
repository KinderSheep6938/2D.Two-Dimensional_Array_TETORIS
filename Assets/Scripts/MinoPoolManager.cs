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
    private const int MAX_USEMINO_CNT = 4; //�ő�g�p�~�m�u���b�N��
    
    [SerializeField,Header("�~�m�u���b�N")]
    private GameObject _minoBlock = default;
    [SerializeField, Header("���O�����~�m��")]
    private int _poolValue = 150;
    private List<IMinoBlockAccessible> _minoPool = new List<IMinoBlockAccessible>(); //�v�[��
    private IMinoBlockAccessible[] _useableMinos = new IMinoBlockAccessible[MAX_USEMINO_CNT]; //�g�p�\�~�m�u���b�N
    #endregion

    #region ���\�b�h
    /// <summary>
    /// ����������
    /// </summary>
    void Awake()
    {
        //������
        //��\���Ő�������
        _minoBlock.GetComponent<IMinoBlockAccessible>().SetMinoView(false);
        //�v�[��������
        _minoPool.Clear();
        //�w�萔�܂Ńv�[���𒙂߂�
        while(_minoPool.Count < _poolValue)
        {
            //�������v�[���ݒ�
            _minoPool.Add(Instantiate(_minoBlock).GetComponent<IMinoBlockAccessible>());
        }

        //�\����Ԃɐݒ肷��
        _minoBlock.GetComponent<IMinoBlockAccessible>().SetMinoView(true);
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
        for(int i = 0; i < MAX_USEMINO_CNT; i++)
        {
            //�v�[���Ɏg�p�\�~�m�����邩
            if(0 < _minoPool.Count)
            {
                //�\������
                _minoPool[0].SetMinoView(true);
                //�v�[������擾
                _useableMinos[i] = _minoPool[0];
                _minoPool.RemoveAt(0);
            }
            else
            {   //�Ȃ��ꍇ�͐V������������
                _useableMinos[i] = Instantiate(_minoBlock).GetComponent<IMinoBlockAccessible>();
            }

        }
        //�g�p�\�ȃ~�m�u���b�N�𑗐M
        return _useableMinos;
    }

    /// <summary>
    /// <para>EndUseableMino</para>
    /// <para>ObjectPool�Ɏg�p�I�������~�m�u���b�N��ԋp���܂�</para>
    /// </summary>
    /// <param name="unUseableMino">�g�p�I�������~�m�u���b�N</param>
    public void EndUseableMino(IMinoBlockAccessible unUseableMino)
    {
        //��\���ɐݒ�
        unUseableMino.SetMinoView(false);
        //�v�[���ɓo�^
        _minoPool.Add(unUseableMino);

        return;
    }
    #endregion
}
