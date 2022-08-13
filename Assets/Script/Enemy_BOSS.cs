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
                // ��ʊO�̊Ԃ̓X���[�v���邱�Ƃŕ��׌y���ɂȂ�
                rigidbody2D.Sleep();
            }
        }
        else
        {
            // ���񂾍ۂ̃A�j���[�V�����E�ȈՂȃG�t�F�N�g�Ȃ�
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
                // ���ꂽ�Ƃ��ɃR���C�_�[��؂��Ă���̂œ������Ă����Ȃ�
                transform.Rotate(new Vector3(0, 0, 7));
            }
        }
    }

    /// <summary>
    /// �{�X�����X�^�[�ւ̃_���[�W����
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
