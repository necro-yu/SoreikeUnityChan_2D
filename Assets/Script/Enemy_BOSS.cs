using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BOSS : MonoBehaviour
{
    #region// �C���X�y�N�^�[�Őݒ肷�鍀��
    [Header("���Z�X�R�A")] public int enemyScore;
    [Header("�ړ����x")] public float speed;
    [Header("�d��")] public float gravity = 0.7f;
    [Header("��ʊO�ł��s�����邩")] public bool nonVisible;
    [Header("�ېH����X�N���v�g")] public Enemy_CollisionCheck eCollisioncheck;
    [Header("�_���[�W���̓_��")] public SpliteRendererBlinker bossBlinker;
    [Header("�{�X�̃��C�t")] public int bossLife;
    #endregion

    #region// �v���C�x�[�g�ϐ��Q
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
            Debug.Log("�_��");
        }
        else
        {
            bossBlinker.EndBlink();
            Debug.Log("�_�ŏI��");
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
                // �ǂ�G�Ŕ��]����
                if (eCollisioncheck.isOn)
                {
                    rightTleftF = !rightTleftF;
                }

                // ���]����
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

                // ���������ʊO�ł�����
                nonVisible = true;
            }
            else
            {
                // ��ʊO�̊Ԃ̓X���[�v���邱�Ƃŕ��׌y���ɂȂ�
                rigidbody2D.Sleep();
            }
        }
        else
        {
            if (objectCollision.playerStepOn)
            {
                objectCollision.playerStepOn = false;
            }
            // �_���[�W���󂯂����̏���
            if (!isDamage)
            {
                isDamage = true;
                --bossLife;
                ThisGameManager.instance.PlaySE(SESetScript.instance.stepOnSE);
                Debug.Log($"{bossLife}");
                Debug.Log("�_���[�W�t���O");
            }
        }

        // ���񂾍ۂ̃A�j���[�V�����E�ȈՂȃG�t�F�N�g�Ȃ�
        if (bossLife <= 0)
        {
            Debug.Log("�|����");
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
            // ���ꂽ�Ƃ��ɃR���C�_�[��؂��Ă���̂œ������Ă����Ȃ�
            transform.Rotate(new Vector3(0, 0, 7));
        }

        timer += Time.deltaTime;
        if (damagetime < timer)
        {
            isDamage = false;
            Debug.Log("�_���[�W�t���O�߂���");
            Debug.Log($"{objectCollision.playerStepOn}");
            timer = 0.0f;
        }
    }
}
