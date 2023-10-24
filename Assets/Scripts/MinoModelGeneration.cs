// -------------------------------------------------------------------------------
// MinoModelGeneration.cs
//
// 作成日: 2023/10/23
// 作成者: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinoModelGeneration : MonoBehaviour,IMinoCreatable
{
    #region 変数
    private Vector3 _createStartPos = default; //ミノスタート位置
    private Color _unionColor = default; //ミノ色
    private Transform _transform = default; //自身のTransform設定

    private IMinoCreatable.MinoType _myModel = default; //ミノ形
    private IMinoBlockAccessible[] _minos = new IMinoBlockAccessible[IMinoCreatable.MAX_MINO_CNT]; //ミノブロック管理リスト
    #endregion

    #region プロパティ
    public Transform MyTransform { get => _transform; }
    public IMinoCreatable.MinoType MyModel { get => _myModel; }
    public IMinoBlockAccessible[] Minos { get => _minos; }
    #endregion

    #region メソッド
    // インターフェイス継承
    public virtual void CreateMinoUnit(IMinoBlockAccessible[] minoBlocks, IMinoCreatable.MinoType setModel)
    {
        //初期化設定を行っていない
        if(_transform == default)
        {
            _transform = transform; //自身のTransform設定
            _createStartPos = _transform.position; //初期位置設定
        }

        //位置、角度初期化
        _transform.position = _createStartPos;
        _transform.eulerAngles = Vector3.zero;

        //ミノブロックを設定
        _minos = new List<IMinoBlockAccessible>(minoBlocks).ToArray();
        _myModel = setModel;

        Debug.Log(_transform.name + "|" + _minos.Length);

        //指定されたモデルに応じて、ミノブロックの位置と色を設定
        switch (_myModel)
        {
            case IMinoCreatable.MinoType.minoO: //Oミノ
                _minos[0].SetMinoPos(IMinoCreatable.EXCEPTION_SHIFT_0_5, IMinoCreatable.EXCEPTION_SHIFT_0_5, _transform);
                _minos[1].SetMinoPos(-IMinoCreatable.EXCEPTION_SHIFT_0_5, IMinoCreatable.EXCEPTION_SHIFT_0_5, _transform);
                _minos[2].SetMinoPos(-IMinoCreatable.EXCEPTION_SHIFT_0_5, -IMinoCreatable.EXCEPTION_SHIFT_0_5, _transform);
                _minos[3].SetMinoPos(IMinoCreatable.EXCEPTION_SHIFT_0_5, -IMinoCreatable.EXCEPTION_SHIFT_0_5, _transform);
                _unionColor = Color.yellow; //黄色
                break;
            case IMinoCreatable.MinoType.minoS: //Sミノ
                _minos[0].SetMinoPos(0, 0, _transform);
                _minos[1].SetMinoPos(-IMinoCreatable.EXCEPTION_SHIFT_1_0, 0, _transform);
                _minos[2].SetMinoPos(0, IMinoCreatable.EXCEPTION_SHIFT_1_0, _transform);
                _minos[3].SetMinoPos(IMinoCreatable.EXCEPTION_SHIFT_1_0, IMinoCreatable.EXCEPTION_SHIFT_1_0, _transform);
                _unionColor = Color.green; //緑色
                break;
            case IMinoCreatable.MinoType.minoZ: //Zミノ
                _minos[0].SetMinoPos(0, 0, _transform);
                _minos[1].SetMinoPos(IMinoCreatable.EXCEPTION_SHIFT_1_0, 0, _transform);
                _minos[2].SetMinoPos(0, IMinoCreatable.EXCEPTION_SHIFT_1_0, _transform);
                _minos[3].SetMinoPos(-IMinoCreatable.EXCEPTION_SHIFT_1_0, IMinoCreatable.EXCEPTION_SHIFT_1_0, _transform);
                _unionColor = Color.red; //赤色
                break;
            case IMinoCreatable.MinoType.minoJ: //Jミノ
                _minos[0].SetMinoPos(0, 0, _transform);
                _minos[1].SetMinoPos(IMinoCreatable.EXCEPTION_SHIFT_1_0, 0, _transform);
                _minos[2].SetMinoPos(-IMinoCreatable.EXCEPTION_SHIFT_1_0, 0, _transform);
                _minos[3].SetMinoPos(-IMinoCreatable.EXCEPTION_SHIFT_1_0, IMinoCreatable.EXCEPTION_SHIFT_1_0, _transform);
                _unionColor = Color.blue; //青色
                break;
            case IMinoCreatable.MinoType.minoL: //Lミノ
                _minos[0].SetMinoPos(0, 0, _transform);
                _minos[1].SetMinoPos(-IMinoCreatable.EXCEPTION_SHIFT_1_0, 0, _transform);
                _minos[2].SetMinoPos(IMinoCreatable.EXCEPTION_SHIFT_1_0, 0, _transform);
                _minos[3].SetMinoPos(IMinoCreatable.EXCEPTION_SHIFT_1_0, IMinoCreatable.EXCEPTION_SHIFT_1_0, _transform);
                _unionColor = Color.red + Color.green / 2; //橙色
                break;
            case IMinoCreatable.MinoType.minoT: //Tミノ
                _minos[0].SetMinoPos(0, 0, _transform);
                _minos[1].SetMinoPos(-IMinoCreatable.EXCEPTION_SHIFT_1_0, 0, _transform);
                _minos[2].SetMinoPos(IMinoCreatable.EXCEPTION_SHIFT_1_0, 0, _transform);
                _minos[3].SetMinoPos(0, IMinoCreatable.EXCEPTION_SHIFT_1_0, _transform);
                _unionColor = Color.red + Color.blue; //紫色
                break;
            case IMinoCreatable.MinoType.minoI: //Iミノ
                _minos[0].SetMinoPos(IMinoCreatable.EXCEPTION_SHIFT_0_5, IMinoCreatable.EXCEPTION_SHIFT_0_5, _transform);
                _minos[1].SetMinoPos(IMinoCreatable.EXCEPTION_SHIFT_1_5, IMinoCreatable.EXCEPTION_SHIFT_0_5, _transform);
                _minos[2].SetMinoPos(-IMinoCreatable.EXCEPTION_SHIFT_0_5, IMinoCreatable.EXCEPTION_SHIFT_0_5, _transform);
                _minos[3].SetMinoPos(-IMinoCreatable.EXCEPTION_SHIFT_1_5, IMinoCreatable.EXCEPTION_SHIFT_0_5, _transform);
                _unionColor = Color.blue + Color.white / 2; //水色
                break;
            default:
                //例外処理
                break;
        }

        //色変更
        foreach (IMinoBlockAccessible mino in _minos)
        {
            mino.ChangeColor(_unionColor);
        }
    }
    #endregion
}
