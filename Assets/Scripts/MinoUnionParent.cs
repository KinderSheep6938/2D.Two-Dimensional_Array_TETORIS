// -------------------------------------------------------------------------------
// MinoUnionParent.cs
//
// �쐬��: 2023/10/18
// �쐬��: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinoUnionParent : MonoBehaviour, IMinoCreatable
{
    #region �ϐ�
    const float EXCEPTION_MINO_0_5_SHIFT = 0.5f;
    const float EXCEPTION_MINO_1_0_SHIFT = 1.0f;
    const float EXCEPTION_MINO_1_5_SHIFT = 1.5f;

    private IMinoInfo[] _minos = default;
    private float _timer = 0;
    private Color _unionColor = default;
    private IMinoCreatable.MinoType _myModel = default; //�~�m�`
    private Transform _myTrans = default;
    #endregion

    #region �v���p�e�B
    public IMinoCreatable.MinoType MyModel { get => _myModel; set => _myModel = value; }
    #endregion

    #region ���\�b�h
    /// <summary>
    /// ����������
    /// </summary>
    void Awake()
    {
        _myTrans = transform;
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
    /// <para>CreateMinoUnit</para>
    /// <para>�w�肳�ꂽ�~�m�`�Ƀ~�m�u���b�N��ݒ肵�܂�</para>
    /// </summary>
    /// <param name="setModel">�ό`����~�m�`</param>
    public void CreateMinoUnit(IMinoInfo[] minoBlocks,IMinoCreatable.MinoType setModel)
    {
        //�~�m�u���b�N��ݒ�
        _minos = minoBlocks;
        //�w�肳�ꂽ���f���ɉ����āA�~�m�u���b�N�̈ʒu�ƐF��ݒ�
        switch (setModel)
        {
            case IMinoCreatable.MinoType.minoO: //O�~�m
                _minos[0].SetMinoPos(EXCEPTION_MINO_0_5_SHIFT, EXCEPTION_MINO_0_5_SHIFT, _myTrans);
                _minos[1].SetMinoPos(-EXCEPTION_MINO_0_5_SHIFT, EXCEPTION_MINO_0_5_SHIFT, _myTrans);
                _minos[2].SetMinoPos(-EXCEPTION_MINO_0_5_SHIFT, -EXCEPTION_MINO_0_5_SHIFT, _myTrans);
                _minos[3].SetMinoPos(EXCEPTION_MINO_0_5_SHIFT, -EXCEPTION_MINO_0_5_SHIFT, _myTrans);
                _unionColor = Color.yellow; //���F
                break;
            case IMinoCreatable.MinoType.minoS: //S�~�m
                _minos[0].SetMinoPos(0,0, _myTrans);
                _minos[1].SetMinoPos(-EXCEPTION_MINO_1_0_SHIFT, 0, _myTrans);
                _minos[2].SetMinoPos(0, EXCEPTION_MINO_1_0_SHIFT, _myTrans);
                _minos[3].SetMinoPos(EXCEPTION_MINO_1_0_SHIFT, EXCEPTION_MINO_1_0_SHIFT, _myTrans);
                _unionColor = Color.green; //�ΐF
                break;
            case IMinoCreatable.MinoType.minoZ: //Z�~�m
                _minos[0].SetMinoPos(0, 0, _myTrans);
                _minos[1].SetMinoPos(EXCEPTION_MINO_1_0_SHIFT, 0, _myTrans);
                _minos[2].SetMinoPos(0, EXCEPTION_MINO_1_0_SHIFT, _myTrans);
                _minos[3].SetMinoPos(-EXCEPTION_MINO_1_0_SHIFT, EXCEPTION_MINO_1_0_SHIFT, _myTrans);
                _unionColor = Color.red; //�ԐF
                break;
            case IMinoCreatable.MinoType.minoJ: //J�~�m
                _minos[0].SetMinoPos(0, 0, _myTrans);
                _minos[1].SetMinoPos(EXCEPTION_MINO_1_0_SHIFT, 0, _myTrans);
                _minos[2].SetMinoPos(-EXCEPTION_MINO_1_0_SHIFT, 0, _myTrans);
                _minos[3].SetMinoPos(-EXCEPTION_MINO_1_0_SHIFT, EXCEPTION_MINO_1_0_SHIFT, _myTrans);
                _unionColor = Color.blue; //�F
                break;
            case IMinoCreatable.MinoType.minoL: //L�~�m
                _minos[0].SetMinoPos(0, 0, _myTrans);
                _minos[1].SetMinoPos(-EXCEPTION_MINO_1_0_SHIFT, 0, _myTrans);
                _minos[2].SetMinoPos(EXCEPTION_MINO_1_0_SHIFT, 0, _myTrans);
                _minos[3].SetMinoPos(EXCEPTION_MINO_1_0_SHIFT, EXCEPTION_MINO_1_0_SHIFT, _myTrans);
                _unionColor = Color.red * Color.yellow; //��F
                break;
            case IMinoCreatable.MinoType.minoT: //T�~�m
                _minos[0].SetMinoPos(0, 0, _myTrans);
                _minos[1].SetMinoPos(-EXCEPTION_MINO_1_0_SHIFT, 0, _myTrans);
                _minos[2].SetMinoPos(EXCEPTION_MINO_1_0_SHIFT, 0, _myTrans);
                _minos[3].SetMinoPos(0, EXCEPTION_MINO_1_0_SHIFT, _myTrans);
                _unionColor = Color.red * Color.blue; //���F
                break;
            case IMinoCreatable.MinoType.minoI: //I�~�m
                _minos[0].SetMinoPos(EXCEPTION_MINO_0_5_SHIFT, EXCEPTION_MINO_0_5_SHIFT, _myTrans);
                _minos[1].SetMinoPos(EXCEPTION_MINO_1_5_SHIFT, EXCEPTION_MINO_0_5_SHIFT, _myTrans);
                _minos[2].SetMinoPos(-EXCEPTION_MINO_0_5_SHIFT, EXCEPTION_MINO_0_5_SHIFT, _myTrans);
                _minos[3].SetMinoPos(-EXCEPTION_MINO_1_5_SHIFT, EXCEPTION_MINO_0_5_SHIFT, _myTrans);
                _unionColor = Color.blue * Color.white; //���F
                break;
            default: 
                //��O����
                break;
        }

        //�F�ύX
        foreach(IMinoInfo mino in _minos)
        {
            mino.ChangeColor(_unionColor);
        }
    }

    /// <summary>
    /// <para>SetUnionPlayable</para>
    /// <para>�~�m���j�b�g�𑀍�\�ɂ��܂�</para>
    /// </summary>
    public void SetUnionPlayable()
    {

    }
    #endregion
}
