// -------------------------------------------------------------------------------
// HoldMino.cs
//
// 作成日: 2023/10/22
// 作成者: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldMino : MinoModelGeneration, IMinoHoldable
{
    #region 変数
    private IMinoCreatable.MinoType _holdModel = default; //ホールド切り替え退避用変数

    private IMinoBlockAccessible[] _holdMinos = new IMinoBlockAccessible[IMinoCreatable.MAX_MINO_CNT]; //表示ミノ
    private bool _hasHold = false; //ホールド所持判定

    private IMinoCreatable _playerMino = default; //操作ミノ
    private MinoFactory _minoFactory = default; //ミノ生成マネージャー
    #endregion

    #region メソッド
    /// <summary>
    /// 初期化処理
    /// </summary>
    void Awake()
    {
        //初期化
        _playerMino = transform.parent.GetComponentInChildren<PlayableMino>().GetComponent<IMinoCreatable>();
        _minoFactory = GetComponentInParent<MinoFactory>();
    }

    // クラス継承
    public override void CreateMinoUnit(IMinoBlockAccessible[] minoBlocks, IMinoCreatable.MinoType setModel)
    {
        //基底メソッドを使用
        base.CreateMinoUnit(minoBlocks, setModel);
        return;
    }

    //インターフェイス継承
    void IMinoHoldable.Hold()
    {
        Debug.Log("Hold");
        //保持しているモデルを退避
        _holdModel = MyModel;
        _holdMinos = Minos;
        //ホールド対象のミノ形に変更する
        //引数としてnullを渡しているのは現在保持しているミノブロックを使いまわすため
        Debug.Log(_playerMino.MyModel);
        CreateMinoUnit(_playerMino.Minos, _playerMino.MyModel);
        
        //ホールドしているミノがない
        if (!_hasHold)
        {
            //新しくミノを生成する
            _minoFactory.CreateMino();
            //ホールドフラグをたてる
            _hasHold = true;
            return;
        }

        //ホールドしているミノを操作ミノに反映させる
        _playerMino.CreateMinoUnit(_holdMinos, _holdModel);
        return;
    }
    #endregion
}
