// -------------------------------------------------------------------------------
// MinoUnionParent.cs
//
// 作成日: 2023/10/18
// 作成者: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinoUnionParent : MonoBehaviour, IMinoCreatable
{
    #region 変数
    const float EXCEPTION_MINO_0_5_SHIFT = 0.5f;
    const float EXCEPTION_MINO_1_0_SHIFT = 1.0f;
    const float EXCEPTION_MINO_1_5_SHIFT = 1.5f;

    private IMinoInfo[] _minos = default;
    private float _timer = 0;
    private Color _unionColor = default;
    private IMinoCreatable.MinoType _myModel = default; //ミノ形
    private Transform _myTrans = default;
    #endregion

    #region プロパティ
    public IMinoCreatable.MinoType MyModel { get => _myModel; set => _myModel = value; }
    #endregion

    #region メソッド
    /// <summary>
    /// 初期化処理
    /// </summary>
    void Awake()
    {
        _myTrans = transform;
    }

    /// <summary>
    /// 更新前処理
    /// </summary>
    void Start()
    {

    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {

    }

    /// <summary>
    /// <para>CreateMinoUnit</para>
    /// <para>指定されたミノ形にミノブロックを設定します</para>
    /// </summary>
    /// <param name="setModel">変形するミノ形</param>
    public void CreateMinoUnit(IMinoInfo[] minoBlocks,IMinoCreatable.MinoType setModel)
    {
        //ミノブロックを設定
        _minos = minoBlocks;
        //指定されたモデルに応じて、ミノブロックの位置と色を設定
        switch (setModel)
        {
            case IMinoCreatable.MinoType.minoO: //Oミノ
                _minos[0].SetMinoPos(EXCEPTION_MINO_0_5_SHIFT, EXCEPTION_MINO_0_5_SHIFT, _myTrans);
                _minos[1].SetMinoPos(-EXCEPTION_MINO_0_5_SHIFT, EXCEPTION_MINO_0_5_SHIFT, _myTrans);
                _minos[2].SetMinoPos(-EXCEPTION_MINO_0_5_SHIFT, -EXCEPTION_MINO_0_5_SHIFT, _myTrans);
                _minos[3].SetMinoPos(EXCEPTION_MINO_0_5_SHIFT, -EXCEPTION_MINO_0_5_SHIFT, _myTrans);
                _unionColor = Color.yellow; //黄色
                break;
            case IMinoCreatable.MinoType.minoS: //Sミノ
                _minos[0].SetMinoPos(0,0, _myTrans);
                _minos[1].SetMinoPos(-EXCEPTION_MINO_1_0_SHIFT, 0, _myTrans);
                _minos[2].SetMinoPos(0, EXCEPTION_MINO_1_0_SHIFT, _myTrans);
                _minos[3].SetMinoPos(EXCEPTION_MINO_1_0_SHIFT, EXCEPTION_MINO_1_0_SHIFT, _myTrans);
                _unionColor = Color.green; //緑色
                break;
            case IMinoCreatable.MinoType.minoZ: //Zミノ
                _minos[0].SetMinoPos(0, 0, _myTrans);
                _minos[1].SetMinoPos(EXCEPTION_MINO_1_0_SHIFT, 0, _myTrans);
                _minos[2].SetMinoPos(0, EXCEPTION_MINO_1_0_SHIFT, _myTrans);
                _minos[3].SetMinoPos(-EXCEPTION_MINO_1_0_SHIFT, EXCEPTION_MINO_1_0_SHIFT, _myTrans);
                _unionColor = Color.red; //赤色
                break;
            case IMinoCreatable.MinoType.minoJ: //Jミノ
                _minos[0].SetMinoPos(0, 0, _myTrans);
                _minos[1].SetMinoPos(EXCEPTION_MINO_1_0_SHIFT, 0, _myTrans);
                _minos[2].SetMinoPos(-EXCEPTION_MINO_1_0_SHIFT, 0, _myTrans);
                _minos[3].SetMinoPos(-EXCEPTION_MINO_1_0_SHIFT, EXCEPTION_MINO_1_0_SHIFT, _myTrans);
                _unionColor = Color.blue; //青色
                break;
            case IMinoCreatable.MinoType.minoL: //Lミノ
                _minos[0].SetMinoPos(0, 0, _myTrans);
                _minos[1].SetMinoPos(-EXCEPTION_MINO_1_0_SHIFT, 0, _myTrans);
                _minos[2].SetMinoPos(EXCEPTION_MINO_1_0_SHIFT, 0, _myTrans);
                _minos[3].SetMinoPos(EXCEPTION_MINO_1_0_SHIFT, EXCEPTION_MINO_1_0_SHIFT, _myTrans);
                _unionColor = Color.red * Color.yellow; //橙色
                break;
            case IMinoCreatable.MinoType.minoT: //Tミノ
                _minos[0].SetMinoPos(0, 0, _myTrans);
                _minos[1].SetMinoPos(-EXCEPTION_MINO_1_0_SHIFT, 0, _myTrans);
                _minos[2].SetMinoPos(EXCEPTION_MINO_1_0_SHIFT, 0, _myTrans);
                _minos[3].SetMinoPos(0, EXCEPTION_MINO_1_0_SHIFT, _myTrans);
                _unionColor = Color.red * Color.blue; //紫色
                break;
            case IMinoCreatable.MinoType.minoI: //Iミノ
                _minos[0].SetMinoPos(EXCEPTION_MINO_0_5_SHIFT, EXCEPTION_MINO_0_5_SHIFT, _myTrans);
                _minos[1].SetMinoPos(EXCEPTION_MINO_1_5_SHIFT, EXCEPTION_MINO_0_5_SHIFT, _myTrans);
                _minos[2].SetMinoPos(-EXCEPTION_MINO_0_5_SHIFT, EXCEPTION_MINO_0_5_SHIFT, _myTrans);
                _minos[3].SetMinoPos(-EXCEPTION_MINO_1_5_SHIFT, EXCEPTION_MINO_0_5_SHIFT, _myTrans);
                _unionColor = Color.blue * Color.white; //水色
                break;
            default: 
                //例外処理
                break;
        }

        //色変更
        foreach(IMinoInfo mino in _minos)
        {
            mino.ChangeColor(_unionColor);
        }
    }

    /// <summary>
    /// <para>SetUnionPlayable</para>
    /// <para>ミノユニットを操作可能にします</para>
    /// </summary>
    public void SetUnionPlayable()
    {

    }
    #endregion
}
