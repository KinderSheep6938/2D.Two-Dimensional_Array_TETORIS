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
    private const int MAX_USEMINO_CNT = 4; //最大使用ミノブロック数
    
    [SerializeField,Header("ミノブロック")]
    private GameObject _minoBlock = default;
    [SerializeField, Header("事前生成ミノ数")]
    private int _poolValue = 150;
    private List<IMinoBlockAccessible> _minoPool = new List<IMinoBlockAccessible>(); //プール
    private IMinoBlockAccessible[] _useableMinos = new IMinoBlockAccessible[MAX_USEMINO_CNT]; //使用可能ミノブロック
    #endregion

    #region メソッド
    /// <summary>
    /// 初期化処理
    /// </summary>
    void Awake()
    {
        //初期化
        //非表示で生成する
        _minoBlock.GetComponent<IMinoBlockAccessible>().SetMinoView(false);
        //プール初期化
        _minoPool.Clear();
        //指定数までプールを貯める
        while(_minoPool.Count < _poolValue)
        {
            //生成しプール設定
            _minoPool.Add(Instantiate(_minoBlock).GetComponent<IMinoBlockAccessible>());
        }

        //表示状態に設定する
        _minoBlock.GetComponent<IMinoBlockAccessible>().SetMinoView(true);
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
        for(int i = 0; i < MAX_USEMINO_CNT; i++)
        {
            //プールに使用可能ミノがあるか
            if(0 < _minoPool.Count)
            {
                //表示する
                _minoPool[0].SetMinoView(true);
                //プールから取得
                _useableMinos[i] = _minoPool[0];
                _minoPool.RemoveAt(0);
            }
            else
            {   //ない場合は新しく生成する
                _useableMinos[i] = Instantiate(_minoBlock).GetComponent<IMinoBlockAccessible>();
            }

        }
        //使用可能なミノブロックを送信
        return _useableMinos;
    }

    /// <summary>
    /// <para>EndUseableMino</para>
    /// <para>ObjectPoolに使用終了したミノブロックを返却します</para>
    /// </summary>
    /// <param name="unUseableMino">使用終了したミノブロック</param>
    public void EndUseableMino(IMinoBlockAccessible unUseableMino)
    {
        //非表示に設定
        unUseableMino.SetMinoView(false);
        //プールに登録
        _minoPool.Add(unUseableMino);

        return;
    }
    #endregion
}
