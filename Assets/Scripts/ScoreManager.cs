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


    [SerializeField, Header("操作ミノ")]
    private PlayableMino _playableMino = default;
    #endregion

    #region プロパティ
    public int Level { get => 1 + (_clearLine / LEVELUP_BORDERLINE); }
    public int Score { get => _score; }

    public int LevelBorder { get => LEVELUP_BORDERLINE; }
    #endregion

    #region メソッド
    /// <summary>
    /// 初期化処理
    /// </summary>
    void Awake()
    {
        //初期速度設定
        _playableMino.FallTime = LevelOfFallTime();
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

        //レベルが変更された または レベルが最速落下時間のレベルより下である
        if(_oldLevel != Level || Level <= MINFALLTIME_LEVEL)
        {
            _oldLevel = Level; //現在のレベルを保存
            _playableMino.FallTime = LevelOfFallTime(); //速度設定
        }
    }

    /// <summary>
    /// <para>LevelOfFallTime</para>
    /// <para>レベルに応じた落下時間を取得する</para>
    /// </summary>
    /// <returns>レベルに対応した落下時間</returns>
    private float LevelOfFallTime()
    {
        //レベルが最小落下時間レベル以下である
        if(Level < MINFALLTIME_LEVEL)
        {
            //対応したレベルの落下時間を返す
            //対応した落下時間 ＝ 基礎落下時間 - (レベル毎の減少時間量 * 現在のレベル)
            return BASE_FALLTIME - _levelUpRatio * Level;
        }

        //最小落下時間を返す
        return MAXLEVEL_FALLTIME;
    }
    #endregion
}
