using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityChanController : MonoBehaviour
{
    //�A�j���[�V�������邽�߂̃R���|�[�l���g������
    Animator animator;
    //Unitychan���ړ�������R���|�[�l���g������
    Rigidbody2D rigid2D;
    // Unitychan�̃R���C�_�[
    CapsuleCollider2D capsuleCollider2D;


    #region// public�ȕϐ��Q
    [Header("�ڒn����")] public GroundCheck groundCheck;
    [Header("���㔻��")] public GroundCheck headCheck;
    [Header("���݂����̓G����")] public GroundCheck enemyCheck;
    [Header("����A�j���[�V�������x�\��")] public AnimationCurve runAnimationCurve;
    [Header("�W�����v�A�j���[�V�������x�\��")] public AnimationCurve jumpAnimationCurve;
    [Header("�_�ŃG�t�F�N�g")] public SpliteRendererBlinker blinker;
    [Header("���݂������̊���")] public float stepOnRate = 10;
    [Header("�ړ����x")] public float runSpeed = 9.0f;
    [Header("�d��")] public float gravity = 0.7f;
    [Header("�W�����v���x")] public float jumpSpeed = 9.0f;
    [Header("�W�����v�̍�������")] public float jumpHeight = 7.0f;
    #endregion

    #region// private�ȕϐ��Q
    // �W�����v�̃|�W�V����
    private float jumpPos = 0.0f;
    // �؋󎞊�
    private float jumpTime = 0.0f;
    // �؋󎞊Ԑ���
    private float jumpLimitTime = 5.0f;
    // ���񂾎��ɒ��˂鍂��
    private float otherJumpHeight = 0.0f;
    // ����������
    private float runTime = 0.0f;
    // �L�[���͂̕ۑ�
    private float beforeKey = 0.0f;
    // �o�ߎ���
    private float _time = 0.0f;
    // �����Ȃ�����
    private float lostTime = 1.5f;

    // �A�j���[�^�[�ɓn�����鑬�x
    private float setRunSpeed = 0.0f;
    // �ڒn������󂯎��bool�l
    private bool isGround = false;
    // �W�����v�̔���
    private bool isJump = false;
    // ����̔���
    private bool isHead = false;
    // �_�E���̔���
    private bool isDamage = false;
    // ���ꂽ�Ƃ��̔���
    private bool isDown = false;
    // ���񂾎��̒��˂锻��
    private bool isOtherJump = false;
    // ���݂��t���O
    private bool isStepEnemy = false;
    // �N���A�t���O
    private bool isClearMotion = false;

    // 1�x�����������������̂Ɏg���ϐ�
    private bool justOnce = false;
    #endregion

    #region// Tag
    // EnemyTag
    private string enemyTag = "Enemy";
    // �m�莀�G���ATag
    private string deadAreaTag = "DeadArea";
    // �_���[�W�G���ATag
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

        // ����SE
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
        // �_���[�W���A�Q�[���I�[�o�[���͓����Ȃ��悤��
        if (!isDamage && !ThisGameManager.instance.isGameOver && !isClearMotion)
        {

            // �ڒn������󂯎��
            isGround = groundCheck.IsGround();
            // ���㔻����󂯎��
            isHead = headCheck.IsGround();
            // ���̓G������󂯎��
            isStepEnemy = enemyCheck.isEnemyCheck();

            float xSpeed = GetXSpeed();
            float ySpeed = GetYSpeed();

            //�A�j���[�V������K�p
            SetAnimation();

            // �ړ����x��ݒ�
            rigid2D.velocity = new Vector2(xSpeed, ySpeed);

        }
        else if (isDamage)
        {
            // �_�E����
            _time += Time.deltaTime;

            if (!justOnce)
            {
                // ���x�𖳂����Ȃ��Ɖ΂��ȋ����ɂȂ�
                rigid2D.velocity = new Vector2(0.0f, gravity * Physics.gravity.y);

                // �Ƃ肠���������Ă������������փm�b�N�o�b�N
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
            // ���x���ۑ����ꂽ�܂܃_�E�����邽��
            rigid2D.velocity = new Vector2(0.0f, gravity * Physics.gravity.y);
        }
        else
        {
            // �N���A���̓N���A���[�V�������s��
            if(!isClearMotion && ThisGameManager.instance.isStageCrear)
            {
                animator.Play("");
                isClearMotion = true;
            }
            rigid2D.velocity = new Vector2(0, -gravity);
        }
    }

    /// <summary>
    /// �A�j���[�V������ݒ肷��B
    /// </summary>
    private void SetAnimation()
    {
        animator.SetBool("Jump", isJump || isOtherJump);
        animator.SetFloat("Speed", setRunSpeed);
        animator.SetBool("Ground", isGround);
    }

    /// <summary>
    /// Y�����̕K�v�Ȍv�Z���s���A���x��Ԃ�
    /// </summary>
    /// <returns>Y���̑��x</returns>
    private float GetYSpeed()
    {
        // InputManager������L�[���͂��󂯎��
        float verticalKey = Input.GetAxis("Vertical");
        // RigitBody��gravity���O�ɂ��Ă��邽�߁A���O�̏d�͂�ݒ肷��B
        float ySpeed = gravity * Physics.gravity.y;

        // �����𓥂񂾎��ɒ��˂�
        if (isOtherJump)
        {
            // ��ׂ鍂���𒴂��Ă��Ȃ���
            bool canJumpHeight = jumpPos + otherJumpHeight > transform.position.y;
            //�W�����v���Ԃ������Ȃ肷���ĂȂ���
            bool canTime = jumpLimitTime > jumpTime;
            // �W�����v�����𖞂����Ă��邩
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
        // �ڒn���Ă���Ƃ�
        else if (isGround && !isJump)
        {
            // ���������
            if (verticalKey > 0)
            {
                if (!isJump)
                {
                    ThisGameManager.instance.PlaySE(SESetScript.instance.jumpSE);
                }
                // ������̗͂�������
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
        // �W�����v��
        else if (isJump && !isGround)
        {
            // ����͂����邩�ǂ���
            bool pushUpKey = verticalKey > 0;
            // ��ׂ鍂���𒴂��Ă��Ȃ���
            bool canJumpHeight = jumpPos + jumpHeight > transform.position.y;
            //�W�����v���Ԃ������Ȃ肷���ĂȂ���
            bool canTime = jumpLimitTime > jumpTime;
            // �W�����v�����𖞂����Ă��邩
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
        // InputManager������L�[���͂��󂯎��
        float horizontalKey = Input.GetAxis("Horizontal");
        float xSpeed = 0.0f;

        // �E��������
        if (horizontalKey > 0)
        {
            transform.localScale = new Vector3(5, 5, 5);
            setRunSpeed = runSpeed;
            runTime += Time.deltaTime;
            xSpeed = runSpeed;
        }
        // ����������
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

        // ���͂̔��]�𔻒f���ĉ����̃��Z�b�g������
        if (horizontalKey > 0 && beforeKey < 0)
        {
            runTime = 0.0f;
        }
        else if (horizontalKey < 0 && beforeKey > 0)
        {
            runTime = 0.0f;
        }

        beforeKey = horizontalKey;
        // �A�j���[�V�����J�[�u�ɓK��
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
            // ��U����Ŋm�莀
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
                // ���݂�����̍���(����) 
                float stepOnHeight = capsuleCollider2D.size.y * (stepOnRate / 100f);

                // ���݂�����̍���
                float judgePos = transform.position.y - (capsuleCollider2D.size.y / 2f) + stepOnHeight;

                // �����̐ڐG����S�Ċm�F����B
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
                            Debug.Log("ObjectCollision������ɂ��Ă��Ȃ���I");
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
    /// UnityChan���_���[�W���󂯂�
    /// </summary>
    private void UnityChanDamage()
    {
        if (isDown || ThisGameManager.instance.isStageCrear)
        {
            return;
        }

        // �_���[�W���󂯂�
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
    /// �R���e�B�j���[����
    /// </summary>
    public void ContinueUnityChan()
    {
        // �t���O�Ȃǂ������AIdle��Ԃ�
        isDown = false;
        animator.Play("Unitychan_Idle");
        isJump = false;
        isOtherJump = false;
        setRunSpeed = 0.0f;
    }

    /// <summary>
    /// �R���e�B�j���[�ҋ@��Ԕ��胁�\�b�h
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
    /// �_�E���A�j���[�V�������������Ă��邩�ǂ���
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