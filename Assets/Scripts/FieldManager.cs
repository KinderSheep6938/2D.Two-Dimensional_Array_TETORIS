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
    const int FIELD_MAX_HEIGHT = 20; //フィールドの縦幅
    const int TILE_NONE_ID = 0; //フィールドの空白ID
    const int TILE_MINO_ID = 1; //ミノID

    private int[,] _field = new int[FIELD_MAX_HEIGHT,FIELD_MAX_WIDTH]; //フィールド保存 [縦軸:y,横軸:x]

    #endregion

    #region メソッド
    /// <summary>
    /// 初期化処理
    /// </summary>
    void Awake()
    {
        //マップ初期化
        for(int y = 0; y < FIELD_MAX_HEIGHT; y++) //縦軸
        { 
            for(int x = 0; x < FIELD_MAX_WIDTH; x++) //横軸
            { 
                //空白で初期化
                _field[y, x] = TILE_NONE_ID;
            }
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
    /// <para>CheckAlreadyMinoExist</para>
    /// <para>指定した座標が空白かどうか検査します</para>
    /// </summary>
    /// <param name="x">検査座標の横軸</param>
    /// <param name="y">検査座標の縦軸</param>
    /// <returns>空白状況</returns>
    public bool CheckAlreadyMinoExist(int x,int y)
    {
        //フィールド外(上下左右) または 既にミノが存在する
        if (x < 0 || y < 0 || FIELD_MAX_WIDTH <= x || FIELD_MAX_HEIGHT <= y || _field[y, x] == TILE_MINO_ID) { return true; /*空白ではない*/ }

        //空白である
        return false;
    }

    /// <summary>
    /// <para>SetMino</para>
    /// <para>指定した座標にミノを設定します</para>
    /// </summary>
    /// <param name="x">設定座標の横軸</param>
    /// <param name="y">設定座標の縦軸</param>
    public void SetMino(int x,int y)
    {
        //ミノ設定
        _field[y, x] = TILE_MINO_ID;
        return;
    }
    #endregion
}
