// -------------------------------------------------------------------------------
// NextMinoView.cs
//
// 作成日: 2023/10/24
// 作成者: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextMino : MinoModelGeneration
{
    #region 変数
    private IMinoCreatable _playableMino = default;
    private bool isSet = false;
    #endregion

    #region メソッド
    /// <summary>
    /// 初期化処理
    /// </summary>
    void Awake()
    {
        //初期化
        _playableMino = transform.parent.GetComponentInChildren<PlayableMino>().GetComponent<IMinoCreatable>();
    }

    //クラス継承
    public override void CreateMinoUnit(IMinoBlockAccessible[] minoBlocks, IMinoCreatable.MinoType setModel)
    {
        if(isSet)
        {
            _playableMino.CreateMinoUnit(Minos, MyModel);
        }

        base.CreateMinoUnit(minoBlocks, setModel);
        isSet = true;
    }
    #endregion
}
