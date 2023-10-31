// -------------------------------------------------------------------------------
// PlayableMino.cs
//
// 作成日: 2023/10/18
// 作成者: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableMino : AccessibleToField, IMinoUnionCtrl
{
    #region 変数
    //スパロテ制御用
    private const int ANGLE_UP_ID = 0; //上向きID
    private const int ANGLE_RIGHT_ID = 1; //右向きID
    private const int ANGLE_DOWN_ID = 2; //下向きID
    private const int ANGLE_LEFT_ID = 3; //左向きID
    private const int DIRE_RIGHT_ID = 1; //右回転向きID
    private const int DIRE_LEFT_ID = -1; //左回転向きID
    struct SRSPosSave //スパロテの座標保存データ
    {
        public Vector3 startPos;
        public Vector3 firstSRSPos;
        public Vector3 secondSRSPos;
    }
    private SRSPosSave _srsPos = new();

    //Tスピン判定用
    private const int TSPIN_JUDGE_CNT = 3; //Tスピン隅判定値
    private const int MINITSPIN_JUDGE_VALUE270 = 270; //ミニTスピン判定用角度１
    private const int MINITSPIN_JUDGE_VALUE180 = 180; //ミニTスピン判定用角度２
    private const int MINITSPIN_JUDGE_SRSCNT = 4; //ミニTスピン

    //プレイヤー操作制御用
    private const int ROTATE_VALUE = 90; //回転処理の回転角度

    //回転系統 スパロテなど
    private int _nowAngleID = 0; //現在のミノの向き
    private int _moveDire = 0; //回転方向
    private bool _needReturn = false; //回転巻き戻し判定
    private int _srsCnt = 0; //スパロテの回数

    //自由落下
    private float _minoFallTime = 0.8f; //落下時間
    private float _fallTimer = 0; //落下計測タイマー

    //ロックダウン機構
    private const float LOCKDOWN_WAIT_TIMER = 0.5f; //設置待機時間
    private float _waitTimer = 0; //待機タイマー
    private int _lockDownCancel = 0; //設置回避数
    private const int MAX_CANCEL_CNT = 15; //最大回避数

    //ソフトドロップ機構
    private const float SOFTDROP_FALLTIME = 0.05f; //ソフトドロップの周期
    private float _softDropTimer = 0f; //ソフトドロップタイマー

    //ホールド判定
    private bool _isHold = false; //ホールド使用判定

    //サウンド関連
    [SerializeField, Header("回転SE")]
    private AudioClip _rotateSE = default;
    [SerializeField, Header("TスピンSE")]
    private AudioClip _tSpinSE = default;
    [SerializeField, Header("ハードドロップSE")]
    private AudioClip _hardDropSE = default;
    private AudioSource _myAudio = default; //自身のAudioSource

    private IGhostStartable _ghost = default; //ゴーストシステム
    #endregion

    #region プロパティ
    public float FallTime { set => _minoFallTime = value; }
    #endregion

    #region メソッド
    /// <summary>
    /// 初期化処理
    /// </summary>
    void Awake()
    {
        //初期化
        _ghost = transform.parent.GetComponentInChildren<GhostMino>();
        _myAudio = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        //ミノ生成されていない
        if(MyTransform == default) { return; }

        //ミノブロックが渡されていない（子付けされていない）
        if (MyTransform.childCount == 0) { return; }

        if (CheckMinoCollision(0, -1))
        {
            //設置判定
            LockDown();
        }
        else
        {
            //時間経過落下
            FallMino();
        }
    }

    // インターフェイス継承
    public void Move(int x)
    {
        //移動先に衝突判定があるか
        if (!CheckMinoCollision(x, 0))
        { 
            //移動反映
            MyTransform.position += Vector3.right * x;
            //ステータス反映
            MoveToChangeStatus();
            //回転方向消去
            _moveDire = 0;
        }
    }

    // インターフェイス継承
    public void Rotate(int dire)
    {
        //回転反映
        MyTransform.eulerAngles -= Vector3.forward * ROTATE_VALUE * dire;
        
        //正規化取得
        _nowAngleID = GetAngleID(Mathf.FloorToInt(MyTransform.eulerAngles.z)); //向き取得
        _moveDire = dire; //回転方向取得
        _needReturn = false; //回転可能

        //スパロテ回数初期化
        _srsCnt = 0;
        //衝突判定があった場合はスーパーローテーションシステムを実行する
        if (CheckMinoCollision())
        {
            if (MyModel == IMinoCreatable.MinoType.minoI) { SRSByFour(); } //サイズが４ｘ４
            SRSByThree(); //サイズが３ｘ３

        }

        //スパロテができない場合は角度を戻す
        if (_needReturn)
        {
            //角度を戻す
            MyTransform.eulerAngles -= Vector3.forward * ROTATE_VALUE * -dire; 
            _nowAngleID = GetAngleID(Mathf.FloorToInt(MyTransform.eulerAngles.z)); //向き取得
            _moveDire = 0; //回転方向消去
            return;
        }

        //Tスピン判定がある場合は、効果音を変える
        if(MyModel == IMinoCreatable.MinoType.minoT && EdgeCollisionByTSpin())
        {
            _myAudio.PlayOneShot(_tSpinSE); //Tスピン効果音再生
        }
        else
        {
            _myAudio.PlayOneShot(_rotateSE); //通常効果音再生
        }

        //ステータス反映
        MoveToChangeStatus();
    }

    /// <summary>
    /// <para>GetAngleID</para>
    /// <para>角度から角度IDを取得します</para>
    /// </summary>
    /// <param name="angle">操作ミノの角度</param>
    /// <returns>角度ID</returns>
    public int GetAngleID(int angle)
    {
        //角度は全てマイナス方向に調整した状態にする
        //0の場合は0を返す
        if(angle == 0) { return 0; }
        //角度がプラスである
        if (0 < angle)
        {
            return (angle - 360) / ROTATE_VALUE * -1;
        }
        return angle / ROTATE_VALUE * -1;
    }
    
    //インターフェイス継承
    public void HardDrop()
    {
        //落下先に衝突判定がない
        if (!CheckMinoCollision(0, -1))
        {
            //回転方向消去
            _moveDire = 0;
            //１列落下
            MyTransform.position += Vector3.down;
            //再起呼び出し
            HardDrop();
        }
        else //ある
        {
            //効果音再生
            _myAudio.PlayOneShot(_hardDropSE);
            //コミット
            Commit();

            return; //処理終了
        }

        return;
    }

    //インターフェイス継承
    public void SoftDrop()
    {
        //下が空白ではない
        if (!CheckMinoCollision(0, -1))
        {
            _softDropTimer += Time.deltaTime; //タイマー加算
            //周期時間以上経った
            if(SOFTDROP_FALLTIME <= _softDropTimer)
            {
                MyTransform.position += Vector3.down; //1マス落下
                _softDropTimer = 0; //タイマー初期化
            }
            _fallTimer = 0; //自由落下を停止（０でストップ）
        }
    }

    //インターフェイス継承
    public bool CheckHasHold()
    {
        //まだホールドしていない
        if (!_isHold)
        {
            _isHold = true; //ホールドすることを記録
            return false; //まだホールドしていないことを送信
        }
        else
        {
            return true; //ホールド済みであることを送信
        }
    }

    /// <summary>
    /// <para>FallMino</para>
    /// <para>ミノを一定時間毎に落下させます</para>
    /// </summary>
    private void FallMino()
    {
        _fallTimer += Time.deltaTime; //タイマー加算

        //落下時間になったか
        if(_minoFallTime < _fallTimer)
        {
            //回転方向消去
            _moveDire = 0;

            //ミノを１マス落下
            MyTransform.position += Vector3.down;

            //タイマー初期化
            _fallTimer = 0;
        }
    }

    /// <summary>
    /// <para>LockDown</para>
    /// <para>ミノを設置するかの判定を行います</para>
    /// </summary>
    private void LockDown()
    {
        _waitTimer += Time.deltaTime; //タイマー加算

        //設置時間になった または 回避数が最大値を超えている場合
        if(LOCKDOWN_WAIT_TIMER <= _waitTimer || MAX_CANCEL_CNT < _lockDownCancel )
        {
            //コミット
            Commit();
        }
    }

    /// <summary>
    /// <para>MoveToResetStatus</para>
    /// <para>移動後に変更されるステータスの初期化・反映をします</para>
    /// </summary>
    private void MoveToChangeStatus()
    {
        //ゴースト設定
        _ghost.ChangeTransformGhost(MyTransform);

        //設置される直前だった場合、回避数をカウント
        if (0 < _waitTimer) { _lockDownCancel++; }
        //回避数が最大を超えていない場合、設置待機タイマーを初期化
        if (_lockDownCancel <= MAX_CANCEL_CNT) { _waitTimer = 0; }
    
    }

    /// <summary>
    /// <para>Commit</para>
    /// <para>操作を完了します</para>
    /// </summary>
    private void Commit()
    {
        //Tスピン検査を行う
        //ミニTスピンは今回の場合はTスピンではないものとして扱う
        if(JudgeTSpin())
        {
            SetTSpin(true); //Tスピンとして設定
        }
        else
        {
            SetTSpin(false); //別物として設定
        }
        //ミノ設置
        SetMinoForField();
        //回避数初期化
        _lockDownCancel = 0;
        //設置待機タイマー初期化
        _waitTimer = 0;
        //タイマー初期化
        _fallTimer = 0;
        //ホールド判定
        _isHold = false;
    }

    /// <summary>
    /// <para>JudgeTSpin</para>
    /// <para>Tスピンか判定します</para>
    /// </summary>
    /// <returns>Tスピン判定</returns>
    private bool JudgeTSpin()
    {
        //前提：Tミノではない
        if(MyModel != IMinoCreatable.MinoType.minoT) { return false; }

        //条件１：軸を中心とした３ｘ３の４つの隅に衝突判定がある
        //判定のあった隅がTスピン判定値より小さい場合は、Tスピンではない
        if (!EdgeCollisionByTSpin()) { return false; }

        //条件２：最後の動作が回転である
        //回転でない場合は、Tスピンではない
        if (_moveDire == 0) { return false; }

        //ここからはミニTスピン判定を行う
        //これらの条件に合致した場合は、ミニTスピンのためfalseと返す
        //条件１：凸の両端のどちらかに衝突判定がない
        if(!(CheckCollisionByCenter(TSpinCos(_nowAngleID), TSpinSin(_nowAngleID)) && CheckCollisionByCenter(TSpinSin(_nowAngleID), -TSpinCos(_nowAngleID))))
        {
            //条件２：スパロテの回転補正の第四条件でないこと
            if (_srsCnt != MINITSPIN_JUDGE_SRSCNT) { return false; } //ミニTスピンである
        }

        //すべての条件に合致する場合は、Tスピンである
        return true;

    }
    
    /// <summary>
    /// <para>EdgeCollisionByTSpin</para>
    /// <para>隅の衝突判定が3つ以上ある場合、Trueを返します</para>
    /// </summary>
    /// <returns></returns>
    private bool EdgeCollisionByTSpin()
    {
        //隅衝突判定カウント用
        int collisionCnt = 0;
        //隅判定
        if (CheckCollisionByCenter(1, 1)) { collisionCnt++; }
        if (CheckCollisionByCenter(-1, 1)) { collisionCnt++; }
        if (CheckCollisionByCenter(1, -1)) { collisionCnt++; }
        if (CheckCollisionByCenter(-1, -1)) { collisionCnt++; }
        //3つ以上ある場合はTrue
        if (TSPIN_JUDGE_CNT <= collisionCnt) { return true; }


        return false; //3つに満たさない
    }

    /// <summary>
    /// <para>TSpinCos</para>
    /// <para>ミニTスピンのアルゴリズム部品</para>
    /// <para>与えられた値が180度以上であれば-1</para>
    /// <para>以下であれば1を返します</para>
    /// </summary>
    /// <param name="angleID">角度</param>
    /// <returns></returns>
    private int TSpinSin(float angleID)
    {
        if(MINITSPIN_JUDGE_VALUE180 <= angleID * ROTATE_VALUE)
        {
            return -1;
        }
        return 1;
    }

    /// <summary>
    /// <para>TSpinCos</para>
    /// <para>ミニTスピンのアルゴリズム部品</para>
    /// <para>与えられた値が270度または0度の時は-1</para>
    /// <para>それ以外であれば1を返します</para>
    /// </summary>
    /// <param name="angleID">角度</param>
    /// <returns></returns>
    private int TSpinCos(float angleID)
    {
        if (angleID * ROTATE_VALUE % MINITSPIN_JUDGE_VALUE270 == 0)
        {
            return -1;
        }
        return 1;
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
                _srsPos.startPos = MyTransform.position; //初期座標保存
                if (_nowAngleID == ANGLE_RIGHT_ID || (_nowAngleID == ANGLE_DOWN_ID && _moveDire == DIRE_LEFT_ID) || (_nowAngleID == ANGLE_UP_ID && _moveDire == DIRE_RIGHT_ID))
                {
                    MyTransform.position += Vector3.left;
                    break; //現条件終了
                }
                MyTransform.position += Vector3.right;
                break; //現条件終了

            case 1: //第２条件 --------------------------------------------------------------------------------------------------------------------------
                //座標継続
                if (_nowAngleID == ANGLE_RIGHT_ID || _nowAngleID == ANGLE_LEFT_ID)
                {
                    MyTransform.position += Vector3.up;
                    break; //現条件終了
                }
                MyTransform.position += Vector3.down;
                break; //現条件終了

            case 2: //第３条件 --------------------------------------------------------------------------------------------------------------------------
                MyTransform.position = _srsPos.startPos; //初期座標引き戻し
                if (_nowAngleID == ANGLE_RIGHT_ID || _nowAngleID == ANGLE_LEFT_ID)
                {
                    MyTransform.position += Vector3.down * 2;
                    break; //現条件終了
                }
                MyTransform.position += Vector3.up * 2;
                break; //現条件終了

            case 3: //第４条件 --------------------------------------------------------------------------------------------------------------------------
                //座標継続
                if (_nowAngleID == ANGLE_RIGHT_ID || (_nowAngleID == ANGLE_DOWN_ID && _moveDire == DIRE_LEFT_ID) || (_nowAngleID == ANGLE_UP_ID && _moveDire == DIRE_RIGHT_ID))
                {
                    MyTransform.position += Vector3.left;
                    break; //現条件終了
                }
                MyTransform.position += Vector3.right;
                break; //現条件終了

            case 4: //回転できない ----------------------------------------------------------------------------------------------------------------------
                MyTransform.position = _srsPos.startPos; //初期座標引き戻し
                _needReturn = true;
                return; //現条件終了
        }
        _srsCnt++; //スパロテ記録
        if (CheckMinoCollision()) { SRSByThree(); } //衝突判定があった場合、スパロテ継続
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
                _srsPos.startPos = MyTransform.position; //初期座標保存
                if(_nowAngleID == ANGLE_UP_ID)
                {
                    MyTransform.position += Vector3.left * 2 * _moveDire;
                    break; //現条件終了
                }
                if(_nowAngleID == ANGLE_DOWN_ID)
                {
                    MyTransform.position += Vector3.left * _moveDire;
                    break; //現条件終了
                }
                if(_moveDire == DIRE_RIGHT_ID)
                {
                    if(_nowAngleID == ANGLE_RIGHT_ID)
                    {
                        MyTransform.position += Vector3.left * 2;
                    }
                    else
                    {
                        MyTransform.position += Vector3.right * 2;
                    }
                }
                else
                {
                    if (_nowAngleID == ANGLE_RIGHT_ID)
                    {
                        MyTransform.position += Vector3.right;
                    }
                    else
                    {
                        MyTransform.position += Vector3.left;
                    }
                }
                break; //現条件終了

            case 1: //第２条件 --------------------------------------------------------------------------------------------------------------------------
                _srsPos.firstSRSPos = MyTransform.position; //第１条件位置保存
                MyTransform.position = _srsPos.startPos; //初期座標引き戻し
                if (_nowAngleID == ANGLE_UP_ID)
                {
                    MyTransform.position += Vector3.right * _moveDire;
                    break; //現条件終了
                }
                if (_nowAngleID == ANGLE_DOWN_ID)
                {
                    MyTransform.position += Vector3.right * 2 * _moveDire;
                    break; //現条件終了
                }
                if (_moveDire == DIRE_LEFT_ID)
                {
                    if (_nowAngleID == ANGLE_RIGHT_ID)
                    {
                        MyTransform.position += Vector3.left * 2;
                    }
                    else
                    {
                        MyTransform.position += Vector3.right * 2;
                    }
                }
                else
                {
                    if (_nowAngleID == ANGLE_RIGHT_ID)
                    {
                        MyTransform.position += Vector3.right;
                    }
                    else
                    {
                        MyTransform.position += Vector3.left;
                    }
                }
                break; //現条件終了

            case 2: //第３条件 --------------------------------------------------------------------------------------------------------------------------
                _srsPos.secondSRSPos = MyTransform.position; //第２条件位置保存
                MyTransform.position = _srsPos.firstSRSPos; //第１座標引き戻し
                if(_nowAngleID == ANGLE_RIGHT_ID)
                {
                    if(_moveDire == DIRE_RIGHT_ID)
                    {
                        MyTransform.position += Vector3.down;
                    }
                    else
                    {
                        MyTransform.position += Vector3.down * 2;
                    }
                    break; //現条件終了
                }
                if(_nowAngleID == ANGLE_LEFT_ID)
                {
                    if(_moveDire == DIRE_RIGHT_ID)
                    {
                        MyTransform.position += Vector3.up;
                    }
                    else
                    {
                        MyTransform.position += Vector3.up * 2;
                    }
                    break; //現条件終了
                }
                if(_nowAngleID == ANGLE_UP_ID)
                {
                    if(_moveDire == DIRE_LEFT_ID)
                    {
                        MyTransform.position += Vector3.down;
                    }
                    else
                    {
                        MyTransform.position += Vector3.up * 2;
                    }
                    break; //現条件終了
                }
                if (_moveDire == DIRE_LEFT_ID)
                {
                    MyTransform.position += Vector3.up;
                }
                else
                {
                    MyTransform.position += Vector3.down * 2;
                }
                break; //現条件終了

            case 3: //第４条件 --------------------------------------------------------------------------------------------------------------------------
                MyTransform.position = _srsPos.secondSRSPos; //第１条件座標引き戻し
                if (_nowAngleID == ANGLE_RIGHT_ID)
                {
                    if (_moveDire == DIRE_LEFT_ID)
                    {
                        MyTransform.position += Vector3.up;
                    }
                    else
                    {
                        MyTransform.position += Vector3.up * 2;
                    }
                    break; //現条件終了
                }
                if (_nowAngleID == ANGLE_LEFT_ID)
                {
                    if (_moveDire == DIRE_LEFT_ID)
                    {
                        MyTransform.position += Vector3.down;
                    }
                    else
                    {
                        MyTransform.position += Vector3.down * 2;
                    }
                    break; //現条件終了
                }
                if (_nowAngleID == ANGLE_UP_ID)
                {
                    if (_moveDire == DIRE_RIGHT_ID)
                    {
                        MyTransform.position += Vector3.up;
                    }
                    else
                    {
                        MyTransform.position += Vector3.down * 2;
                    }
                    break; //現条件終了
                }
                if (_moveDire == DIRE_RIGHT_ID)
                {
                    MyTransform.position += Vector3.down;
                }
                else
                {
                    MyTransform.position += Vector3.up * 2;
                }
                break; //現条件終了

            case 4: //回転できない -----------------------------------------------------------------------------------------------------------------------
                MyTransform.position = _srsPos.startPos; //初期座標引き戻し
                _needReturn = true;
                return; //現条件終了
        }
        _srsCnt++; //スパロテ記録
        if (CheckMinoCollision()) { SRSByFour(); } //衝突判定があった場合、スパロテ継続
        return;
    }

    // クラス継承
    public override void CreateMinoUnit(IMinoBlockAccessible[] minoBlocks,IMinoCreatable.MinoType setModel)
    {
        //基底メソッド使用
        base.CreateMinoUnit(minoBlocks,setModel);

        //OミノとIミノは形が特殊なのでスタート地点を変更する
        if(MyModel == IMinoCreatable.MinoType.minoO) //Oミノ
        {
            MyTransform.position += Vector3.right * IMinoCreatable.EXCEPTION_SHIFT_0_5 + Vector3.up * IMinoCreatable.EXCEPTION_SHIFT_0_5;
        }
        if(MyModel == IMinoCreatable.MinoType.minoI) //Iミノ
        {
            MyTransform.position += Vector3.right * IMinoCreatable.EXCEPTION_SHIFT_0_5 + Vector3.down * IMinoCreatable.EXCEPTION_SHIFT_0_5;
        }

        //ゴースト設定
        _ghost.ChangeModelGhost(MyModel, MyTransform.position);

        //生成位置にミノが既にあるか
        if (CheckMinoCollision())
        {
            //プレイ不可能
            NotPlay();
        }
    }
    #endregion
}
