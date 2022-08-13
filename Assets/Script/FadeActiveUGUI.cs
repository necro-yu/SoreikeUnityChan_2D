using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeActiveUGUI : MonoBehaviour
{
    [Header("�t�F�[�h�X�s�[�h")] public float speed = 1.0f;
    [Header("�㏸��")] public float moveDis = 10.0f;
    [Header("�㏸����")] public float moveTime = 1.0f;
    [Header("�L�����o�X�O���[�v")] public CanvasGroup canvasGroup;
    [Header("�v���C���[����")] public PlayerTriggerCheck playerTriggerCheck;

    private Vector3 defaultPos;
    private float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        // ������
        if(canvasGroup == null && playerTriggerCheck == null)
        {
            Debug.Log("�C���X�y�N�^�[�̐ݒ肪����܂���");
            Destroy(this);
        }
        else
        {
            // �t�F�[�h�C�����Ȃ��猳�̈ʒu�ɖ߂�`���쐬
            canvasGroup.alpha = 0.0f;
            defaultPos = canvasGroup.transform.position;
            canvasGroup.transform.position = defaultPos - Vector3.up * moveDis;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // ������ɓ����Ă����ꍇ
        if (playerTriggerCheck.isOn)
        {
            if (canvasGroup.transform.position.y < defaultPos.y || canvasGroup.alpha < 1.0f)
            {
                // �㏸���Ȃ���t�F�[�h�C�����s��
                canvasGroup.alpha = timer / moveTime;
                // ���Ԃ𒲐߂��邽�߂ɁA�X�s�[�h�Ǝ��Ԃ�����Ɋ|���Ă���
                canvasGroup.transform.position += Vector3.up * (moveDis / moveTime) * speed * Time.deltaTime;
                timer += speed * Time.deltaTime;
            }
            // �t�F�[�h�C�������̏ꍇ
            else
            {
                // ���m�Ȑ��l�����Ă���
                canvasGroup.alpha = 1.0f;
                canvasGroup.transform.position = defaultPos;
            }
        }
        // �v���C���[������O�̏ꍇ
        else
        {
            if (canvasGroup.transform.position.y > defaultPos.y - moveDis || canvasGroup.alpha > 0.0f)
            {
                canvasGroup.alpha = timer / moveTime;
                canvasGroup.transform.position -= Vector3.up * (moveDis / moveTime) * speed * Time.deltaTime;
                timer -= speed * Time.deltaTime;
            }
            // �t�F�[�h�A�E�g����
            else
            {
                timer = 0.0f;
                canvasGroup.alpha = 0.0f;
                canvasGroup.transform.position = defaultPos - Vector3.up * moveDis;
            }
        }

    }
}
