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

    //プレイヤー操作制御用
    private const float ROTATE_VALUE = 90f; //回転処理の回転角度
    private const float SOFTDROP_SPEED = 4.5f; //ソフトドロップの倍速速度

    //回転系統 スパロテなど
    private int _nowAngle = 0; //現在のミノの向き
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

    //ホールド判定
    private bool _isHold = false;

    [SerializeField, Tooltip("回転SE")]
    private AudioClip _rotateSE = default;
    [SerializeField, Tooltip("ハードドロップSE")]
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
        _ghost = FindObjectOfType<GhostMino>(); //ゴースト取得
        _myAudio = GetComponent<AudioSource>();
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
        //ミノブロックが渡されている（子付けされている）
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
    public void Rotate(int angle)
    {
        _myAudio.PlayOneShot(_rotateSE); //効果音再生
        //回転反映
        MyTransform.eulerAngles -= Vector3.forward * ROTATE_VALUE * angle;

        _nowAngle = (int)(MyTransform.eulerAngles.z / ROTATE_VALUE); //向き取得
        _moveDire = angle; //回転方向取得
        _needReturn = false; //回転可能

        //スパロテ回数初期化
        _srsCnt = 0;
        //衝突判定があった場合はスーパーローテーションシステムを実行する
        if (CheckMinoCollision())
        {
            Debug.Log("srs");
            if (MyModel == IMinoCreatable.MinoType.minoI) { SRSByFour(); } //サイズが４ｘ４
            SRSByThree(); //サイズが３ｘ３
        }

        //スパロテができない場合は角度を戻す
        if (_needReturn)
        {
            MyTransform.eulerAngles -= Vector3.forward * ROTATE_VALUE * -angle; //角度戻す
            _nowAngle = (int)(MyTransform.eulerAngles.z / ROTATE_VALUE); //向き取得
            _moveDire = 0; //回転方向消去
            return;
        }

        //ステータス反映
        MoveToChangeStatus();
    }
    
    //インターフェイス継承
    public void HardDrop()
    {
        //落下先に衝突判定がない
        if (!CheckMinoCollision(0, -1))
        {
            //効果音再生
            _myAudio.PlayOneShot(_hardDropSE); 
            //１列落下
            MyTransform.position += Vector3.down;
            //再起呼び出し
            HardDrop();
        }
        else //ある
        {
            //コミット
            Commit();

            return; //処理終了
        }

        return;
    }

    //インターフェイス継承
    public void SoftDrop()
    {
        //タイマー倍増
        _fallTimer += Time.deltaTime * SOFTDROP_SPEED;
    }

    //インターフェイス継承
    public bool CheckHasHold()
    {
        Debug.Log(_isHold);
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
        //Tミノである場合はTスピン検査を行う
        //条件１ : Tミノである (1行目最初)
        //条件２ : 最後の動作が回転である (1行目最後)
        //条件３ : 軸を中心とした３ｘ３の４つの角に衝突判定がある (2行目)
        //すべての条件が合致した場合は、Tスピン判定を設定する
        if(MyModel == IMinoCreatable.MinoType.minoT && _moveDire != 0
            && CheckCollisionByCenter(1,1) && CheckCollisionByCenter(-1, 1) && CheckCollisionByCenter(-1, -1) && CheckCollisionByCenter(1, -1))
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
    /// <para>SRSByThree</para>
    /// <para>サイズが３のスーパーローテーションシステム</para>
    /// </summary>
    private void SRSByThree()
    {
        switch (_srsCnt)
        {
            case 0: //第１条件 --------------------------------------------------------------------------------------------------------------------------
                _srsPos.startPos = MyTransform.position; //初期座標保存
                if (_nowAngle == ANGLE_RIGHT_ID || (_nowAngle == ANGLE_DOWN_ID && _moveDire == DIRE_LEFT_ID) || (_nowAngle == ANGLE_UP_ID && _moveDire == DIRE_RIGHT_ID))
                {
                    MyTransform.position += Vector3.left;
                    break; //現条件終了
                }
                MyTransform.position += Vector3.right;
                break; //現条件終了

            case 1: //第２条件 --------------------------------------------------------------------------------------------------------------------------
                //座標継続
                if (_nowAngle == ANGLE_RIGHT_ID || _nowAngle == ANGLE_LEFT_ID)
                {
                    MyTransform.position += Vector3.up;
                    break; //現条件終了
                }
                MyTransform.position += Vector3.down;
                break; //現条件終了

            case 2: //第３条件 --------------------------------------------------------------------------------------------------------------------------
                MyTransform.position = _srsPos.startPos; //初期座標引き戻し
                if (_nowAngle == ANGLE_RIGHT_ID || _nowAngle == ANGLE_LEFT_ID)
                {
                    MyTransform.position += Vector3.down * 2;
                    break; //現条件終了
                }
                MyTransform.position += Vector3.up * 2;
                break; //現条件終了

            case 3: //第４条件 --------------------------------------------------------------------------------------------------------------------------
                //座標継続
                if (_nowAngle == ANGLE_RIGHT_ID || (_nowAngle == ANGLE_DOWN_ID && _moveDire == DIRE_LEFT_ID) || (_nowAngle == ANGLE_UP_ID && _moveDire == DIRE_RIGHT_ID))
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
                if(_nowAngle == ANGLE_UP_ID)
                {
                    MyTransform.position += Vector3.left * 2 * _moveDire;
                    break; //現条件終了
                }
                if(_nowAngle == ANGLE_DOWN_ID)
                {
                    MyTransform.position += Vector3.left * _moveDire;
                    break; //現条件終了
                }
                if(_moveDire == DIRE_RIGHT_ID)
                {
                    if(_nowAngle == ANGLE_RIGHT_ID)
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
                    if (_nowAngle == ANGLE_RIGHT_ID)
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
                if (_nowAngle == ANGLE_UP_ID)
                {
                    MyTransform.position += Vector3.right * _moveDire;
                    break; //現条件終了
                }
                if (_nowAngle == ANGLE_DOWN_ID)
                {
                    MyTransform.position += Vector3.right * 2 * _moveDire;
                    break; //現条件終了
                }
                if (_moveDire == DIRE_LEFT_ID)
                {
                    if (_nowAngle == ANGLE_RIGHT_ID)
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
                    if (_nowAngle == ANGLE_RIGHT_ID)
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
                if(_nowAngle == ANGLE_RIGHT_ID)
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
                if(_nowAngle == ANGLE_LEFT_ID)
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
                if(_nowAngle == ANGLE_UP_ID)
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
                if (_nowAngle == ANGLE_RIGHT_ID)
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
                if (_nowAngle == ANGLE_LEFT_ID)
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
                if (_nowAngle == ANGLE_UP_ID)
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
            Debug.Log("MinoCantPlay");
            //プレイ不可能
            NotPlay();
        }
    }
    #endregion
}
