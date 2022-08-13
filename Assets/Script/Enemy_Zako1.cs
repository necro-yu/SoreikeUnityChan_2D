using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Zako1 : MonoBehaviour
{
    #region// �C���X�y�N�^�[�Őݒ肷�鍀��
    [Header("���Z�X�R�A")] public int enemyScore = 100;
    [Header("�ړ����x")] public float speed = 5.0f;
    [Header("�d��")] public float gravity = 0.7f;
    [Header("��ʊO�ł��s�����邩")] public bool nonVisible;
    [Header("�ېH����X�N���v�g")] public Enemy_CollisionCheck eCollisioncheck;

    #endregion

    #region// �v���C�x�[�g�ϐ��Q
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
                // ��ʊO�̊Ԃ̓X���[�v���邱�Ƃŕ��׌y���ɂȂ�
                rigidbody2D.Sleep();
            }
        }
        else
        {
            // ���񂾍ۂ̃A�j���[�V�����E�ȈՂȃG�t�F�N�g�Ȃ�
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
                // ���ꂽ�Ƃ��ɃR���C�_�[��؂��Ă���̂œ������Ă����Ȃ�
                transform.Rotate(new Vector3(0, 0, 7));
                justOne = false;
            }
        }
    }
}
