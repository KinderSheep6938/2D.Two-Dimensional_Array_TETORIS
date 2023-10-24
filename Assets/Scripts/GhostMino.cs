// -------------------------------------------------------------------------------
// GhostMino.cs
//
// 作成日: 2023/10/24
// 作成者: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GhostMino : AccessibleToField, IGhostStartable
{
    #region 変数

    #endregion

    #region プロパティ

    #endregion

    #region メソッド
    //インターフェイス継承
    public void ChangeTransformGhost(Transform playableMino)
    {
        //操作中のミノの位置、角度をゴーストに反映させる
        MyTransform.position = playableMino.position;
        MyTransform.rotation = playableMino.rotation;
        //ゴーストを落とす
        Drop();
    }

    //インターフェイス継承
    public void ChangeModelGhost(IMinoCreatable.MinoType playableMino)
    {
        //操作中のミノの形をゴーストに反映させる
        if(Minos.Length != 0) //ゴーストのミノが設定されているか
        {
            //ゴースト形成
            CreateMinoUnit(Minos, playableMino);
        }
        else //設定されていない
        {
            //子付けされているミノブロックを設定し、ゴースト形成
            CreateMinoUnit(GetComponentsInChildren<IMinoBlockAccessible>(), playableMino);
        }

        //ゴースト落とす
        Drop();
    }

    /// <summary>
    /// <para>Drop</para>
    /// <para>ミノを急降下させます</para>
    /// </summary>
    private void Drop()
    {
        //再帰処理

        //1マス落とす
        MyTransform.position += Vector3.down;

        //衝突判定があるか
        if (CheckMino())
        {
            //１マス戻す
            MyTransform.position += Vector3.up;
            return; //再帰処理終了
        }

        //衝突判定があるまで再帰処理
        Drop(); //再帰
        return;
    }

    public override void CreateMinoUnit(IMinoBlockAccessible[] minoBlocks, IMinoCreatable.MinoType setModel)
    {
        base.CreateMinoUnit(minoBlocks, setModel);

        foreach(IMinoBlockAccessible mino in minoBlocks)
        {
            
        }
    }
    #endregion
}
