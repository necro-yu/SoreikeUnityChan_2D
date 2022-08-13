using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    private Text scoreText = null;
    private int oldScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<Text>();
        if(ThisGameManager.instance != null)
        {
            scoreText.text = "Score " + ThisGameManager.instance.scoreNum;
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
        if(oldScore != ThisGameManager.instance.scoreNum)
        {
            scoreText.text = "Score " + ThisGameManager.instance.scoreNum;
            oldScore = ThisGameManager.instance.scoreNum;
        }
    }
}
