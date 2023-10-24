// -------------------------------------------------------------------------------
// PlayerInput.cs
//
// �쐬��: 2023/10/21
// �쐬��: Shizuku
// -------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    #region �ϐ�
    private IMinoUnionCtrl _minoUnion = default; //�~�m����V�X�e���̃C���^�[�t�F�C�X
    private IMinoHoldable _holdSystem = default;
    #endregion

    #region �v���p�e�B

    #endregion

    #region ���\�b�h
    /// <summary>
    /// ����������
    /// </summary>
    void Awake()
    {
        _minoUnion = GetComponent<IMinoUnionCtrl>();
        _holdSystem = FindObjectOfType<HoldMino>().GetComponent<IMinoHoldable>();
    }

    /// <summary>
    /// �X�V�O����
    /// </summary>
    void Start()
    {

    }

    /// <summary>
    /// �X�V����
    /// </summary>
    void Update()
    {
        PlayerIO();
    }

    /// <summary>
    /// <para>PlayerIO</para>
    /// <para>�v���C���[�̓��͂ɉ����āA�~�m�𑀍삵�܂�</para>
    /// </summary>
    private void PlayerIO()
    {
        if (Input.GetKeyDown(KeyCode.A)) { _minoUnion.Move(-1); }
        if (Input.GetKeyDown(KeyCode.D)) { _minoUnion.Move(1); }
        if (Input.GetKeyDown(KeyCode.Q)) { _minoUnion.Rotate(-1); }
        if (Input.GetKeyDown(KeyCode.E)) { _minoUnion.Rotate(1); }
        if (Input.GetKeyDown(KeyCode.W)) { _minoUnion.HardDrop(); }
        if (Input.GetKey(KeyCode.S)) { _minoUnion.SoftDrop(); }
        Debug.Log(_holdSystem);
        if (Input.GetKeyDown(KeyCode.LeftShift)) { _holdSystem.Hold(); }
    }
    #endregion
}
