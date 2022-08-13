using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThisGameManager : MonoBehaviour
{
    public static ThisGameManager instance = null;

    [Header("現在のステージ")] public int stageNum;
    [Header("現在の復帰位置")] public int continueNum;
    [Header("現在の残基")] public int lifeNum;
    [Header("デフォルト残基")] public int defaultLideNum = 3;
    [Header("残基の上限")] public int limitLifeNum = 99;
    [HideInInspector] public bool isGameOver;
    [HideInInspector] public bool isStageCrear;

    private AudioSource audioSource = null;

    /// <summary>
    /// 現状未使用
    /// </summary>
    [Header("スコア")] public int scoreNum;

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

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void AddLifeNum()
    {
        if (lifeNum < limitLifeNum)
        {
            ++lifeNum;
        }
    }

    /// <summary>
    /// 残基を減らすメソッド
    /// </summary>
    public void SubLifeNum()
    {
        if (lifeNum > 0)
        {
            --lifeNum;
        }
        else
        {
            isGameOver = true;
        }
    }

    /// <summary>
    ///  最初から始める場合の処理
    /// </summary>
    public void RetryGame()
    {
        isGameOver = false;
        lifeNum = defaultLideNum;
        scoreNum = 0;
        stageNum = 1;
        continueNum = 0;
    }

    /// <summary>
    /// 直近のセーブポイントから再開する
    /// </summary>
    public void ContinueGame()
    {
        isGameOver = false;
        lifeNum = defaultLideNum;
    }

    /// <summary>
    /// SEを鳴らすメソッド
    /// </summary>
    /// <param name="audioClip">鳴らすSE</param>
    public void PlaySE(AudioClip audioClip)
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(audioClip);
        }
        else
        {
            Debug.Log("GMにオーディオソースが設定されていないよ。");
        }
    }
}
