// -------------------------------------------------------------------------------
// MinoUnionParent.cs
//
// 作成日: 2023/10/18
// 作成者: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinoUnionParent : MonoBehaviour, IMinoCreatable, IMinoUnionCtrl
{
    #region 変数
    struct SRSPosSave //スパロテの座標保存データ
    {
        public Vector3 startPos;
        public Vector3 firstSRSPos;
        public Vector3 secondSRSPos;
    }
    private SRSPosSave _srsPos = new();

    const float EXCEPTION_MINO_0_5_SHIFT = 0.5f; //ミノ形生成用0.5差分
    const float EXCEPTION_MINO_1_0_SHIFT = 1.0f; //ミノ形生成用1.0差分
    const float EXCEPTION_MINO_1_5_SHIFT = 1.5f; //ミノ形生成用1.5差分
    const int ANGLE_UP_ID = 0; //上向きID
    const int ANGLE_RIGHT_ID = 1; //右向きID
    const int ANGLE_DOWN_ID = 2; //下向きID
    const int ANGLE_LEFT_ID = 3; //左向きID
    const int DIRE_RIGHT_ID = 1; //左向きID
    const int DIRE_LEFT_ID = -1; //左向きID
    const float ROTATE_VALUE = 90f; //回転処理の回転角度
    const float FALL_TIME = 0.5f; //落下時間
    
    private Vector3 _createStartPos = default; //ミノスタート位置
    private int _nowAngle = 0; //現在のミノの向き
    private int _moveDire = 0; //回転方向
    private bool _needReturn = false; //回転巻き戻し判定
    private int _srsCnt = 0; //スパロテの回数
    private List<IMinoInfo> _minos = new(); //ミノブロック管理リスト
    private float _fallTimer = 0; //落下計測タイマー
    private Color _unionColor = default; //ミノ色
    private IMinoCreatable.MinoType _myModel = default; //ミノ形

    private IFieldCtrl _fieldCtrl = default; //フィールド管理システムのインターフェイス
    private Transform _myTrans = default; //自身のTransform
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
        _myTrans = transform;
        _createStartPos = _myTrans.position;
        _fieldCtrl = FindObjectOfType<FieldManager>().GetComponent<IFieldCtrl>();
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
        if (_myTrans.childCount == 0) { return; }

        //時間経過落下
        FallMino();
    }

    // インターフェイス継承
    public void Move(int x)
    {
        //移動反映
        _myTrans.position += Vector3.right * x;
        //衝突判定があった場合は戻す
        if (CheckMino()) { _myTrans.position -= Vector3.right * x; }
    }

    // インターフェイス継承
    public void Rotate(int angle)
    {
        //回転反映
        _myTrans.eulerAngles -= Vector3.forward * ROTATE_VALUE * angle;

        _nowAngle = (int)(_myTrans.eulerAngles.z / ROTATE_VALUE); //向き取得
        _moveDire = angle; //回転方向取得
        _needReturn = false; //回転可能

        //スパロテ回数初期化
        _srsCnt = 0;
        //衝突判定があった場合はスーパーローテーションシステムを実行する
        if (CheckMino())
        {
            Debug.Log("srs");
            if (_myModel == IMinoCreatable.MinoType.minoI) { SRSByFour(); } //サイズが４ｘ４
            SRSByThree(); //サイズが３ｘ３
        }

        //スパロテができない場合は角度を戻す
        if (_needReturn)
        {
            _myTrans.eulerAngles -= Vector3.forward * ROTATE_VALUE * -angle;
            _nowAngle = (int)(_myTrans.eulerAngles.z / ROTATE_VALUE); //向き取得
        }
    }
    
    //インターフェイス継承
    public void HardDrop()
    {
        //１列落下
        _myTrans.position += Vector3.down;

        //落下先に衝突判定がある
        if (CheckMino())
        {
            //もとに戻す
            _myTrans.position += Vector3.up;
            //ミノをフィールドに設定
            foreach (IMinoInfo mino in _minos)
            {
                //コミット
                _fieldCtrl.SetMino(mino.MinoX, mino.MinoY);
                //親子関係削除
                mino.DisConnect();
            }
            //落下タイマー初期化
            _fallTimer = 0;

            return; //処理終了
        }
        else //衝突判定がない
        {
            //再起呼び出し
            HardDrop();
        }
        return;
    }

    private void FallMino()
    {
        _fallTimer += Time.deltaTime; //タイマー加算

        //落下時間になったか
        if(FALL_TIME < _fallTimer)
        {
            _fallTimer = 0;
            //ミノを１マス落下
            _myTrans.position += Vector3.down;

            //落下先に衝突判定がある
            if (CheckMino())
            {
                //もとに戻す
                _myTrans.position += Vector3.up;
                //ミノをフィールドに設定
                foreach(IMinoInfo mino in _minos)
                {
                    //コミット
                    _fieldCtrl.SetMino(mino.MinoX, mino.MinoY);
                    //親子関係削除
                    mino.DisConnect();

                }

            }
        }
    }

    /// <summary>
    /// <para>CheckMino</para>
    /// <para>ミノブロックの衝突をチェックします</para>
    /// </summary>
    /// <returns>衝突判定</returns>
    private bool CheckMino()
    {
        //ミノブロックが衝突してないか
        foreach (IMinoInfo mino in _minos)
        {
            //空白ではない
            if (_fieldCtrl.CheckAlreadyMinoExist(mino.MinoX, mino.MinoY)) { return true; }
        }
        //空白である
        return false;
    }

    /// <summary>
    /// <para>SRSByThree</para>
    /// <para>サイズが３のスーパーローテーションシステム</para>
    /// </summary>
    private void SRSByThree()
    {
        switch (_srsCnt)
        {
            case 0: //第１条件 --------------------------------------------------------------------------------------------------------------------------
                _srsPos.startPos = _myTrans.position; //初期座標保存
                if (_nowAngle == ANGLE_RIGHT_ID || (_nowAngle == ANGLE_DOWN_ID && _moveDire == DIRE_LEFT_ID) || (_nowAngle == ANGLE_UP_ID && _moveDire == DIRE_RIGHT_ID))
                {
                    _myTrans.position += Vector3.left;
                    break; //現条件終了
                }
                _myTrans.position += Vector3.right;
                break; //現条件終了

            case 1: //第２条件 --------------------------------------------------------------------------------------------------------------------------
                //座標継続
                if (_nowAngle == ANGLE_RIGHT_ID || _nowAngle == ANGLE_LEFT_ID)
                {
                    _myTrans.position += Vector3.up;
                    break; //現条件終了
                }
                _myTrans.position += Vector3.down;
                break; //現条件終了

            case 2: //第３条件 --------------------------------------------------------------------------------------------------------------------------
                _myTrans.position = _srsPos.startPos; //初期座標引き戻し
                if (_nowAngle == ANGLE_RIGHT_ID || _nowAngle == ANGLE_LEFT_ID)
                {
                    _myTrans.position += Vector3.down * 2;
                    break; //現条件終了
                }
                _myTrans.position += Vector3.up * 2;
                break; //現条件終了

            case 3: //第４条件 --------------------------------------------------------------------------------------------------------------------------
                //座標継続
                if (_nowAngle == ANGLE_RIGHT_ID || (_nowAngle == ANGLE_DOWN_ID && _moveDire == DIRE_LEFT_ID) || (_nowAngle == ANGLE_UP_ID && _moveDire == DIRE_RIGHT_ID))
                {
                    _myTrans.position += Vector3.left;
                    break; //現条件終了
                }
                _myTrans.position += Vector3.right;
                break; //現条件終了

            case 4: //回転できない ----------------------------------------------------------------------------------------------------------------------
                _myTrans.position = _srsPos.startPos; //初期座標引き戻し
                _needReturn = true;
                return; //現条件終了
        }
        _srsCnt++; //スパロテ記録
        if (CheckMino()) { SRSByThree(); } //衝突判定があった場合、スパロテ継続
        return;
    }

    /// <summary>
    /// <para>SRSByFour</para>
    /// <para>サイズが４のスーパーローテーションシステム</para>
    /// </summary>
    private void SRSByFour()
    {
        switch (_srsCnt)
        {
            case 0: //第１条件 --------------------------------------------------------------------------------------------------------------------------
                _srsPos.startPos = _myTrans.position; //初期座標保存
                if(_nowAngle == ANGLE_UP_ID)
                {
                    _myTrans.position += Vector3.left * 2 * _moveDire;
                    break; //現条件終了
                }
                if(_nowAngle == ANGLE_DOWN_ID)
                {
                    _myTrans.position += Vector3.left * _moveDire;
                    break; //現条件終了
                }
                if(_moveDire == DIRE_RIGHT_ID)
                {
                    if(_nowAngle == ANGLE_RIGHT_ID)
                    {
                        _myTrans.position += Vector3.left * 2;
                    }
                    else
                    {
                        _myTrans.position += Vector3.right * 2;
                    }
                }
                else
                {
                    if (_nowAngle == ANGLE_RIGHT_ID)
                    {
                        _myTrans.position += Vector3.right;
                    }
                    else
                    {
                        _myTrans.position += Vector3.left;
                    }
                }
                break; //現条件終了

            case 1: //第２条件 --------------------------------------------------------------------------------------------------------------------------
                _srsPos.firstSRSPos = _myTrans.position; //第１条件位置保存
                _myTrans.position = _srsPos.startPos; //初期座標引き戻し
                if (_nowAngle == ANGLE_UP_ID)
                {
                    _myTrans.position += Vector3.right * _moveDire;
                    break; //現条件終了
                }
                if (_nowAngle == ANGLE_DOWN_ID)
                {
                    _myTrans.position += Vector3.right * 2 * _moveDire;
                    break; //現条件終了
                }
                if (_moveDire == DIRE_LEFT_ID)
                {
                    if (_nowAngle == ANGLE_RIGHT_ID)
                    {
                        _myTrans.position += Vector3.left * 2;
                    }
                    else
                    {
                        _myTrans.position += Vector3.right * 2;
                    }
                }
                else
                {
                    if (_nowAngle == ANGLE_RIGHT_ID)
                    {
                        _myTrans.position += Vector3.right;
                    }
                    else
                    {
                        _myTrans.position += Vector3.left;
                    }
                }
                break; //現条件終了

            case 2: //第３条件 --------------------------------------------------------------------------------------------------------------------------
                _srsPos.secondSRSPos = _myTrans.position; //第２条件位置保存
                _myTrans.position = _srsPos.firstSRSPos; //第１座標引き戻し
                if(_nowAngle == ANGLE_RIGHT_ID)
                {
                    if(_moveDire == DIRE_RIGHT_ID)
                    {
                        _myTrans.position += Vector3.down;
                    }
                    else
                    {
                        _myTrans.position += Vector3.down * 2;
                    }
                    break; //現条件終了
                }
                if(_nowAngle == ANGLE_LEFT_ID)
                {
                    if(_moveDire == DIRE_RIGHT_ID)
                    {
                        _myTrans.position += Vector3.up;
                    }
                    else
                    {
                        _myTrans.position += Vector3.up * 2;
                    }
                    break; //現条件終了
                }
                if(_nowAngle == ANGLE_UP_ID)
                {
                    if(_moveDire == DIRE_LEFT_ID)
                    {
                        _myTrans.position += Vector3.down;
                    }
                    else
                    {
                        _myTrans.position += Vector3.up * 2;
                    }
                    break; //現条件終了
                }
                if (_moveDire == DIRE_LEFT_ID)
                {
                    _myTrans.position += Vector3.up;
                }
                else
                {
                    _myTrans.position += Vector3.down * 2;
                }
                break; //現条件終了

            case 3: //第４条件 --------------------------------------------------------------------------------------------------------------------------
                _myTrans.position = _srsPos.secondSRSPos; //第１条件座標引き戻し
                if (_nowAngle == ANGLE_RIGHT_ID)
                {
                    if (_moveDire == DIRE_LEFT_ID)
                    {
                        _myTrans.position += Vector3.up;
                    }
                    else
                    {
                        _myTrans.position += Vector3.up * 2;
                    }
                    break; //現条件終了
                }
                if (_nowAngle == ANGLE_LEFT_ID)
                {
                    if (_moveDire == DIRE_LEFT_ID)
                    {
                        _myTrans.position += Vector3.down;
                    }
                    else
                    {
                        _myTrans.position += Vector3.down * 2;
                    }
                    break; //現条件終了
                }
                if (_nowAngle == ANGLE_UP_ID)
                {
                    if (_moveDire == DIRE_RIGHT_ID)
                    {
                        _myTrans.position += Vector3.up;
                    }
                    else
                    {
                        _myTrans.position += Vector3.down * 2;
                    }
                    break; //現条件終了
                }
                if (_moveDire == DIRE_RIGHT_ID)
                {
                    _myTrans.position += Vector3.down;
                }
                else
                {
                    _myTrans.position += Vector3.up * 2;
                }
                break; //現条件終了

            case 4: //回転できない -----------------------------------------------------------------------------------------------------------------------
                _myTrans.position = _srsPos.startPos; //初期座標引き戻し
                _needReturn = true;
                return; //現条件終了
        }
        _srsCnt++; //スパロテ記録
        if (CheckMino()) { SRSByFour(); } //衝突判定があった場合、スパロテ継続
        return;
    }

    // インターフェイス継承
    public void CreateMinoUnit(IMinoInfo[] minoBlocks,IMinoCreatable.MinoType setModel)
    {
        //位置、角度初期化
        _myTrans.position = _createStartPos;
        _myTrans.eulerAngles = Vector3.zero;

        //ミノブロックを設定
        _minos.Clear();
        _minos.AddRange(minoBlocks);

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
        foreach(IMinoInfo mino in _minos)
        {
            mino.ChangeColor(_unionColor);
        }
    }
    #endregion
}
