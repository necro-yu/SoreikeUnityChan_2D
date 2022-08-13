using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeActiveUGUI : MonoBehaviour
{
    [Header("フェードスピード")] public float speed = 1.0f;
    [Header("上昇量")] public float moveDis = 10.0f;
    [Header("上昇時間")] public float moveTime = 1.0f;
    [Header("キャンバスグループ")] public CanvasGroup canvasGroup;
    [Header("プレイヤー判定")] public PlayerTriggerCheck playerTriggerCheck;

    private Vector3 defaultPos;
    private float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        // 初期化
        if(canvasGroup == null && playerTriggerCheck == null)
        {
            Debug.Log("インスペクターの設定が足りません");
            Destroy(this);
        }
        else
        {
            // フェードインしながら元の位置に戻る形を作成
            canvasGroup.alpha = 0.0f;
            defaultPos = canvasGroup.transform.position;
            canvasGroup.transform.position = defaultPos - Vector3.up * moveDis;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 判定内に入ってきた場合
        if (playerTriggerCheck.isOn)
        {
            if (canvasGroup.transform.position.y < defaultPos.y || canvasGroup.alpha < 1.0f)
            {
                // 上昇しながらフェードインを行う
                canvasGroup.alpha = timer / moveTime;
                // 時間を調節するために、スピードと時間をさらに掛けている
                canvasGroup.transform.position += Vector3.up * (moveDis / moveTime) * speed * Time.deltaTime;
                timer += speed * Time.deltaTime;
            }
            // フェードイン完了の場合
            else
            {
                // 正確な数値を入れておく
                canvasGroup.alpha = 1.0f;
                canvasGroup.transform.position = defaultPos;
            }
        }
        // プレイヤーが判定外の場合
        else
        {
            if (canvasGroup.transform.position.y > defaultPos.y - moveDis || canvasGroup.alpha > 0.0f)
            {
                canvasGroup.alpha = timer / moveTime;
                canvasGroup.transform.position -= Vector3.up * (moveDis / moveTime) * speed * Time.deltaTime;
                timer -= speed * Time.deltaTime;
            }
            // フェードアウト完了
            else
            {
                timer = 0.0f;
                canvasGroup.alpha = 0.0f;
                canvasGroup.transform.position = defaultPos - Vector3.up * moveDis;
            }
        }

    }
}
