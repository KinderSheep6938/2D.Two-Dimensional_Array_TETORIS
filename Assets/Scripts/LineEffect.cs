// -------------------------------------------------------------------------------
// LineEffect.cs
//
// 作成日: 2023/10/25
// 作成者: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineEffect : MonoBehaviour
{
    #region 変数
    private readonly Vector3 _effectWidthPos = new Vector3(4.5f, 0, 0); //エフェクト発生位置の横軸

    private Transform _transform = default; //自身のTransform
    private ParticleSystem _particleSystem = default; //自身のParticleSystem
    #endregion

    #region メソッド
    /// <summary>
    /// 初期化処理
    /// </summary>
    void Awake()
    {
        _transform = transform;
        _particleSystem = _transform.GetComponent<ParticleSystem>();
    }

    /// <summary>
    /// <para>SetEffect</para>
    /// <para>指定した列にエフェクトを発生させます</para>
    /// </summary>
    /// <param name="line"></param>
    public void SetEffect(int line)
    {
        //座標設定
        _transform.position = _effectWidthPos + Vector3.up * line;
        //パーティクル開始
        _particleSystem.Play();
    }
    #endregion
}
