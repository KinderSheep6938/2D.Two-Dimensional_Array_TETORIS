// -------------------------------------------------------------------------------
// IMinoInfo.Interface
//
// �쐬��: 2023/10/17
// �쐬��: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMinoInfo
{
    int MinoX { get; set; }
    int MinoY { get; set; }

    void ChangeColor(Color minoColor);
}
