using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityChanController : MonoBehaviour
{
    //アニメーションするためのコンポーネントを入れる
    Animator animator;
    //Unitychanを移動させるコンポーネントを入れる
    Rigidbody2D rigid2D;
    // Unitychanのコライダー
    CapsuleCollider2D capsuleCollider2D;


    #region// publicな変数群
    [Header("接地判定")] public GroundCheck groundCheck;
    [Header("頭上判定")] public GroundCheck headCheck;
    [Header("踏みつけ時の敵判定")] public GroundCheck enemyCheck;
    [Header("走るアニメーション速度表現")] public AnimationCurve runAnimationCurve;
    [Header("ジャンプアニメーション速度表現")] public AnimationCurve jumpAnimationCurve;
    [Header("点滅エフェクト")] public SpliteRendererBlinker blinker;
    [Header("踏みつけ高さの割合")] public float stepOnRate = 10;
    [Header("移動速度")] public float runSpeed = 9.0f;
    [Header("重力")] public float gravity = 0.7f;
    [Header("ジャンプ速度")] public float jumpSpeed = 9.0f;
    [Header("ジャンプの高さ制限")] public float jumpHeight = 7.0f;
    #endregion

    #region// privateな変数群
    // ジャンプのポジション
    private float jumpPos = 0.0f;
    // 滞空時間
    private float jumpTime = 0.0f;
    // 滞空時間制限
    private float jumpLimitTime = 5.0f;
    // 踏んだ時に跳ねる高さ
    private float otherJumpHeight = 0.0f;
    // 走った時間
    private float runTime = 0.0f;
    // キー入力の保存
    private float beforeKey = 0.0f;
    // 経過時間
    private float _time = 0.0f;
    // 動けない時間
    private float lostTime = 1.5f;

    // アニメーターに渡す走る速度
    private float setRunSpeed = 0.0f;
    // 接地判定を受け取るbool値
    private bool isGround = false;
    // ジャンプの判定
    private bool isJump = false;
    // 頭上の判定
    private bool isHead = false;
    // ダウンの判定
    private bool isDamage = false;
    // やられたときの判定
    private bool isDown = false;
    // 踏んだ時の跳ねる判定
    private bool isOtherJump = false;
    // 踏みつけフラグ
    private bool isStepEnemy = false;
    // クリアフラグ
    private bool isClearMotion = false;

    // 1度だけ処理したいものに使う変数
    private bool justOnce = false;
    #endregion

    #region// Tag
    // EnemyTag
    private string enemyTag = "Enemy";
    // 確定死エリアTag
    private string deadAreaTag = "DeadArea";
    // ダメージエリアTag
    private string damageAreaTag = "DamageArea";
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigid2D = GetComponent<Rigidbody2D>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        blinker = GetComponent<SpliteRendererBlinker>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (isDamage)
        {
            blinker.BeginBlink();
        }
        else
        {
            blinker.EndBlink();
        }

        // 走るSE
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Unitychan_Run"))
        {
            GetComponent<AudioSource>().volume = 0.2f;
        }
        else
        {
            GetComponent<AudioSource>().volume = 0.0f;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // ダメージ時、ゲームオーバー時は動かないように
        if (!isDamage && !ThisGameManager.instance.isGameOver && !isClearMotion)
        {

            // 接地判定を受け取る
            isGround = groundCheck.IsGround();
            // 頭上判定を受け取る
            isHead = headCheck.IsGround();
            // 下の敵判定を受け取る
            isStepEnemy = enemyCheck.isEnemyCheck();

            float xSpeed = GetXSpeed();
            float ySpeed = GetYSpeed();

            //アニメーションを適用
            SetAnimation();

            // 移動速度を設定
            rigid2D.velocity = new Vector2(xSpeed, ySpeed);

        }
        else if (isDamage)
        {
            // ダウン中
            _time += Time.deltaTime;

            if (!justOnce)
            {
                // 速度を無くさないと可笑しな挙動になる
                rigid2D.velocity = new Vector2(0.0f, gravity * Physics.gravity.y);

                // とりあえず向いている方向から後ろへノックバック
                if (transform.localScale.x >= 0)
                {
                    rigid2D.AddForce(transform.right * -400.0f);
                }
                else
                {
                    rigid2D.AddForce(transform.right * 400.0f);
                }
                justOnce = true;
            }
            if (lostTime < _time)
            {
                isDamage = false;
                animator.SetBool("Damage", isDamage);
                justOnce = false;
                _time = 0.0f;
            }
        }
        else if (ThisGameManager.instance.isGameOver)
        {
            // 速度が保存されたままダウンするため
            rigid2D.velocity = new Vector2(0.0f, gravity * Physics.gravity.y);
        }
        else
        {
            // クリア時はクリアモーションを行う
            if(!isClearMotion && ThisGameManager.instance.isStageCrear)
            {
                animator.Play("");
                isClearMotion = true;
            }
            rigid2D.velocity = new Vector2(0, -gravity);
        }
    }

    /// <summary>
    /// アニメーションを設定する。
    /// </summary>
    private void SetAnimation()
    {
        animator.SetBool("Jump", isJump || isOtherJump);
        animator.SetFloat("Speed", setRunSpeed);
        animator.SetBool("Ground", isGround);
    }

    /// <summary>
    /// Y成分の必要な計算を行い、速度を返す
    /// </summary>
    /// <returns>Y軸の速度</returns>
    private float GetYSpeed()
    {
        // InputManagerを介したキー入力を受け取る
        float verticalKey = Input.GetAxis("Vertical");
        // RigitBodyのgravityを０にしているため、自前の重力を設定する。
        float ySpeed = gravity * Physics.gravity.y;

        // 何かを踏んだ時に跳ねる
        if (isOtherJump)
        {
            // 飛べる高さを超えていないか
            bool canJumpHeight = jumpPos + otherJumpHeight > transform.position.y;
            //ジャンプ時間が長くなりすぎてないか
            bool canTime = jumpLimitTime > jumpTime;
            // ジャンプ条件を満たしているか
            if (canJumpHeight && canTime && !isHead)
            {
                ySpeed = jumpSpeed;
                jumpTime += Time.deltaTime;
            }
            else
            {
                isOtherJump = false;
                jumpTime = 0.0f;
            }
        }
        // 接地しているとき
        else if (isGround && !isJump)
        {
            // 上方向入力
            if (verticalKey > 0)
            {
                if (!isJump)
                {
                    ThisGameManager.instance.PlaySE(SESetScript.instance.jumpSE);
                }
                // 上方向の力をかける
                ySpeed = jumpSpeed;
                jumpPos = transform.position.y;
                isJump = true;
                jumpTime = 0.0f;
            }
            else
            {
                isJump = false;
            }
        }
        // ジャンプ中
        else if (isJump && !isGround)
        {
            // 上入力があるかどうか
            bool pushUpKey = verticalKey > 0;
            // 飛べる高さを超えていないか
            bool canJumpHeight = jumpPos + jumpHeight > transform.position.y;
            //ジャンプ時間が長くなりすぎてないか
            bool canTime = jumpLimitTime > jumpTime;
            // ジャンプ条件を満たしているか
            if (pushUpKey && canJumpHeight && canTime && !isHead)
            {
                ySpeed = jumpSpeed;
                jumpTime += Time.deltaTime;
            }
            else
            {
                isJump = false;
                jumpTime = 0.0f;
            }
        }

        if (isJump || isOtherJump)
        {
            ySpeed *= jumpAnimationCurve.Evaluate(jumpTime);
        }

        return ySpeed;
    }

    private float GetXSpeed()
    {
        // InputManagerを介したキー入力を受け取る
        float horizontalKey = Input.GetAxis("Horizontal");
        float xSpeed = 0.0f;

        // 右方向入力
        if (horizontalKey > 0)
        {
            transform.localScale = new Vector3(5, 5, 5);
            setRunSpeed = runSpeed;
            runTime += Time.deltaTime;
            xSpeed = runSpeed;
        }
        // 左方向入力
        else if (horizontalKey < 0)
        {
            transform.localScale = new Vector3(-5, 5, 5);
            setRunSpeed = runSpeed;
            runTime += Time.deltaTime;
            xSpeed = -runSpeed;
        }
        else
        {
            setRunSpeed = 0.0f;
            runTime = 0.0f;
            xSpeed = 0.0f;
        }

        // 入力の反転を判断して加速のリセットをする
        if (horizontalKey > 0 && beforeKey < 0)
        {
            runTime = 0.0f;
        }
        else if (horizontalKey < 0 && beforeKey > 0)
        {
            runTime = 0.0f;
        }

        beforeKey = horizontalKey;
        // アニメーションカーブに適応
        xSpeed *= runAnimationCurve.Evaluate(runTime);

        return xSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ThisGameManager.instance.isGameOver)
        {
            return;
        }

        if (collision.tag == damageAreaTag)
        {
            // 一旦これで確定死
            ThisGameManager.instance.SubLifeNum();
            ThisGameManager.instance.SubLifeNum();
            ThisGameManager.instance.SubLifeNum();
            UnityChanDamage();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == enemyTag)
        {
            if (isStepEnemy)
            {
                // 踏みつけ判定の高さ(割合) 
                float stepOnHeight = capsuleCollider2D.size.y * (stepOnRate / 100f);

                // 踏みつけ判定の高さ
                float judgePos = transform.position.y - (capsuleCollider2D.size.y / 2f) + stepOnHeight;

                // 複数の接触情報を全て確認する。
                foreach (ContactPoint2D p in collision.contacts)
                {
                    if (p.point.y < judgePos)
                    {
                        ObjectCollision o = collision.gameObject.GetComponent<ObjectCollision>();
                        if (o != null)
                        {
                            otherJumpHeight = o.boundHeight;
                            o.playerStepOn = true;
                            jumpPos = transform.position.y;
                            isOtherJump = true;
                            isJump = true;
                            jumpTime = 0.0f;
                        }
                        else
                        {
                            Debug.Log("ObjectCollisionが相手についていないよ！");
                        }
                    }
                }
            }
            else
            {
                UnityChanDamage();
            }
        }
    }

    /// <summary>
    /// UnityChanがダメージを受ける
    /// </summary>
    private void UnityChanDamage()
    {
        if (isDown || ThisGameManager.instance.isStageCrear)
        {
            return;
        }

        // ダメージを受ける
        ThisGameManager.instance.SubLifeNum();
        if (!ThisGameManager.instance.isGameOver)
        {
            ThisGameManager.instance.PlaySE(SESetScript.instance.playerDamageSE);
            isDamage = true;
            animator.SetBool("Damage", isDamage);
        }
        else
        {
            ThisGameManager.instance.PlaySE(SESetScript.instance.gameOverSE);
            isDown = true;
            animator.Play("Unitychan_Damage_down");
        }
    }

    /// <summary>
    /// コンティニューする
    /// </summary>
    public void ContinueUnityChan()
    {
        // フラグなどを下し、Idle状態へ
        isDown = false;
        animator.Play("Unitychan_Idle");
        isJump = false;
        isOtherJump = false;
        setRunSpeed = 0.0f;
    }

    /// <summary>
    /// コンティニュー待機状態判定メソッド
    /// </summary>
    /// <returns></returns>
    public bool IsContinueWaitng()
    {
        if (ThisGameManager.instance.isGameOver)
        {
            return IsDownAnimEnd();
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// ダウンアニメーションが完了しているかどうか
    /// </summary>
    /// <returns></returns>
    private bool IsDownAnimEnd()
    {
        if (isDown && animator != null)
        {
            AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
            if (currentState.IsName("Unitychan_Damage_down"))
            {
                if (currentState.normalizedTime >= 1)
                {
                    return true;
                }
            }
        }
        return false;
    }

}