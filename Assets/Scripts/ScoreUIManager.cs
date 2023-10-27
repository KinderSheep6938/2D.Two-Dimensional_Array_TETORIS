// -------------------------------------------------------------------------------
// ScoreUIManager.cs
//
// 作成日: 2023/10/25
// 作成者: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUIManager : MonoBehaviour
{
    #region 変数
    [SerializeField,Header("スコア表示テキスト")]
    private Text _scoreText = default;
    [SerializeField,Header("レベル表示テキスト")]
    private Text _levelText = default;

    private ScoreManager _scoreManager = default; //スコア管理クラス
    #endregion

    #region メソッド
    /// <summary>
    /// 初期化処理
    /// </summary>
    void Awake()
    {
        _scoreManager = FindObjectOfType<ScoreManager>();
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        //スコア表示
        SetTextToScore();
    }

    /// <summary>
    /// <para>OutputDisplay</para>
    /// <para>スコアを表示されているテキストに設定します</para>
    /// </summary>
    private void SetTextToScore()
    {
        _scoreText.text = _scoreManager.Score.ToString();
        _levelText.text = _scoreManager.Level.ToString();
    }

    
    #endregion
}
