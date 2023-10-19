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
    const string OBJECTPOOL_SYSTEM_TAG = "ObjectPool";

    private MinoPoolManager minoManager = default;
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
        //初期化
        minoManager = GameObject.FindGameObjectWithTag(OBJECTPOOL_SYSTEM_TAG).GetComponent<MinoPoolManager>();
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
    public void DisConnectParent()
    {
        //親子関係削除
        _myTrans.parent = null;
    }

    //インターフェイス継承
    public void DownMino()
    {
        _myTrans.position += Vector3.down;
        return;
    }

    //インターフェイス継承
    public void DeleteMino()
    {
        minoManager.EndUseableMino(gameObject);
        return;
    }
    #endregion
}
