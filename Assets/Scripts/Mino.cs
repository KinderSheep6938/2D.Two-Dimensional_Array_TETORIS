// -------------------------------------------------------------------------------
// Mino.cs
//
// �쐬��: 2023/10/17
// �쐬��: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mino : MonoBehaviour, IMinoInfo, ILineMinoCtrl
{
    #region �ϐ�
    private int _deleteLineIndex = default;

    private MinoPoolManager minoManager = default;
    private SpriteRenderer _myRen = default;
    private Transform _myTrans = default;
    #endregion

    #region �v���p�e�B
    public int MinoX { get => Mathf.RoundToInt(_myTrans.position.x); }
    public int MinoY { get => Mathf.RoundToInt(_myTrans.position.y); }
    
    #endregion

    #region ���\�b�h
    /// <summary>
    /// ����������
    /// </summary>
    void Awake()
    {
        //������
        minoManager = FindObjectOfType<MinoPoolManager>().GetComponent<MinoPoolManager>();
        _myTrans = transform;
        _myRen = GetComponent<SpriteRenderer>();
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
        //����������
        if(_myTrans.eulerAngles.z != 0) { _myTrans.eulerAngles = Vector3.zero; }
    }

    //�C���^�[�t�F�C�X�p��
    public void ChangeColor(Color minoColor)
    {
        _myRen.color = minoColor;
        return;
    }

    //�C���^�[�t�F�C�X�p��
    public void SetMinoPos(float x, float y, Transform parent)
    {
        //���𒆐S�ɍ��W����
        _myTrans.position = parent.position + Vector3.right * x + Vector3.up * y;
        //����e�ɐݒ�
        _myTrans.parent = parent;
    }

    //�C���^�[�t�F�C�X�p��
    public void DisConnect()
    {
        //�e�q�֌W�폜
        _myTrans.parent = null;
        //���W���K��
        _myTrans.position =
            Vector3.right * Mathf.RoundToInt(_myTrans.position.x) +
            Vector3.up * Mathf.RoundToInt(_myTrans.position.y);
    }

    //�C���^�[�t�F�C�X�p��
    public void LineCtrl(List<int> deleteLineHeights)
    {
        //�z�[���h���̃~�m�͖�������
        if (_myTrans.parent != null) { return; }

        //�폜�Ώۂ̃��C���ɂ���ꍇ�A�폜����
        if (deleteLineHeights.Contains(MinoY)) { DeleteMino(); }

        //�폜�Ώۂ̃��C���ɉ����āA�����������s���܂�
        for (_deleteLineIndex = 0; _deleteLineIndex < deleteLineHeights.Count; _deleteLineIndex++)
        {
            //���݂̗����Ώۃ��C���ȉ� ���� �P�ԉ��w�̃��C���ł���
            if(MinoY <= deleteLineHeights[_deleteLineIndex] && _deleteLineIndex == 0)
            {
                //�������Ȃ�
                return;
            }

            //���݂̗����Ώۃ��C���ȉ��ł���i�P�ԉ��w�̃��C���ł͂Ȃ��j
            if(MinoY <= deleteLineHeights[_deleteLineIndex])
            {
                DownMino(_deleteLineIndex);
                return;
            }

            //���݂̗����Ώۃ��C������
            if(deleteLineHeights[_deleteLineIndex] < MinoY)
            {
                //���̗����Ώۃ��C���Ɣ�r
                continue;
            }
        }
        //�S�Ă̗����Ώۃ��C������ɂ���
        DownMino(_deleteLineIndex);
        return;
    }

    /// <summary>
    /// <para>DownMino</para>
    /// <para>�~�m�𗎉������܂�</para>
    /// </summary>
    /// <param name="value">��������</param>
    private void DownMino(int value)
    {
        _myTrans.position += Vector3.down * value;
        return;
    }

    /// <summary>
    /// <para>DeleteMino</para>
    /// <para>�~�m���폜���܂�</para>
    /// </summary>
    private void DeleteMino()
    {
        minoManager.EndUseableMino(gameObject);
        return;
    }
    #endregion
}
