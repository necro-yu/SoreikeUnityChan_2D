using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreItem : MonoBehaviour
{
    [Header("���Z����X�R�A")] public int addScore;
    [Header("�v���C���[�̔���")] public PlayerTriggerCheck playerTriggerCheck;

    // Update is called once per frame
    void Update()
    {
        if (playerTriggerCheck.isOn)
        {
            if(ThisGameManager.instance != null)
            {
                // ��Item�ɃR���C�_�[���t����̂�Y�ꂸ��
                ThisGameManager.instance.scoreNum += addScore;
                Destroy(this.gameObject);
            }
        }
    }
}
