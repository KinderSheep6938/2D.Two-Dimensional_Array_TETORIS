// -------------------------------------------------------------------------------
// IMinoUnionCtrl.Interface
//
// ì¬“ú: 2023/10/19
// ì¬Ò: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMinoUnionCtrl
{
    /// <summary>
    /// <para>Move</para>
    /// <para>ƒ~ƒm‚ğˆÚ“®‚³‚¹‚Ü‚·</para>
    /// </summary>
    /// <param name="x">ˆÚ“®•ûŒü</param>
    void Move(int x);

    /// <summary>
    /// <para>Rotate</para>
    /// <para>ƒ~ƒm‚ğ‰ñ“]‚³‚¹‚Ü‚·</para>
    /// </summary>
    /// <param name="angle">‰ñ“]•ûŒü</param>
    void Rotate(int angle);

    /// <summary>
    /// <para>HardDrop</para>
    /// <para>ƒ~ƒm‚ğ‹}~‰º‚³‚¹‚Ü‚·</para>
    /// </summary>
    void HardDrop();

    /// <summary>
    /// <para>SoftDrop</para>
    /// <para>ƒ~ƒm‚ğ‘f‘‚­—‰º‚³‚¹‚Ü‚·</para>
    /// </summary>
    void SoftDrop();

    /// <summary>
    /// <para>MinoHold</para>
    /// <para>ƒ~ƒm‚ğƒz[ƒ‹ƒhİ’è‚µ‚Ü‚·</para>
    /// </summary>
    bool CheckHasHold();
}
