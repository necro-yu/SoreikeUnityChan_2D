using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageControle : MonoBehaviour
{
    [Header("プレイヤーのゲームオブジェクト")] public GameObject plyaerObj;
    [Header("コンティニュー位置")] public GameObject[] continuePoint;
    [Header("ゲームオーバー")] public GameObject gameOverObj;
    [Header("ステージクリア")] public GameObject stageClearObj;
    [Header("フェード")] public FadeImage fade;

    private UnityChanController unityChanController;
    private int nextStageNum;
    private bool startFade = false;
    private bool nextStageGo = false;
    private bool retryGame = false;
    private bool doGameOver = false;
    private bool doClear = false;
    private bool doScoreChange = false;


    // Start is called before the first frame update
    void Start()
    {
        if (plyaerObj != null && continuePoint != null && continuePoint.Length > 0
            && gameOverObj != null && fade != null && !retryGame)
        {
            gameOverObj.SetActive(false);
            stageClearObj.SetActive(false);

            // 初期開始位置(最後に踏んでいるコンティニューポイント)
            plyaerObj.transform.position = continuePoint[ThisGameManager.instance.continueNum].transform.position;

            unityChanController = plyaerObj.GetComponent<UnityChanController>();
            if (unityChanController == null)
            {
                Debug.Log("プレイヤーじゃないものがアタッチされているよ。");
            }
        }
        else
        {
            Debug.Log("設定が足りていないよ");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // ゲームオーバーの処理
        if (ThisGameManager.instance.isGameOver && !doGameOver)
        {
            gameOverObj.SetActive(true);
            doGameOver = true;
            Debug.Log("ゲームオーバー処理");
        }

        // ステージクリア処理
        if (ThisGameManager.instance.isStageCrear && !doClear)
        {
            stageClearObj.SetActive(true);
            ThisGameManager.instance.PlaySE(SESetScript.instance.stageClearSE);
            doClear = true;
            Debug.Log("ステージクリア処理");
        }

        // ステージ切り替え
        if (fade != null && startFade && !doScoreChange)
        {
            if (fade.IsFadeOutComplete())
            {
                // リトライゲーム
                if (retryGame)
                {
                    ThisGameManager.instance.ContinueGame();
                    ThisGameManager.instance.PlaySE(SESetScript.instance.retrySE);
                    Debug.Log("リトライゲーム");
                }
                // 次のステージを設定
                else
                {
                    ThisGameManager.instance.stageNum = nextStageNum;
                }

                // 画面移動
                if (retryGame || nextStageGo)
                {
                    ThisGameManager.instance.isStageCrear = false;
                    // まだ次のステージはできていない。
                    SceneManager.LoadScene("Stage" + ThisGameManager.instance.stageNum);
                    doScoreChange = true;
                    Debug.Log("ロードシーン");
                }
            }
        }
    }

    /// <summary>
    /// Retryボタン押下時にSave位置から再開する
    /// </summary>
    public void Retry()
    {
        ChangeScene(ThisGameManager.instance.stageNum);
        retryGame = true;
        Debug.Log("リトライボタン");
    }

    /// <summary>
    /// Nextボタン押下時に次のステージへいく
    /// </summary>
    public void NextStage()
    {
        nextStageGo = true;
        // Stage2がまだないためThankyouForPlayingになる
        ChangeScene(ThisGameManager.instance.stageNum + 1);
        Debug.Log("次のステージ");
    }

    /// <summary>
    /// ステージ切り替え用メソッド
    /// </summary>
    /// <param name="num">切り替えるステージ番号</param>
    public void ChangeScene(int num)
    {
        if (fade != null)
        {
            nextStageNum = num;
            fade.StartFadeOut();
            startFade = true;
            Debug.Log("チェンジステージ");
        }
    }
}
