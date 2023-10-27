// -------------------------------------------------------------------------------
// GameCtrlManager.cs
//
// 作成日: 2023/10/21
// 作成者: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameCtrlManager : MonoBehaviour
{
    #region 変数
    private const int GAMEEND_ID = -1; //ゲーム終了ID
    private const int COMMIT_ID = 1; //操作完了ID
    private int _nowGameStatus = 0; //現在のゲーム状況
    private bool _isStart = false; //ゲーム開始フラグ
    [SerializeField, Header("ゲームスタートテキスト")]
    private GameObject _gameStartText = default;

    private bool _canRetry = false; //リトライ可能フラグ
    [SerializeField, Header("ゲームオーバーテキスト")]
    private GameObject _gameoverViewObj = default;

    private AudioSource _bgmAudio = default; //BGMのAudioSource
    private MinoFactory _factorySystem = default; //ミノ生成システム
    private FieldManager _fieldSystem = default; //フィールド管理マネージャー
    private PlayerInput _playerInput = default; //プレイヤー操作マネージャー
    #endregion

    #region メソッド
    /// <summary>
    /// 初期化処理
    /// </summary>
    void Awake()
    {
        //初期化
        _bgmAudio = GetComponent<AudioSource>();
        _factorySystem = GetComponent<MinoFactory>();
        _fieldSystem = GetComponent<FieldManager>();
        _playerInput = GetComponentInChildren<PlayerInput>();
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        //ゲーム状況
        _nowGameStatus = _fieldSystem.GetPlayStatus();

        //ゲーム状況がコミット状態ではない
        if(_nowGameStatus != COMMIT_ID)
        {
            //ゲーム状態が終了状態である
            if(_nowGameStatus == GAMEEND_ID)
            {
                _gameoverViewObj.SetActive(true); //ゲームオーバー表示
                _canRetry = true; //リトライ可能に
                _bgmAudio.Stop(); //BGM停止
                _playerInput.SetStopInput(); //プレイヤー入力不能に
            }
            return; //操作終了
        }
        
        //コミット状態である場合は新しくミノを生成する
        _factorySystem.CreateMino();
    }


    /// <summary>
    /// <para>OnStart</para>
    /// <para>開始ボタンが押されたときに呼ばれます</para>
    /// </summary>
    /// <param name="context">ボタン状態</param>
    public void OnStart(InputAction.CallbackContext context)
    {
        //ゲーム開始ボタンが押された かつ スタートしていない
        if (context.phase == InputActionPhase.Started && !_isStart)
        { //ゲームスタート
            _isStart = true; //開始フラグ
            _gameStartText.SetActive(false); //テキスト非表示
            _bgmAudio.Play(); //BGM再生

            //ネクストミノと操作ミノの二つにミノを送らなければいかないため、ミノ生成を２回行う
            _factorySystem.CreateMino(); //ミノ生成
            _factorySystem.CreateMino(); //ミノ生成
        }
    }

    /// <summary>
    /// <para>OnRetry</para>
    /// <para>再挑戦ボタンが押されたときに呼ばれます</para>
    /// </summary>
    /// <param name="context">ボタン状態</param>
    public void OnRetry(InputAction.CallbackContext context)
    {
        //リトライボタンが押された かつ リトライ可能である
        if (context.phase == InputActionPhase.Started && _canRetry)
        { //リトライ
            //同じシーン読み込み
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    #endregion
}
