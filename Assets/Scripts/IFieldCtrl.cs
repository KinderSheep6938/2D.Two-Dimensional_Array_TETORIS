// -------------------------------------------------------------------------------
// IFieldCtrl.Interface
//
// 作成日: 2023/10/17
// 作成者: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFieldCtrl
{
    bool CheckAlreadyMinoExist(int x, int y);
    void SetMino(int x,int y);
}
