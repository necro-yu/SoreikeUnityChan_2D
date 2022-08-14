using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BOSS : MonoBehaviour
{
    #region// インスペクターで設定する項目
    [Header("加算スコア")] public int enemyScore;
    [Header("移動速度")] public float speed;
    [Header("重力")] public float gravity = 0.7f;
    [Header("画面外でも行動するか")] public bool nonVisible;
    [Header("摂食判定スクリプト")] public Enemy_CollisionCheck eCollisioncheck;
    [Header("ダメージ時の点滅")] public SpliteRendererBlinker bossBlinker;
    [Header("ボスのライフ")] public int bossLife;
    #endregion

    #region// プライベート変数群
    private new Rigidbody2D rigidbody2D = null;
    private SpriteRenderer spriteRenderer = null;
    private Animator animator = null;
    private ObjectCollision objectCollision = null;
    private BoxCollider2D boxCollider2D = null;
    private Vector3 defaultScale;
    private bool rightTleftF = false;
    private bool isDamage = false;
    private bool isDead = false;
    private float damagetime = 2.0f;
    private float timer = 0.0f;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        objectCollision = GetComponent<ObjectCollision>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        defaultScale = transform.localScale;
    }

    // Update is called once per frame
    private void Update()
    {
        if (isDamage)
        {
            bossBlinker.BeginBlink();
            Debug.Log("点滅");
        }
        else
        {
            bossBlinker.EndBlink();
            Debug.Log("点滅終了");
        }
    }

    // FixedUpdate
    void FixedUpdate()
    {
        if (ThisGameManager.instance.isGameOver || ThisGameManager.instance.isGameOver)
        {
            rigidbody2D.velocity = new Vector2(0, -gravity);
            return;
        }

        if (!objectCollision.playerStepOn)
        {
            if (spriteRenderer.isVisible || nonVisible)
            {
                // 壁や敵で反転する
                if (eCollisioncheck.isOn)
                {
                    rightTleftF = !rightTleftF;
                }

                // 反転処理
                int xVector = -1;
                if (rightTleftF)
                {
                    xVector = 1;
                    transform.localScale = new Vector3(-defaultScale.x, defaultScale.y, defaultScale.z);
                }
                else
                {
                    transform.localScale = new Vector3(defaultScale.x, defaultScale.y, defaultScale.z);
                }
                rigidbody2D.velocity = new Vector2(xVector * speed, gravity * Physics.gravity.y);

                // 動いたら画面外でも動く
                nonVisible = true;
            }
            else
            {
                // 画面外の間はスリープすることで負荷軽減になる
                rigidbody2D.Sleep();
            }
        }
        else
        {
            if (objectCollision.playerStepOn)
            {
                objectCollision.playerStepOn = false;
            }
            // ダメージを受けた時の処理
            if (!isDamage)
            {
                isDamage = true;
                --bossLife;
                ThisGameManager.instance.PlaySE(SESetScript.instance.stepOnSE);
                Debug.Log($"{bossLife}");
                Debug.Log("ダメージフラグ");
            }
        }

        // 死んだ際のアニメーション・簡易なエフェクトなど
        if (bossLife <= 0)
        {
            Debug.Log("倒した");
            animator.Play("Zako_Down");
            rigidbody2D.velocity = new Vector2(0, gravity * Physics2D.gravity.y);
            if (ThisGameManager.instance != null)
            {
                ThisGameManager.instance.scoreNum += enemyScore;
            }
            isDead = true;
            ThisGameManager.instance.isStageCrear = true;

            boxCollider2D.enabled = false;
            Destroy(gameObject, 2f);
        }
        else if(isDead)
        {
            // やられたときにコライダーを切っているので動かしても問題ない
            transform.Rotate(new Vector3(0, 0, 7));
        }

        timer += Time.deltaTime;
        if (damagetime < timer)
        {
            isDamage = false;
            Debug.Log("ダメージフラグ戻った");
            Debug.Log($"{objectCollision.playerStepOn}");
            timer = 0.0f;
        }
    }
}
