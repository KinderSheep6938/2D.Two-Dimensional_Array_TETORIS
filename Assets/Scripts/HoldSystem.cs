// -------------------------------------------------------------------------------
// HoldSystem.cs
//
// 作成日: 2023/10/22
// 作成者: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldSystem : MonoBehaviour, IMinoHoldable, IMinoCreatable
{
    #region 変数
    const float EXCEPTION_MINO_0_5_SHIFT = 0.5f; //ミノ形生成用0.5差分
    const float EXCEPTION_MINO_1_0_SHIFT = 1.0f; //ミノ形生成用1.0差分
    const float EXCEPTION_MINO_1_5_SHIFT = 1.5f; //ミノ形生成用1.5差分
    const string PLAYABLE_MINOCTRL_TAG = "PlayableMino";

    private IMinoCreatable.MinoType _myModel = default; //ミノ形

    private List<IMinoInfo> _minos = new();
    private Vector3 _createStartPos = default;
    private Color _unionColor = default;

    private Transform _myTrans = default;
    private IMinoCreatable _playerMino = default;
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
        //初期化
        _minos.Clear();
        _minos.AddRange(GetComponentsInChildren<IMinoInfo>());

        _playerMino = GameObject.FindGameObjectWithTag(PLAYABLE_MINOCTRL_TAG).GetComponent<IMinoCreatable>();
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

    // インターフェイス継承
    public void CreateMinoUnit(IMinoInfo[] minoBlocks, IMinoCreatable.MinoType setModel)
    {
        //位置、角度初期化
        _myTrans.position = _createStartPos;

        //指定されたモデルに応じて、ミノブロックの位置と色を設定
        switch (setModel)
        {
            case IMinoCreatable.MinoType.minoO: //Oミノ
                _minos[0].SetMinoPos(EXCEPTION_MINO_0_5_SHIFT, EXCEPTION_MINO_0_5_SHIFT, _myTrans);
                _minos[1].SetMinoPos(-EXCEPTION_MINO_0_5_SHIFT, EXCEPTION_MINO_0_5_SHIFT, _myTrans);
                _minos[2].SetMinoPos(-EXCEPTION_MINO_0_5_SHIFT, -EXCEPTION_MINO_0_5_SHIFT, _myTrans);
                _minos[3].SetMinoPos(EXCEPTION_MINO_0_5_SHIFT, -EXCEPTION_MINO_0_5_SHIFT, _myTrans);
                _unionColor = Color.yellow; //黄色
                _myTrans.position += Vector3.right * EXCEPTION_MINO_0_5_SHIFT + Vector3.up * EXCEPTION_MINO_0_5_SHIFT; //Oミノの初期設定
                break;
            case IMinoCreatable.MinoType.minoS: //Sミノ
                _minos[0].SetMinoPos(0, 0, _myTrans);
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
                _unionColor = Color.red + Color.green / 2; //橙色
                break;
            case IMinoCreatable.MinoType.minoT: //Tミノ
                _minos[0].SetMinoPos(0, 0, _myTrans);
                _minos[1].SetMinoPos(-EXCEPTION_MINO_1_0_SHIFT, 0, _myTrans);
                _minos[2].SetMinoPos(EXCEPTION_MINO_1_0_SHIFT, 0, _myTrans);
                _minos[3].SetMinoPos(0, EXCEPTION_MINO_1_0_SHIFT, _myTrans);
                _unionColor = Color.red + Color.blue; //紫色
                break;
            case IMinoCreatable.MinoType.minoI: //Iミノ
                _minos[0].SetMinoPos(EXCEPTION_MINO_0_5_SHIFT, EXCEPTION_MINO_0_5_SHIFT, _myTrans);
                _minos[1].SetMinoPos(EXCEPTION_MINO_1_5_SHIFT, EXCEPTION_MINO_0_5_SHIFT, _myTrans);
                _minos[2].SetMinoPos(-EXCEPTION_MINO_0_5_SHIFT, EXCEPTION_MINO_0_5_SHIFT, _myTrans);
                _minos[3].SetMinoPos(-EXCEPTION_MINO_1_5_SHIFT, EXCEPTION_MINO_0_5_SHIFT, _myTrans);
                _unionColor = Color.blue + Color.white / 2; //水色
                _myTrans.position += Vector3.right * EXCEPTION_MINO_0_5_SHIFT + Vector3.down * EXCEPTION_MINO_0_5_SHIFT; //Iミノの初期設定
                break;
            default:
                //例外処理
                break;
        }

        //色変更
        foreach (IMinoInfo mino in _minos)
        {
            mino.ChangeColor(_unionColor);
        }
    }

    //インターフェイス継承
    public void Hold()
    {

    }
    #endregion
}
