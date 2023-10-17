// -------------------------------------------------------------------------------
// IFieldCtrl.Interface
//
// ì¬“ú: 2023/10/17
// ì¬Ò: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFieldCtrl
{
    bool CheckAlreadyMinoExist(int x, int y);
    void SetMino(int x,int y);
}
