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
    #region メソッド
    /// <summary>
    /// 初期化処理
    /// </summary>
    void Awake()
    {
        //軸に子付け
        Minos = GetComponentsInChildren<IMinoBlockAccessible>();
    }

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
    public void ChangeModelGhost(IMinoCreatable.MinoType playableMino, Vector3 minoParentPos)
    {
        //操作中のミノの形をゴーストに反映させる
        CreateMinoUnit(Minos, playableMino);
        //操作中のミノの位置をゴーストに反映させる
        MyTransform.position = minoParentPos;

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

        //衝突判定があるか
        if (CheckMinoCollision(0, -1))
        {
            return; //再帰処理終了
        }

        //1マス落とす
        MyTransform.position += Vector3.down;

        //衝突判定があるまで再帰処理
        Drop(); //再帰
        return;
    }

    public override void CreateMinoUnit(IMinoBlockAccessible[] minoBlocks, IMinoCreatable.MinoType setModel)
    {
        base.CreateMinoUnit(minoBlocks, setModel);

        foreach(SpriteRenderer minoSprite in GetComponentsInChildren<SpriteRenderer>())
        {
            minoSprite.color = Color.white;
        }
    }
    #endregion
}
