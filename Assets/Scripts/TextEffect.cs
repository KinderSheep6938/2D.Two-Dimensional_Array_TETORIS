// -------------------------------------------------------------------------------
// TetrisEffect.cs
//
// 作成日: 2023/10/26
// 作成者: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextEffect : MonoBehaviour
{
    #region 変数
    const float MAX_VIEW_TIME = 2.5f; //テキストの表示時間

    private float _viewTimer = 0f; //表示カウントタイマー
    private bool _nowShow = false; //表示判定フラグ
    private Text _viewText = default; //表示テキスト
    #endregion

    #region メソッド
    /// <summary>
    /// 初期化処理
    /// </summary>
    void Awake()
    {
        _viewText = GetComponent<Text>();
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        //表示判定がある場合は、表示管理を行う
        if (_nowShow) { ViewCtrl(); }
    }

    /// <summary>
    /// <para>ViewCtrl</para>
    /// <para>表示管理をします</para>
    /// </summary>
    private void ViewCtrl()
    {
        //タイマー加算
        _viewTimer += Time.deltaTime;

        //表示時間を満たした
        if (MAX_VIEW_TIME <= _viewTimer)
        {
            //表示判定を不可視に
            _nowShow = false;
            //非表示に設定
            _viewText.enabled = false;

        }
    }

    /// <summary>
    /// <para>SetView</para>
    /// <para>表示状態に設定します</para>
    /// </summary>
    public void SetView()
    {
        //表示判定を可視に
        _nowShow = true;

        //表示に設定
        _viewText.enabled = true;
        //タイマー初期化
        _viewTimer = 0;
    }
    #endregion
}
