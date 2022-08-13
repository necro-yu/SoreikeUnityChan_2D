using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuePoint : MonoBehaviour
{
    #region
    [Header("コンティニュー先番号")] public int continueNum;
    [Header("スピード")] public float speed = 2.0f;
    [Header("動く幅")] public float moveDis = 3.0f;
    [Header("プレイヤー判定")] public PlayerTriggerCheck playerTriggerCheck;
    [Header("取得アニメーション")] public AnimationCurve animationCurve;

    private bool on = false;
    private float kakudo = 0.0f;
    private Vector3 defaultPos;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        if (playerTriggerCheck == null || SESetScript.instance.continueSE == null)
        {
            Debug.Log("トリガーとSEの設定が足りません。");
            Destroy(this);
        }
        defaultPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTriggerCheck.isOn && !on)
        {
            ThisGameManager.instance.continueNum = continueNum;
            ThisGameManager.instance.PlaySE(SESetScript.instance.continueSE);
            on = true;
        }

        if (on)
        {
            if (kakudo < 180.0f)
            {
                //sinカーブで振動させる
                transform.position = defaultPos + Vector3.up * moveDis * Mathf.Sin(kakudo * Mathf.Deg2Rad);

                //途中からちっちゃくなる
                if (kakudo > 90.0f)
                {
                    transform.localScale = Vector3.one * (1 - ((kakudo - 90.0f) / 90.0f));
                }
                kakudo += 180.0f * Time.deltaTime * speed;
            }
            else
            {
                gameObject.SetActive(false);
                on = false;
            }
        }
    }
}
