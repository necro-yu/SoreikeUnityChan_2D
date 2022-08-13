using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Zako1 : MonoBehaviour
{
    #region// インスペクターで設定する項目
    [Header("加算スコア")] public int enemyScore = 100;
    [Header("移動速度")] public float speed = 5.0f;
    [Header("重力")] public float gravity = 0.7f;
    [Header("画面外でも行動するか")] public bool nonVisible;
    [Header("摂食判定スクリプト")] public Enemy_CollisionCheck eCollisioncheck;

    #endregion

    #region// プライベート変数群
    private new Rigidbody2D rigidbody2D = null;
    private SpriteRenderer spriteRenderer = null;
    private Animator animator = null;
    private ObjectCollision objectCollision = null;
    private BoxCollider2D boxCollider2D = null;
    private bool rightTleftF = false;
    private bool isDead = false;
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
                    transform.localScale = new Vector3(-0.6f, 0.6f, 0.6f);
                }
                else
                {
                    transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
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
            if (!isDead)
            {
                animator.Play("Zako_Down");
                if (!justOne)
                {
                    ThisGameManager.instance.PlaySE(SESetScript.instance.stepOnSE);
                    justOne = true;
                }
                rigidbody2D.velocity = new Vector2(0, -gravity);
                isDead = true;
                if (ThisGameManager.instance != null)
                {
                    ThisGameManager.instance.scoreNum += enemyScore;
                }
                boxCollider2D.enabled = false;
                Destroy(gameObject, 2f);
            }
            else
            {
                // やられたときにコライダーを切っているので動かしても問題ない
                transform.Rotate(new Vector3(0, 0, 7));
                justOne = false;
            }
        }
    }
}
