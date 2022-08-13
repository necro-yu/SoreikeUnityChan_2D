using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuePoint : MonoBehaviour
{
    #region
    [Header("�R���e�B�j���[��ԍ�")] public int continueNum;
    [Header("�X�s�[�h")] public float speed = 2.0f;
    [Header("������")] public float moveDis = 3.0f;
    [Header("�v���C���[����")] public PlayerTriggerCheck playerTriggerCheck;
    [Header("�擾�A�j���[�V����")] public AnimationCurve animationCurve;

    private bool on = false;
    private float kakudo = 0.0f;
    private Vector3 defaultPos;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        if (playerTriggerCheck == null || SESetScript.instance.continueSE == null)
        {
            Debug.Log("�g���K�[��SE�̐ݒ肪����܂���B");
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
                //sin�J�[�u�ŐU��������
                transform.position = defaultPos + Vector3.up * moveDis * Mathf.Sin(kakudo * Mathf.Deg2Rad);

                //�r�����炿�����Ⴍ�Ȃ�
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
