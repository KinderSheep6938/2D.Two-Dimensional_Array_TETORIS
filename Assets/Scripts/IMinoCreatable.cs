// -------------------------------------------------------------------------------
// IMinoUnionCtrl.Interface
//
// 作成日: 2023/10/18
// 作成者: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMinoCreatable
{
    enum MinoType
    {
        minoO,
        minoS,
        minoZ,
        minoJ,
        minoL,
        minoT,
        minoI
    }
    MinoType MyModel { get; set; }
    void CreateMinoUnit(IMinoInfo[] minoBlocks,MinoType setModel);
    void SetUnionPlayable();
}
