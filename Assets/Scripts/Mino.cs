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
    const string OBJECTPOOL_SYSTEM_TAG = "ObjectPool";

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
        minoManager = GameObject.FindGameObjectWithTag(OBJECTPOOL_SYSTEM_TAG).GetComponent<MinoPoolManager>();
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
    public void DownMino()
    {
        _myTrans.position += Vector3.down;
        return;
    }

    //�C���^�[�t�F�C�X�p��
    public void DeleteMino()
    {
        minoManager.EndUseableMino(gameObject);
        return;
    }
    #endregion
}
