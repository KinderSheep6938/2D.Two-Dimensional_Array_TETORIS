// -------------------------------------------------------------------------------
// MinoFactory.cs
//
// 作成日: 2023/10/18
// 作成者: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MinoPoolManager))]
public class MinoFactory : MonoBehaviour
{
    #region 変数
    private IMinoCreatable _minoCreator = default; //ミノの形生成インターフェイス
    private MinoPoolManager _minoManager = default; //ミノブロックのオブジェクトプール

    //ミノ形リストを初期化するリスト
    private IMinoCreatable.MinoType[] _InitializeModels =
    {
        IMinoCreatable.MinoType.minoO,
        IMinoCreatable.MinoType.minoS,
        IMinoCreatable.MinoType.minoZ,
        IMinoCreatable.MinoType.minoJ,
        IMinoCreatable.MinoType.minoL,
        IMinoCreatable.MinoType.minoT,
        IMinoCreatable.MinoType.minoI
    };
    //生成可能なミノ形を保存するリスト
    private List<IMinoCreatable.MinoType> _canCreateModels = new();
    private int _createModelIndex = 0; //生成するミノ形のインデックス

    [SerializeField]
    private bool _onDebug = false;
    #endregion

    #region プロパティ

    #endregion

    #region メソッド
    /// <summary>
    /// 初期化処理
    /// </summary>
    void Awake()
    {
        //初期化
        _minoCreator = FindObjectOfType<NextMino>().GetComponent<IMinoCreatable>();
        _minoManager = GetComponent<MinoPoolManager>();
        _canCreateModels.Clear();
        _canCreateModels.AddRange(_InitializeModels);
    }

    /// <summary>
    /// <para>CreateMino</para>
    /// <para>ミノを生成します</para>
    /// </summary>
    public void CreateMino()
    {
        if (_onDebug) { DebugView(); } //デバッグ表示
        //生成可能なミノ形が存在しない
        if(_canCreateModels.Count == 0)
        {
            //リスト初期化
            _canCreateModels.Clear();
            _canCreateModels.AddRange(_InitializeModels);
        }
        //生成可能なミノ形のIndexを設定
        _createModelIndex = Random.Range(0, _canCreateModels.Count);
        //ミノ生成　引数：使用可能なミノブロック,生成するミノ形
        _minoCreator.CreateMinoUnit(_minoManager.GetUseableMinos(), _canCreateModels[_createModelIndex]);
        //生成したミノ形をリストから除外
        _canCreateModels.RemoveAt(_createModelIndex);
    }

    /// <summary>
    /// <para>DebugView</para>
    /// <para>デバッグ表示</para>
    /// </summary>
    void DebugView()
    {
        Debug.Log("----------");
        for (int i = 0; i < _canCreateModels.Count; i++)
        {
            Debug.Log(_canCreateModels[i]);
        }
    }
    #endregion
}
