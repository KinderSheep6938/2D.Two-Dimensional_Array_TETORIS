// -------------------------------------------------------------------------------
// ScoreManager.cs
//
// 作成日: 2023/10/24
// 作成者: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    #region 変数
    private const int LEVELUP_BORDERLINE = 10; //レベル上昇のしきい値
    private const int BASE_SCORE = 100; //基礎スコア

    private int _score = 0; //現在スコア
    private int _clearLine = 0; //削除したラインの値


    #endregion

    #region プロパティ
    public int Level { get => _clearLine / LEVELUP_BORDERLINE; }
    public int Score { get => _score; }
    #endregion

    #region メソッド
    /// <summary>
    /// 初期化処理
    /// </summary>
    void Awake()
    {
        
    }

    /// <summary>
    /// <para>AddScore</para>
    /// <para>消したラインに応じて、スコアを加算します</para>
    /// </summary>
    /// <param name="deleteLine">削除したライン</param>
    public void AddScore(int deleteLine)
    {
        //スコア加算
        _score += deleteLine * deleteLine * Level * BASE_SCORE;

        //消したラインを加算
        _clearLine += deleteLine;
    }
    #endregion
}
