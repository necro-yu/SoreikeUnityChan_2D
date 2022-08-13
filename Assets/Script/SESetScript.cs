using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SESetScript : MonoBehaviour
{
    public static SESetScript instance = null;
    #region// SE
    // プレイヤー関連
    [Header("ジャンプSE")] public AudioClip jumpSE;
    [Header("ダメージSE")] public AudioClip playerDamageSE;
    [Header("GameOverSE")] public AudioClip gameOverSE;
    [Header("StageClearSE")] public AudioClip stageClearSE;
    [Header("コンティニューポイントSE")] public AudioClip continueSE;

    // 敵関連
    [Header("リトライSE")] public AudioClip retrySE;
    [Header("踏みつけSE")] public AudioClip stepOnSE;
    #endregion

    // 初期化処理
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
