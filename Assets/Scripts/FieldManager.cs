// -------------------------------------------------------------------------------
// FieldManager.cs
//
// 作成日: 2023/10/17
// 作成者: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour, IFieldCtrl
{
    #region 変数
    //リスト管理定数
    const int FIELD_MAX_WIDTH = 10; //フィールドの横幅
    const int FIELD_MAX_HEIGHT = 25; //フィールドの縦幅
    const int FIELD_VIEW_HEIGHT = 20; //フィールドの最大表示縦幅
    const int TILE_NONE_ID = 0; //フィールドの空白ID
    const int TILE_MINO_ID = 1; //ミノID

    [SerializeField]
    private GameObject _fieldTileObj = default; //空白タイル

    private int[,] _field = new int[FIELD_MAX_HEIGHT,FIELD_MAX_WIDTH]; //フィールド保存 [縦軸:y,横軸:x]
    private bool[] _lines = new bool[FIELD_MAX_HEIGHT]; //フィールドのライン検査リスト [縦軸:y]

    private Transform _myTrans = default;
    #endregion

    #region メソッド
    /// <summary>
    /// 初期化処理
    /// </summary>
    void Awake()
    {
        //変数初期化
        _myTrans = transform;

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
                        _myTrans
                        );
                }
            }
            //ライン検査初期化
            _lines[y] = false;

        }
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
        //ライン調査   
    }

    /// <summary>
    /// <para>DeleteLine</para>
    /// <para>指定したラインを削除します</para>
    /// </summary>
    private void DeleteLine()
    {
        //ライン消去
    }
    

    public bool CheckAlreadyMinoExist(int x,int y)
    {
        //フィールド外(上下左右) または 既にミノが存在する
        if (x < 0 || y < 0 || FIELD_MAX_WIDTH <= x || FIELD_MAX_HEIGHT <= y || _field[y, x] == TILE_MINO_ID) { return true; /*空白ではない*/ }

        //空白である
        return false;
    }


    public void SetMino(int x,int y)
    {
        //ミノ設定
        _field[y, x] = TILE_MINO_ID;
        return;
    }
    #endregion
}
