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
    private bool rightTleftF = false;
    private bool isDamage = false;
    private bool justOne = false;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        objectCollision = GetComponent<ObjectCollision>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (isDamage)
        {
            bossBlinker.BeginBlink();
        }
        else
        {
            bossBlinker.EndBlink();
        }
    }

    // FixedUpdate
    void FixedUpdate()
    {
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
                    transform.localScale = new Vector3(-1.8f, 1.8f, 1.8f);
                }
                else
                {
                    transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
                }
                rigidbody2D.velocity = new Vector2(xVector * speed, gravity * Physics.gravity.y);
            }
            else
            {
                // 画面外の間はスリープすることで負荷軽減になる
                rigidbody2D.Sleep();
            }
        }
        else
        {
            // 死んだ際のアニメーション・簡易なエフェクトなど
            if (!isDamage)
            {
                animator.Play("Zako_Down");
                if (!justOne)
                {
                    ThisGameManager.instance.PlaySE(SESetScript.instance.stepOnSE);
                    justOne = true;
                }
                rigidbody2D.velocity = new Vector2(0, -gravity);
                isDamage = true;
                if (ThisGameManager.instance != null)
                {
                    ThisGameManager.instance.scoreNum += enemyScore;
                }
                boxCollider2D.enabled = false;
                Destroy(gameObject, 2f);

                ThisGameManager.instance.isStageCrear = true;
            }
            else
            {
                // やられたときにコライダーを切っているので動かしても問題ない
                transform.Rotate(new Vector3(0, 0, 7));
            }
        }
    }

    /// <summary>
    /// ボスモンスターへのダメージ処理
    /// </summary>
    private void BossSubLife()
    {
        if (bossLife > 0)
        {
            --bossLife;
        }
        else
        {
            isDamage = true;
        }
    }
}
