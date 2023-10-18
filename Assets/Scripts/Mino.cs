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
    private bool _isCommit = false;

    private SpriteRenderer _myRen = default;
    private Transform _myTrans = default;
    #endregion

    #region プロパティ
    public int MinoX { get => (int)_myTrans.position.x; }
    public int MinoY { get => (int)_myTrans.position.y; }
    
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
    /// <para>SetMinoPos</para>
    /// <para>ミノブロックを指定した値分移動します</para>
    /// <para>また、指定したTransformを親（ミノ軸）として設定します</para>
    /// </summary>
    /// <param name="x">移動する横軸</param>
    /// <param name="y">移動する縦軸</param>
    /// <param name="parent">ミノ軸</param>
    public void SetMinoPos(float x, float y, Transform parent)
    {
        //軸を中心に座標調整
        _myTrans.position = parent.position + Vector3.right * x + Vector3.up * y;
        //軸を親に設定
        _myTrans.parent = parent;
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
