// -------------------------------------------------------------------------------
// FieldManager.cs
//
// 作成日: 2023/10/17
// 作成者: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour, IFieldAccess
{
    #region 変数
    //フィールド管理定数
    private const int FIELD_MAX_WIDTH = 10; //フィールドの横幅
    private const int FIELD_MAX_HEIGHT = 25; //フィールドの縦幅
    private const int FIELD_VIEW_HEIGHT = 21; //フィールドの最大表示縦幅
    private const int TILE_NONE_ID = 0; //フィールドの空白ID
    private const int TILE_MINO_ID = 1; //ミノID
    //ゲームフロー制御用
    private const int MAX_COMMITMINO_CNT = 4; //操作ミノの数
    private const int GAMEEND_ID = -1; //ゲーム不可能状態ID
    private const int PLAYING_ID = 0; //プレイ中状態ID
    private const int COMMIT_ID = 1; //操作完了状態ID

    //その他
    private const int TETRIS_LINE = 4; //テトリス判定ライン数
    private const int TSPIN_SCORE_RATIO = 2; //Tスピン判定のスコア倍増率

    private int[,] _field = new int[FIELD_MAX_HEIGHT,FIELD_MAX_WIDTH]; //フィールド保存 [縦軸:y,横軸:x]
    private List<int> _deleteLineIndexs = new List<int>(); //完成ラインのIndex保存用
    private bool _isLine = false; //完成ライン判定
    private int _fallValue = 0; //ライン削除の落下距離
    private int _commitCnt = 0; //設置した操作ミノブロックの数
    private bool _canPlay = true; //フィールドでのプレイ可否判定
    private bool _tSpin = false; //Tスピン判定

    [SerializeField, Header("空白タイル")]
    private GameObject _fieldTileObj = default; //空白タイル
    [SerializeField, Header("フィールド管理オブジェクト")]
    private Transform _fieldParent = default; //フィールド管理オブジェ

    [SerializeField, Header("ライン消去エフェクト")]
    private LineEffect[] _deleteLineEfe = default; //ライン消去のエフェクトオブジェ
    [SerializeField, Header("テトリステキスト")]
    private TextEffect _tetrisText = default; //テトリス表示のテキスト
    [SerializeField, Header("Tスピンテキスト")]
    private TextEffect _tSpinText = default; //Tスピン表示のテキスト
    [SerializeField, Header("ライン消去SE 0:通常 1:テトリス")]
    private AudioClip[] _deleteLineSE = default; //ライン消去時の効果音
    private AudioSource _myAudio = default; //自身のAudioSource

    private ScoreManager _scoreManager = default; //スコア管理マネージャー
    #endregion

    #region プロパティ
    public bool TSpin { set => _tSpin = value; }
    #endregion

    #region メソッド
    /// <summary>
    /// 初期化処理
    /// </summary>
    void Awake()
    {
        //プレイ可能に設定
        _canPlay = true;

        //マップ初期化
        for(int y = 0; y < FIELD_MAX_HEIGHT; y++) //縦軸
        {
            for(int x = 0; x < FIELD_MAX_WIDTH; x++) //横軸
            { 
                //空白で初期化
                _field[y, x] = TILE_NONE_ID;
                //空白タイル設置
                if(y < FIELD_VIEW_HEIGHT)
                {
                    //空白タイル,生成位置,角度,管理用オブジェ
                    Instantiate(
                        _fieldTileObj,
                        Vector3.right * x + Vector3.up * y,
                        Quaternion.identity,
                        _fieldParent
                        );
                }
            }
        }
        //ライン検査初期化
        _deleteLineIndexs.Clear();

        //その他 初期化
        _myAudio = GetComponent<AudioSource>(); //サウンド
        _scoreManager = GetComponent<ScoreManager>(); //スコアマネージャー
    }

    /// <summary>
    /// <para>CheckLine</para>
    /// <para>フィールドの中でラインができているか検査します</para>
    /// <para>また、ラインができていた場合、スコアを加算します</para>
    /// </summary>
    private void CheckLine()
    {
        //初期化
        _deleteLineIndexs.Clear();

        //ライン検査
        for (int y = 0; y < FIELD_MAX_HEIGHT; y++) //縦軸
        {
            //ライン判定初期化
            _isLine = true;

            //一列検査
            for (int x = 0; x < FIELD_MAX_WIDTH; x++) //横軸
            {
                //一列の中に空白があるか
                if(_field[y,x] == TILE_NONE_ID) 
                {
                    //ラインとして完成していない
                    _isLine = false;
                    break;
                }
            }

            //ラインが完成しているか
            if (_isLine)
            {
                _deleteLineIndexs.Add(y); //完成しているラインを削除対象として保存
            }
        }

        //削除対象ラインがある
        if(_deleteLineIndexs.Count != 0)
        {
            //削除処理を行う
            DeleteLine();

            //効果音
            _myAudio.PlayOneShot(_deleteLineSE[_deleteLineIndexs.Count / TETRIS_LINE]);

            //列ごとにエフェクト
            for(int i = 0; i < _deleteLineIndexs.Count; i++) { _deleteLineEfe[i].SetEffect(_deleteLineIndexs[i]); }

            //4列の場合はテトリスエフェクト (引数としては一番上の列を渡す)
            if (_deleteLineIndexs.Count == TETRIS_LINE) { _tetrisText.SetView(); }

            //Tスピン判定の場合は Tスピンエフェクト と スコア倍増
            if (_tSpin)
            {
                _tSpinText.SetView(); //エフェクト
                _scoreManager.AddScore(_deleteLineIndexs.Count * TSPIN_SCORE_RATIO); //スコア倍増
                return;
            }

            //スコア加算
            _scoreManager.AddScore(_deleteLineIndexs.Count);
        }
        return;
    }

    /// <summary>
    /// <para>DeleteLine</para>
    /// <para>指定したラインを削除します</para>
    /// <para>また、削除した部分より上のミノを落下させます</para>
    /// </summary>
    private void DeleteLine()
    {
        //初期化
        _fallValue = 0;

        //二次元配列操作
        for (int y = 0; y < FIELD_MAX_HEIGHT; y++) //縦軸
        {
            //該当した条件のみ処理を実行する
            for (int x = 0; x < FIELD_MAX_WIDTH; x++) //横軸
            {
                //削除対象のラインである
                if (_deleteLineIndexs.Contains(y))
                {
                    //空白で埋める
                    _field[y, x] = TILE_NONE_ID;
                    continue;
                }

                //落下距離が設定されている
                if(0 < _fallValue)
                {
                    //現在の状況を落下先に移動させる
                    _field[y - _fallValue, x] = _field[y, x];
                    //落下前の座標に空白を設定する
                    _field[y, x] = TILE_NONE_ID;
                    continue;
                }
            }

            //落下距離増加
            if (_deleteLineIndexs.Contains(y)) { _fallValue++; }
        }

        //フィールドに存在するミノに削除処理と落下処理を実行させる
        foreach (ILineMinoCtrl lineMino in FindObjectsOfType<Mino>())
        {
            lineMino.LineCtrl(_deleteLineIndexs);
        }
        return;
    }

    /// <summary>
    /// <para>GetPlayStatus</para>
    /// <para>操作ミノの操作状況を検査します</para>
    /// </summary>
    /// <returns>操作判定 -1:プレイ不可能 0:操作中 1:操作終了</returns>
    public int GetPlayStatus()
    {
        //プレイが可能ではない
        if (!_canPlay)
        {
            //プレイが不可能であることを返す
            return GAMEEND_ID;
        }

        //設置した回数 が 操作可能なミノの最大数 より多いか
        if(MAX_COMMITMINO_CNT <= _commitCnt)
        {
            //カウントリセット
            _commitCnt = 0;
            //設置が完了していることを返す
            return COMMIT_ID;
        }

        //まだ操作している
        return PLAYING_ID;
    }
    
    //インターフェイス継承
    public bool CheckAlreadyMinoExist(int x,int y)
    {
        //フィールド外(上下左右) または 既にミノが存在する
        if (x < 0 || y < 0 || FIELD_MAX_WIDTH <= x || FIELD_MAX_HEIGHT <= y || _field[y, x] == TILE_MINO_ID) { return true; /*空白ではない*/ }

        //空白である
        return false;
    }

    //インターフェイス継承
    public void SetMino(int x,int y)
    {
        //ミノ設定
        _field[y, x] = TILE_MINO_ID;
        //コミットした操作ミノの数を増分
        _commitCnt++;

        //操作分のコミットがされた時、ラインチェックをする
        if(MAX_COMMITMINO_CNT <= _commitCnt)
        {
            CheckLine();
        }
        return;
    }

    //インターフェイス継承
    public void NotPlayable()
    {
        _canPlay = false;
        return;
    }
    #endregion

    #region デバッグ用メソッド
    /// <summary>
    /// <para>RemoveField</para>
    /// <para>フィールドを初期化します</para>
    /// </summary>
    public void RemoveField()
    {
        //4ラインずつ消去する
        int allLine = FIELD_MAX_HEIGHT / TETRIS_LINE;
        //消去ライン設定
        _deleteLineIndexs.Clear();
        _deleteLineIndexs.Add(0);
        _deleteLineIndexs.Add(1);
        _deleteLineIndexs.Add(2);
        _deleteLineIndexs.Add(3);
        //ミノ削除
        foreach (ILineMinoCtrl lineMino in FindObjectsOfType<Mino>())
        {
            for(int i = 0;i <= allLine; i++)
            {
                lineMino.LineCtrl(_deleteLineIndexs);
            }
        }
        //フィールド初期化
        for(int y = 0; y < FIELD_MAX_HEIGHT; y++)
        {
            for(int x = 0; x < FIELD_MAX_WIDTH; x++)
            {
                _field[y, x] = TILE_NONE_ID;
            }
        }
    }

    /// <summary>
    /// <para>LevelUp</para>
    /// <para>レベルを１上げます</para>
    /// </summary>
    public void LevelUp()
    {
        _scoreManager.AddScore(_scoreManager.LevelBorder);
    }

    /// <summary>
    /// <para>DeleteCommit</para>
    /// <para>コミット処理を無効にします</para>
    /// </summary>
    public void DeleteCommit()
    {
        _commitCnt = 0;
    }
    #endregion
}
