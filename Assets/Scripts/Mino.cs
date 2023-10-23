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
    private int _deleteLineIndex = default;

    private MinoPoolManager minoManager = default;
    private SpriteRenderer _myRen = default;
    private Transform _myTrans = default;
    #endregion

    #region プロパティ
    public int MinoX { get => Mathf.RoundToInt(_myTrans.position.x); }
    public int MinoY { get => Mathf.RoundToInt(_myTrans.position.y); }
    
    #endregion

    #region メソッド
    /// <summary>
    /// 初期化処理
    /// </summary>
    void Awake()
    {
        //初期化
        minoManager = FindObjectOfType<MinoPoolManager>().GetComponent<MinoPoolManager>();
        _myTrans = transform;
        _myRen = GetComponent<SpriteRenderer>();
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
        //向き初期化
        if(_myTrans.eulerAngles.z != 0) { _myTrans.eulerAngles = Vector3.zero; }
    }

    //インターフェイス継承
    public void ChangeColor(Color minoColor)
    {
        _myRen.color = minoColor;
        return;
    }

    //インターフェイス継承
    public void SetMinoPos(float x, float y, Transform parent)
    {
        //軸を中心に座標調整
        _myTrans.position = parent.position + Vector3.right * x + Vector3.up * y;
        //軸を親に設定
        _myTrans.parent = parent;
    }

    //インターフェイス継承
    public void DisConnect()
    {
        //親子関係削除
        _myTrans.parent = null;
        //座標正規化
        _myTrans.position =
            Vector3.right * Mathf.RoundToInt(_myTrans.position.x) +
            Vector3.up * Mathf.RoundToInt(_myTrans.position.y);
    }

    //インターフェイス継承
    public void LineCtrl(List<int> deleteLineHeights)
    {
        //ホールド中のミノは無視する
        if (_myTrans.parent != null) { return; }

        //削除対象のラインにある場合、削除する
        if (deleteLineHeights.Contains(MinoY)) { DeleteMino(); }

        //削除対象のラインに応じて、落下処理を行います
        for (_deleteLineIndex = 0; _deleteLineIndex < deleteLineHeights.Count; _deleteLineIndex++)
        {
            //現在の落下対象ライン以下 かつ １番下層のラインである
            if(MinoY <= deleteLineHeights[_deleteLineIndex] && _deleteLineIndex == 0)
            {
                //何もしない
                return;
            }

            //現在の落下対象ライン以下である（１番下層のラインではない）
            if(MinoY <= deleteLineHeights[_deleteLineIndex])
            {
                DownMino(_deleteLineIndex);
                return;
            }

            //現在の落下対象ラインより上
            if(deleteLineHeights[_deleteLineIndex] < MinoY)
            {
                //次の落下対象ラインと比較
                continue;
            }
        }
        //全ての落下対象ラインより上にある
        DownMino(_deleteLineIndex);
        return;
    }

    /// <summary>
    /// <para>DownMino</para>
    /// <para>ミノを落下させます</para>
    /// </summary>
    /// <param name="value">落下距離</param>
    private void DownMino(int value)
    {
        _myTrans.position += Vector3.down * value;
        return;
    }

    /// <summary>
    /// <para>DeleteMino</para>
    /// <para>ミノを削除します</para>
    /// </summary>
    private void DeleteMino()
    {
        minoManager.EndUseableMino(gameObject);
        return;
    }
    #endregion
}
