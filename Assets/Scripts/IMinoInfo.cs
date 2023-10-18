// -------------------------------------------------------------------------------
// IMinoInfo.Interface
//
// çÏê¨ì˙: 2023/10/17
// çÏê¨é“: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMinoInfo
{
    int MinoX { get; }
    int MinoY { get; }

    void ChangeColor(Color minoColor);
    void SetMinoPos(float x, float y, Transform parent);
}
