using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreItem : MonoBehaviour
{
    [Header("加算するスコア")] public int addScore;
    [Header("プレイヤーの判定")] public PlayerTriggerCheck playerTriggerCheck;

    // Update is called once per frame
    void Update()
    {
        if (playerTriggerCheck.isOn)
        {
            if(ThisGameManager.instance != null)
            {
                // ※Itemにコライダーも付けるのを忘れずに
                ThisGameManager.instance.scoreNum += addScore;
                Destroy(this.gameObject);
            }
        }
    }
}
