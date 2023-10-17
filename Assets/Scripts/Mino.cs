// -------------------------------------------------------------------------------
// Mino.cs
//
// 作成日: 2023/10/17
// 作成者: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mino : MonoBehaviour, IMinoInfo, ILineMinoCtrl
{
    #region 変数
    private int _minoX = 0; //X座標
    private int _minoY = 0; //Y座標
    private bool _isCommit = false;

    private SpriteRenderer _myRen = default;
    private Transform _myTrans = default;
    #endregion

    #region プロパティ
    public int MinoX { get => _minoX; set => _minoX = value; }
    public int MinoY { get => _minoY; set => _minoY = value; }
    
    #endregion

    #region メソッド
    /// <summary>
    /// 初期化処理
    /// </summary>
    void Awake()
    {

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
    /// <para>ChangeColor</para>
    /// <para>ミノの色を設定します</para>
    /// </summary>
    /// <param name="minoColor">設定される色</param>
    public void ChangeColor(Color minoColor)
    {
        _myRen.color = minoColor;
        return;
    }

    /// <summary>
    /// <para>DownMino</para>
    /// <para>ミノを1列分下げます</para>
    /// </summary>
    public void DownMino()
    {
        _myTrans.position += Vector3.down;
        return;
    }

    /// <summary>
    /// <para>DeleteMino</para>
    /// <para>ミノを削除します</para>
    /// </summary>
    public void DeleteMino()
    {
        //ミノ削除処理
        return;
    }
    #endregion
}
