using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SESetScript : MonoBehaviour
{
    public static SESetScript instance = null;
    #region// SE
    // �v���C���[�֘A
    [Header("�W�����vSE")] public AudioClip jumpSE;
    [Header("�_���[�WSE")] public AudioClip playerDamageSE;
    [Header("GameOverSE")] public AudioClip gameOverSE;
    [Header("StageClearSE")] public AudioClip stageClearSE;
    [Header("�R���e�B�j���[�|�C���gSE")] public AudioClip continueSE;

    // �G�֘A
    [Header("���g���CSE")] public AudioClip retrySE;
    [Header("���݂�SE")] public AudioClip stepOnSE;
    #endregion

    // ����������
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
