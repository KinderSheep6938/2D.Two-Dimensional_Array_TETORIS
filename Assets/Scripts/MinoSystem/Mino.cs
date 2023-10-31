// -------------------------------------------------------------------------------
// Mino.cs
//
// 作成日: 2023/10/17
// 作成者: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mino : MonoBehaviour, IMinoBlockAccessible, ILineMinoCtrl
{
    #region 変数
    private int _deleteLineCnt = default; //削除ラインカウント

    private MinoPoolManager minoManager = default; //ミノ管理マネージャー
    private SpriteRenderer _myRen = default; //自身のSpriteRenderer
    private Transform _transform = default; //自身のTransform
    private IMinoBlockAccessible _myBlock = default; //自身のブロック情報
    #endregion

    #region プロパティ
    public int MinoX { get => Mathf.RoundToInt(_transform.position.x); }
    public int MinoY { get => Mathf.RoundToInt(_transform.position.y); }
    #endregion

    #region メソッド
    /// <summary>
    /// 初期化処理
    /// </summary>
    void Awake()
    {
        //初期化
        minoManager = FindObjectOfType<MinoPoolManager>().GetComponent<MinoPoolManager>();
        _transform = transform;
        _myRen = GetComponent<SpriteRenderer>();
        _myBlock = GetComponent<IMinoBlockAccessible>();
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        //向き初期化
        if(_transform.eulerAngles.z != 0) { _transform.eulerAngles = Vector3.zero; }
    }

    //インターフェイス継承
    public void ChangeColor(Color minoColor)
    {
        //色変更
        _myRen.color = minoColor;
        return;
    }

    //インターフェイス継承
    public void SetMinoPos(float x, float y, Transform parent)
    {
        //軸を中心に座標調整
        _transform.position = parent.position + Vector3.right * x + Vector3.up * y;
        //軸を親に設定
        _transform.parent = parent;
        //表示
        _myRen.enabled = true;
    }

    //インターフェイス継承
    public void DisConnect()
    {
        //親子関係削除
        _transform.parent = null;
        //座標正規化
        _transform.position =
            Vector3.right * Mathf.RoundToInt(_transform.position.x) +
            Vector3.up * Mathf.RoundToInt(_transform.position.y);
    }
    
    //インターフェイス継承
    public void SetMinoView(bool canShow)
    {
        //表示
        if (canShow)
        {
            gameObject.SetActive(true);
        }
        else
        { //非表示
            gameObject.SetActive(false);
        }
    }

    //インターフェイス継承
    public void LineCtrl(List<int> deleteLineHeights)
    {
        //ホールド中のミノ または 非表示のミノ は無視する
        if (_transform.parent != null || !_myRen.enabled ) { return; }

        //削除対象のラインにある場合、削除する
        if (deleteLineHeights.Contains(MinoY)) 
        {
            DeleteMino();
            return;
        }

        //削除対象のラインに応じて、落下処理を行います
        for (_deleteLineCnt = 0; _deleteLineCnt < deleteLineHeights.Count; _deleteLineCnt++)
        {
            //現在の落下対象ライン以下 かつ １番下層のラインである
            if(MinoY <= deleteLineHeights[_deleteLineCnt] && _deleteLineCnt == 0)
            {
                //何もしない
                return;
            }

            //現在の落下対象ライン以下である（１番下層のラインではない）
            if(MinoY <= deleteLineHeights[_deleteLineCnt])
            {
                DownMino(_deleteLineCnt);
                return;
            }

            //現在の落下対象ラインより上
            if(deleteLineHeights[_deleteLineCnt] < MinoY)
            {
                //次の落下対象ラインと比較
                continue;
            }
        }
        //全ての落下対象ラインより上にある
        DownMino(_deleteLineCnt);
        return;
    }

    /// <summary>
    /// <para>DownMino</para>
    /// <para>ミノを落下させます</para>
    /// </summary>
    /// <param name="value">落下距離</param>
    private void DownMino(int value)
    {
        _transform.position += Vector3.down * value;
        return;
    }

    /// <summary>
    /// <para>DeleteMino</para>
    /// <para>ミノを削除します</para>
    /// </summary>
    private void DeleteMino()
    {
        _myRen.enabled = false;
        minoManager.EndUseableMino(_myBlock);
        return;
    }
    #endregion
}
