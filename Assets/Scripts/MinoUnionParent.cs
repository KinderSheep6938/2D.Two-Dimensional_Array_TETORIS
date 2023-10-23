// -------------------------------------------------------------------------------
// MinoUnionParent.cs
//
// 作成日: 2023/10/18
// 作成者: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinoUnionParent : MinoModelCreate, IMinoUnionCtrl
{
    #region 変数
    struct SRSPosSave //スパロテの座標保存データ
    {
        public Vector3 startPos;
        public Vector3 firstSRSPos;
        public Vector3 secondSRSPos;
    }
    private SRSPosSave _srsPos = new();

    //スパロテ制御用
    const int ANGLE_UP_ID = 0; //上向きID
    const int ANGLE_RIGHT_ID = 1; //右向きID
    const int ANGLE_DOWN_ID = 2; //下向きID
    const int ANGLE_LEFT_ID = 3; //左向きID
    const int DIRE_RIGHT_ID = 1; //右回転向きID
    const int DIRE_LEFT_ID = -1; //左回転向きID

    const float ROTATE_VALUE = 90f; //回転処理の回転角度
    const float FALL_TIME = 0.5f; //落下時間
    const float SOFTDROP_SPEED = 4.5f; //ソフトドロップの倍速速度
    
    private int _nowAngle = 0; //現在のミノの向き
    private int _moveDire = 0; //回転方向
    private bool _needReturn = false; //回転巻き戻し判定
    private int _srsCnt = 0; //スパロテの回数
    private float _fallTimer = 0; //落下計測タイマー

    private IFieldCtrl _fieldCtrl = default; //フィールド管理システムのインターフェイス
    #endregion

    #region プロパティ

    #endregion

    #region メソッド
    /// <summary>
    /// 初期化処理
    /// </summary>
    void Awake()
    {
        //初期化
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
        if (MyTrans.childCount == 0) { return; }

        //時間経過落下
        FallMino();
    }

    // インターフェイス継承
    public void Move(int x)
    {
        //移動反映
        MyTrans.position += Vector3.right * x;
        //衝突判定があった場合は戻す
        if (CheckMino()) { MyTrans.position -= Vector3.right * x; }
    }

    // インターフェイス継承
    public void Rotate(int angle)
    {
        //回転反映
        MyTrans.eulerAngles -= Vector3.forward * ROTATE_VALUE * angle;

        _nowAngle = (int)(MyTrans.eulerAngles.z / ROTATE_VALUE); //向き取得
        _moveDire = angle; //回転方向取得
        _needReturn = false; //回転可能

        //スパロテ回数初期化
        _srsCnt = 0;
        //衝突判定があった場合はスーパーローテーションシステムを実行する
        if (CheckMino())
        {
            Debug.Log("srs");
            if (MyModel == IMinoCreatable.MinoType.minoI) { SRSByFour(); } //サイズが４ｘ４
            SRSByThree(); //サイズが３ｘ３
        }

        //スパロテができない場合は角度を戻す
        if (_needReturn)
        {
            MyTrans.eulerAngles -= Vector3.forward * ROTATE_VALUE * -angle;
            _nowAngle = (int)(MyTrans.eulerAngles.z / ROTATE_VALUE); //向き取得
        }
    }
    
    //インターフェイス継承
    public void HardDrop()
    {
        //１列落下
        MyTrans.position += Vector3.down;

        //落下先に衝突判定がある
        if (CheckMino())
        {
            //もとに戻す
            MyTrans.position += Vector3.up;
            //ミノをフィールドに設定
            foreach (IMinoInfo mino in Minos)
            {
                //親子関係削除
                mino.DisConnect();
                //コミット
                _fieldCtrl.SetMino(mino.MinoX, mino.MinoY);
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

    //インターフェイス継承
    public void SoftDrop()
    {
        //タイマー倍増
        _fallTimer += Time.deltaTime * SOFTDROP_SPEED;
    }

    private void FallMino()
    {
        _fallTimer += Time.deltaTime; //タイマー加算

        //落下時間になったか
        if(FALL_TIME < _fallTimer)
        {
            _fallTimer = 0;
            //ミノを１マス落下
            MyTrans.position += Vector3.down;

            //落下先に衝突判定がある
            if (CheckMino())
            {
                //もとに戻す
                MyTrans.position += Vector3.up;
                //ミノをフィールドに設定
                foreach(IMinoInfo mino in Minos)
                {
                    //親子関係削除
                    mino.DisConnect();
                    //コミット
                    _fieldCtrl.SetMino(mino.MinoX, mino.MinoY);
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
        foreach (IMinoInfo mino in Minos)
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
                _srsPos.startPos = MyTrans.position; //初期座標保存
                if (_nowAngle == ANGLE_RIGHT_ID || (_nowAngle == ANGLE_DOWN_ID && _moveDire == DIRE_LEFT_ID) || (_nowAngle == ANGLE_UP_ID && _moveDire == DIRE_RIGHT_ID))
                {
                    MyTrans.position += Vector3.left;
                    break; //現条件終了
                }
                MyTrans.position += Vector3.right;
                break; //現条件終了

            case 1: //第２条件 --------------------------------------------------------------------------------------------------------------------------
                //座標継続
                if (_nowAngle == ANGLE_RIGHT_ID || _nowAngle == ANGLE_LEFT_ID)
                {
                    MyTrans.position += Vector3.up;
                    break; //現条件終了
                }
                MyTrans.position += Vector3.down;
                break; //現条件終了

            case 2: //第３条件 --------------------------------------------------------------------------------------------------------------------------
                MyTrans.position = _srsPos.startPos; //初期座標引き戻し
                if (_nowAngle == ANGLE_RIGHT_ID || _nowAngle == ANGLE_LEFT_ID)
                {
                    MyTrans.position += Vector3.down * 2;
                    break; //現条件終了
                }
                MyTrans.position += Vector3.up * 2;
                break; //現条件終了

            case 3: //第４条件 --------------------------------------------------------------------------------------------------------------------------
                //座標継続
                if (_nowAngle == ANGLE_RIGHT_ID || (_nowAngle == ANGLE_DOWN_ID && _moveDire == DIRE_LEFT_ID) || (_nowAngle == ANGLE_UP_ID && _moveDire == DIRE_RIGHT_ID))
                {
                    MyTrans.position += Vector3.left;
                    break; //現条件終了
                }
                MyTrans.position += Vector3.right;
                break; //現条件終了

            case 4: //回転できない ----------------------------------------------------------------------------------------------------------------------
                MyTrans.position = _srsPos.startPos; //初期座標引き戻し
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
                _srsPos.startPos = MyTrans.position; //初期座標保存
                if(_nowAngle == ANGLE_UP_ID)
                {
                    MyTrans.position += Vector3.left * 2 * _moveDire;
                    break; //現条件終了
                }
                if(_nowAngle == ANGLE_DOWN_ID)
                {
                    MyTrans.position += Vector3.left * _moveDire;
                    break; //現条件終了
                }
                if(_moveDire == DIRE_RIGHT_ID)
                {
                    if(_nowAngle == ANGLE_RIGHT_ID)
                    {
                        MyTrans.position += Vector3.left * 2;
                    }
                    else
                    {
                        MyTrans.position += Vector3.right * 2;
                    }
                }
                else
                {
                    if (_nowAngle == ANGLE_RIGHT_ID)
                    {
                        MyTrans.position += Vector3.right;
                    }
                    else
                    {
                        MyTrans.position += Vector3.left;
                    }
                }
                break; //現条件終了

            case 1: //第２条件 --------------------------------------------------------------------------------------------------------------------------
                _srsPos.firstSRSPos = MyTrans.position; //第１条件位置保存
                MyTrans.position = _srsPos.startPos; //初期座標引き戻し
                if (_nowAngle == ANGLE_UP_ID)
                {
                    MyTrans.position += Vector3.right * _moveDire;
                    break; //現条件終了
                }
                if (_nowAngle == ANGLE_DOWN_ID)
                {
                    MyTrans.position += Vector3.right * 2 * _moveDire;
                    break; //現条件終了
                }
                if (_moveDire == DIRE_LEFT_ID)
                {
                    if (_nowAngle == ANGLE_RIGHT_ID)
                    {
                        MyTrans.position += Vector3.left * 2;
                    }
                    else
                    {
                        MyTrans.position += Vector3.right * 2;
                    }
                }
                else
                {
                    if (_nowAngle == ANGLE_RIGHT_ID)
                    {
                        MyTrans.position += Vector3.right;
                    }
                    else
                    {
                        MyTrans.position += Vector3.left;
                    }
                }
                break; //現条件終了

            case 2: //第３条件 --------------------------------------------------------------------------------------------------------------------------
                _srsPos.secondSRSPos = MyTrans.position; //第２条件位置保存
                MyTrans.position = _srsPos.firstSRSPos; //第１座標引き戻し
                if(_nowAngle == ANGLE_RIGHT_ID)
                {
                    if(_moveDire == DIRE_RIGHT_ID)
                    {
                        MyTrans.position += Vector3.down;
                    }
                    else
                    {
                        MyTrans.position += Vector3.down * 2;
                    }
                    break; //現条件終了
                }
                if(_nowAngle == ANGLE_LEFT_ID)
                {
                    if(_moveDire == DIRE_RIGHT_ID)
                    {
                        MyTrans.position += Vector3.up;
                    }
                    else
                    {
                        MyTrans.position += Vector3.up * 2;
                    }
                    break; //現条件終了
                }
                if(_nowAngle == ANGLE_UP_ID)
                {
                    if(_moveDire == DIRE_LEFT_ID)
                    {
                        MyTrans.position += Vector3.down;
                    }
                    else
                    {
                        MyTrans.position += Vector3.up * 2;
                    }
                    break; //現条件終了
                }
                if (_moveDire == DIRE_LEFT_ID)
                {
                    MyTrans.position += Vector3.up;
                }
                else
                {
                    MyTrans.position += Vector3.down * 2;
                }
                break; //現条件終了

            case 3: //第４条件 --------------------------------------------------------------------------------------------------------------------------
                MyTrans.position = _srsPos.secondSRSPos; //第１条件座標引き戻し
                if (_nowAngle == ANGLE_RIGHT_ID)
                {
                    if (_moveDire == DIRE_LEFT_ID)
                    {
                        MyTrans.position += Vector3.up;
                    }
                    else
                    {
                        MyTrans.position += Vector3.up * 2;
                    }
                    break; //現条件終了
                }
                if (_nowAngle == ANGLE_LEFT_ID)
                {
                    if (_moveDire == DIRE_LEFT_ID)
                    {
                        MyTrans.position += Vector3.down;
                    }
                    else
                    {
                        MyTrans.position += Vector3.down * 2;
                    }
                    break; //現条件終了
                }
                if (_nowAngle == ANGLE_UP_ID)
                {
                    if (_moveDire == DIRE_RIGHT_ID)
                    {
                        MyTrans.position += Vector3.up;
                    }
                    else
                    {
                        MyTrans.position += Vector3.down * 2;
                    }
                    break; //現条件終了
                }
                if (_moveDire == DIRE_RIGHT_ID)
                {
                    MyTrans.position += Vector3.down;
                }
                else
                {
                    MyTrans.position += Vector3.up * 2;
                }
                break; //現条件終了

            case 4: //回転できない -----------------------------------------------------------------------------------------------------------------------
                MyTrans.position = _srsPos.startPos; //初期座標引き戻し
                _needReturn = true;
                return; //現条件終了
        }
        _srsCnt++; //スパロテ記録
        if (CheckMino()) { SRSByFour(); } //衝突判定があった場合、スパロテ継続
        return;
    }

    // クラス継承
    public override void CreateMinoUnit(IMinoInfo[] minoBlocks,IMinoCreatable.MinoType setModel)
    {
        //基底メソッド使用
        base.CreateMinoUnit(minoBlocks,setModel);

        //OミノとIミノは形が特殊なのでスタート地点を変更する
        if(MyModel == IMinoCreatable.MinoType.minoO) //Oミノ
        {
            MyTrans.position += Vector3.right * IMinoCreatable.EXCEPTION_SHIFT_0_5 + Vector3.up * IMinoCreatable.EXCEPTION_SHIFT_0_5;
            return;
        }
        if(MyModel == IMinoCreatable.MinoType.minoI) //Iミノ
        {
            MyTrans.position += Vector3.right * IMinoCreatable.EXCEPTION_SHIFT_0_5 + Vector3.down * IMinoCreatable.EXCEPTION_SHIFT_0_5;
        }
    }
    #endregion
}
