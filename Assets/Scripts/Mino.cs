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
    private bool _isCommit = false;

    private SpriteRenderer _myRen = default;
    private Transform _myTrans = default;
    #endregion

    #region �v���p�e�B
    public int MinoX { get => (int)_myTrans.position.x; }
    public int MinoY { get => (int)_myTrans.position.y; }
    
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
    /// <para>ChangeColor</para>
    /// <para>�~�m�̐F��ݒ肵�܂�</para>
    /// </summary>
    /// <param name="minoColor">�ݒ肳���F</param>
    public void ChangeColor(Color minoColor)
    {
        _myRen.color = minoColor;
        return;
    }

    /// <summary>
    /// <para>SetMinoPos</para>
    /// <para>�~�m�u���b�N���w�肵���l���ړ����܂�</para>
    /// <para>�܂��A�w�肵��Transform��e�i�~�m���j�Ƃ��Đݒ肵�܂�</para>
    /// </summary>
    /// <param name="x">�ړ����鉡��</param>
    /// <param name="y">�ړ�����c��</param>
    /// <param name="parent">�~�m��</param>
    public void SetMinoPos(float x, float y, Transform parent)
    {
        //���𒆐S�ɍ��W����
        _myTrans.position = parent.position + Vector3.right * x + Vector3.up * y;
        //����e�ɐݒ�
        _myTrans.parent = parent;
    }

    /// <summary>
    /// <para>DownMino</para>
    /// <para>�~�m��1�񕪉����܂�</para>
    /// </summary>
    public void DownMino()
    {
        _myTrans.position += Vector3.down;
        return;
    }

    /// <summary>
    /// <para>DeleteMino</para>
    /// <para>�~�m���폜���܂�</para>
    /// </summary>
    public void DeleteMino()
    {
        //�~�m�폜����
        return;
    }
    #endregion
}
