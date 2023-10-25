// -------------------------------------------------------------------------------
// MinoPoolManager.cs
//
// 作成日: 2023/10/19
// 作成者: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinoPoolManager : MonoBehaviour
{
    #region 変数
    private const int MAX_MINO_CNT = 4;
    

    [SerializeField,Tooltip("ミノブロック")]
    private GameObject _minoBlock = default;
    private IMinoBlockAccessible[] _useableMinos = new IMinoBlockAccessible[MAX_MINO_CNT];
    #endregion

    #region プロパティ

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
    /// <para>GetUseableMino</para>
    /// <para>ObjectPoolから使用可能なミノブロックを取得します</para>
    /// </summary>
    /// <returns>使用可能なミノブロック</returns>
    public IMinoBlockAccessible[] GetUseableMino()
    {
        //現在はInstantiate

        //使用可能なミノブロックを取得
        for(int i = 0; i < MAX_MINO_CNT; i++)
        {
            _useableMinos[i] = Instantiate(_minoBlock).GetComponent<IMinoBlockAccessible>();
        }
        //使用可能なミノブロックを送信
        return _useableMinos;
    }

    /// <summary>
    /// <para>EndUseableMino</para>
    /// <para>ObjectPoolに使用終了したミノブロックを返却します</para>
    /// </summary>
    /// <param name="unUseableMino">使用終了したミノブロック</param>
    public void EndUseableMino(GameObject unUseableMino)
    {
        //現在はDestroy

        //オブジェクト破棄
        Destroy(unUseableMino);
        return;
    }
    #endregion
}
