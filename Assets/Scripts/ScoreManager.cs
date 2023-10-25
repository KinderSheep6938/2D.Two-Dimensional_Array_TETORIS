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
    private const int MINFALLTIME_LEVEL = 20; //最速落下時間のレベル
    private const int BASE_SCORE = 100; //基礎スコア
    private const float BASE_FALLTIME = 0.8f; //基礎落下時間
    private const float MAXLEVEL_FALLTIME = 0.03f; //最大レベル落下時間
    //レベル上昇時に減少する落下時間
    //減少時間 = (基礎落下時間 - 最大レベル落下時間) / 最大レベル
    private readonly float _levelUpRatio = (BASE_FALLTIME - MAXLEVEL_FALLTIME) / MINFALLTIME_LEVEL;

    private int _score = 0; //現在スコア
    private int _clearLine = 0; //削除したラインの値
    private int _oldLevel = 0; //レベル上昇検査用


    [SerializeField, Tooltip("操作ミノ")]
    private PlayableMino _playableMino = default;
    #endregion

    #region プロパティ
    public int Level { get => 1 + (_clearLine / LEVELUP_BORDERLINE); }
    public int Score { get => _score; }
    #endregion

    #region メソッド
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

        //レベルが変更された または レベルが最速落下時間のレベルより下である
        if(_oldLevel != Level || Level <= MINFALLTIME_LEVEL)
        {
            _oldLevel = Level; //現在のレベルを保存
            _playableMino.FallTime = LevelOfFallTime(); //速度設定
        }
    }

    private float LevelOfFallTime()
    {
        if(Level < MINFALLTIME_LEVEL)
        {
            return BASE_FALLTIME - _levelUpRatio * Level;
        }
        return MAXLEVEL_FALLTIME;
    }
    #endregion
}
