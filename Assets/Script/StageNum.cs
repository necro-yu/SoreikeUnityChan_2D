using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageNum : MonoBehaviour
{
    private Text stageText = null;
    private int oldStage = 0;

    // Start is called before the first frame update
    void Start()
    {
        stageText = GetComponent<Text>();
        if (ThisGameManager.instance != null)
        {
            stageText.text = "Stage " + ThisGameManager.instance.stageNum;
        }
        else
        {
            Debug.Log("ゲームマネージャーがないよ");
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (oldStage != ThisGameManager.instance.stageNum)
        {
            stageText.text = "Stage " + ThisGameManager.instance.stageNum;
            oldStage = ThisGameManager.instance.stageNum;
        }
    }
}
