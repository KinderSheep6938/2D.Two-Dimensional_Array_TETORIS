// -------------------------------------------------------------------------------
// Mino.cs
//
// �쐬��: 2023/10/17
// �쐬��: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mino : MonoBehaviour, IMinoBlockAccessible, ILineMinoCtrl
{
    #region �ϐ�
    private int _deleteLineCnt = default; //�폜���C���J�E���g

    private MinoPoolManager minoManager = default; //�~�m�Ǘ��}�l�[�W���[
    private SpriteRenderer _myRen = default; //���g��SpriteRenderer
    private Transform _transform = default; //���g��Transform
    private IMinoBlockAccessible _myBlock = default; //���g�̃u���b�N���
    #endregion

    #region �v���p�e�B
    public int MinoX { get => Mathf.RoundToInt(_transform.position.x); }
    public int MinoY { get => Mathf.RoundToInt(_transform.position.y); }
    #endregion

    #region ���\�b�h
    /// <summary>
    /// ����������
    /// </summary>
    void Awake()
    {
        //������
        minoManager = FindObjectOfType<MinoPoolManager>().GetComponent<MinoPoolManager>();
        _transform = transform;
        _myRen = GetComponent<SpriteRenderer>();
        _myBlock = GetComponent<IMinoBlockAccessible>();
    }

    /// <summary>
    /// �X�V����
    /// </summary>
    void Update()
    {
        //����������
        if(_transform.eulerAngles.z != 0) { _transform.eulerAngles = Vector3.zero; }
    }

    //�C���^�[�t�F�C�X�p��
    public void ChangeColor(Color minoColor)
    {
        //�F�ύX
        _myRen.color = minoColor;
        return;
    }

    //�C���^�[�t�F�C�X�p��
    public void SetMinoPos(float x, float y, Transform parent)
    {
        //���𒆐S�ɍ��W����
        _transform.position = parent.position + Vector3.right * x + Vector3.up * y;
        //����e�ɐݒ�
        _transform.parent = parent;
        //�\��
        _myRen.enabled = true;
    }

    //�C���^�[�t�F�C�X�p��
    public void DisConnect()
    {
        //�e�q�֌W�폜
        _transform.parent = null;
        //���W���K��
        _transform.position =
            Vector3.right * Mathf.RoundToInt(_transform.position.x) +
            Vector3.up * Mathf.RoundToInt(_transform.position.y);
    }
    
    //�C���^�[�t�F�C�X�p��
    public void SetMinoView(bool canShow)
    {
        //�\��
        if (canShow)
        {
            gameObject.SetActive(true);
        }
        else
        { //��\��
            gameObject.SetActive(false);
        }
    }

    //�C���^�[�t�F�C�X�p��
    public void LineCtrl(List<int> deleteLineHeights)
    {
        //�z�[���h���̃~�m �܂��� ��\���̃~�m �͖�������
        if (_transform.parent != null || !_myRen.enabled ) { return; }

        //�폜�Ώۂ̃��C���ɂ���ꍇ�A�폜����
        if (deleteLineHeights.Contains(MinoY)) 
        {
            DeleteMino();
            return;
        }

        //�폜�Ώۂ̃��C���ɉ����āA�����������s���܂�
        for (_deleteLineCnt = 0; _deleteLineCnt < deleteLineHeights.Count; _deleteLineCnt++)
        {
            //���݂̗����Ώۃ��C���ȉ� ���� �P�ԉ��w�̃��C���ł���
            if(MinoY <= deleteLineHeights[_deleteLineCnt] && _deleteLineCnt == 0)
            {
                //�������Ȃ�
                return;
            }

            //���݂̗����Ώۃ��C���ȉ��ł���i�P�ԉ��w�̃��C���ł͂Ȃ��j
            if(MinoY <= deleteLineHeights[_deleteLineCnt])
            {
                DownMino(_deleteLineCnt);
                return;
            }

            //���݂̗����Ώۃ��C������
            if(deleteLineHeights[_deleteLineCnt] < MinoY)
            {
                //���̗����Ώۃ��C���Ɣ�r
                continue;
            }
        }
        //�S�Ă̗����Ώۃ��C������ɂ���
        DownMino(_deleteLineCnt);
        return;
    }

    /// <summary>
    /// <para>DownMino</para>
    /// <para>�~�m�𗎉������܂�</para>
    /// </summary>
    /// <param name="value">��������</param>
    private void DownMino(int value)
    {
        _transform.position += Vector3.down * value;
        return;
    }

    /// <summary>
    /// <para>DeleteMino</para>
    /// <para>�~�m���폜���܂�</para>
    /// </summary>
    private void DeleteMino()
    {
        _myRen.enabled = false;
        minoManager.EndUseableMino(_myBlock);
        return;
    }
    #endregion
}
