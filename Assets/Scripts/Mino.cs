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
    private int _minoX = 0; //X���W
    private int _minoY = 0; //Y���W
    private bool _isCommit = false;

    private SpriteRenderer _myRen = default;
    private Transform _myTrans = default;
    #endregion

    #region �v���p�e�B
    public int MinoX { get => _minoX; set => _minoX = value; }
    public int MinoY { get => _minoY; set => _minoY = value; }
    
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
