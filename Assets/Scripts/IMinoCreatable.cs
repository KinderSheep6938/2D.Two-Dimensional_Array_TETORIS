// -------------------------------------------------------------------------------
// IMinoUnionCtrl.Interface
//
// çÏê¨ì˙: 2023/10/18
// çÏê¨é“: Shizuku
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
