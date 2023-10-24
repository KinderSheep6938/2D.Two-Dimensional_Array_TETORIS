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
    //リスト管理定数
    private const int FIELD_MAX_WIDTH = 10; //フィールドの横幅
    private const int FIELD_MAX_HEIGHT = 25; //フィールドの縦幅
    private const int FIELD_VIEW_HEIGHT = 21; //フィールドの最大表示縦幅
    private const int TILE_NONE_ID = 0; //フィールドの空白ID
    private const int TILE_MINO_ID = 1; //ミノID
    private const int MAX_COMMITMINO_CNT = 4;

    [SerializeField]
    private GameObject _fieldTileObj = default; //空白タイル

    private int[,] _field = new int[FIELD_MAX_HEIGHT,FIELD_MAX_WIDTH]; //フィールド保存 [縦軸:y,横軸:x]
    private List<int> _deleteLineIndexs = new(); //完成ラインのIndex保存用
    private bool _isLine = false; //完成ライン判定
    private int _fallValue = 0; //ライン削除の落下距離
    private int _commitCnt = 0; //設置した操作ミノブロックの数



    private Transform _transform = default;
    #endregion

    #region メソッド
    /// <summary>
    /// 初期化処理
    /// </summary>
    void Awake()
    {
        //変数初期化
        _transform = transform;

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
                        _transform
                        );
                }
            }
        }
        //ライン検査初期化
        _deleteLineIndexs.Clear();
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
    /// <para>CheckLine</para>
    /// <para>フィールドの中でラインができているか検査します</para>
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

        //削除対象が一つ以上ある場合は削除処理を行う
        if(_deleteLineIndexs.Count != 0) { DeleteLine(); }

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
        Debug.Log("------------");
        return;
    }

    /// <summary>
    /// <para>GetCommitStatus</para>
    /// <para>操作中のミノが設置されたかどうか検査します</para>
    /// </summary>
    /// <returns>操作完了判定</returns>
    public bool GetCommitStatus()
    {
        //設置した回数 が 操作可能なミノの最大数 より多いか
        if(MAX_COMMITMINO_CNT <= _commitCnt)
        {
            //カウントリセット
            _commitCnt = 0;
            //設置が完了していることを返す
            return true;
        }

        //まだ操作している
        return false;
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
    #endregion
}
