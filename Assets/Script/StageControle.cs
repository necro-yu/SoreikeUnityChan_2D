using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageControle : MonoBehaviour
{
    [Header("�v���C���[�̃Q�[���I�u�W�F�N�g")] public GameObject plyaerObj;
    [Header("�R���e�B�j���[�ʒu")] public GameObject[] continuePoint;
    [Header("�Q�[���I�[�o�[")] public GameObject gameOverObj;
    [Header("�X�e�[�W�N���A")] public GameObject stageClearObj;
    [Header("�t�F�[�h")] public FadeImage fade;

    private UnityChanController unityChanController;
    private int nextStageNum;
    private bool startFade = false;
    private bool nextStageGo = false;
    private bool retryGame = false;
    private bool doGameOver = false;
    private bool doClear = false;
    private bool doScoreChange = false;


    // Start is called before the first frame update
    void Start()
    {
        if (plyaerObj != null && continuePoint != null && continuePoint.Length > 0
            && gameOverObj != null && fade != null && !retryGame)
        {
            gameOverObj.SetActive(false);
            stageClearObj.SetActive(false);

            // �����J�n�ʒu(�Ō�ɓ���ł���R���e�B�j���[�|�C���g)
            plyaerObj.transform.position = continuePoint[ThisGameManager.instance.continueNum].transform.position;

            unityChanController = plyaerObj.GetComponent<UnityChanController>();
            if (unityChanController == null)
            {
                Debug.Log("�v���C���[����Ȃ����̂��A�^�b�`����Ă����B");
            }
        }
        else
        {
            Debug.Log("�ݒ肪����Ă��Ȃ���");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // �Q�[���I�[�o�[�̏���
        if (ThisGameManager.instance.isGameOver && !doGameOver)
        {
            gameOverObj.SetActive(true);
            doGameOver = true;
            Debug.Log("�Q�[���I�[�o�[����");
        }

        // �X�e�[�W�N���A����
        if (ThisGameManager.instance.isStageCrear && !doClear)
        {
            stageClearObj.SetActive(true);
            ThisGameManager.instance.PlaySE(SESetScript.instance.stageClearSE);
            doClear = true;
            Debug.Log("�X�e�[�W�N���A����");
        }

        // �X�e�[�W�؂�ւ�
        if (fade != null && startFade && !doScoreChange)
        {
            if (fade.IsFadeOutComplete())
            {
                // ���g���C�Q�[��
                if (retryGame)
                {
                    ThisGameManager.instance.ContinueGame();
                    ThisGameManager.instance.PlaySE(SESetScript.instance.retrySE);
                    Debug.Log("���g���C�Q�[��");
                }
                // ���̃X�e�[�W��ݒ�
                else
                {
                    ThisGameManager.instance.stageNum = nextStageNum;
                }

                // ��ʈړ�
                if (retryGame || nextStageGo)
                {
                    ThisGameManager.instance.isStageCrear = false;
                    // �܂����̃X�e�[�W�͂ł��Ă��Ȃ��B
                    SceneManager.LoadScene("Stage" + ThisGameManager.instance.stageNum);
                    doScoreChange = true;
                    Debug.Log("���[�h�V�[��");
                }
            }
        }
    }

    /// <summary>
    /// Retry�{�^����������Save�ʒu����ĊJ����
    /// </summary>
    public void Retry()
    {
        ChangeScene(ThisGameManager.instance.stageNum);
        retryGame = true;
        Debug.Log("���g���C�{�^��");
    }

    /// <summary>
    /// Next�{�^���������Ɏ��̃X�e�[�W�ւ���
    /// </summary>
    public void NextStage()
    {
        nextStageGo = true;
        // Stage2���܂��Ȃ�����ThankyouForPlaying�ɂȂ�
        ChangeScene(ThisGameManager.instance.stageNum + 1);
        Debug.Log("���̃X�e�[�W");
    }

    /// <summary>
    /// �X�e�[�W�؂�ւ��p���\�b�h
    /// </summary>
    /// <param name="num">�؂�ւ���X�e�[�W�ԍ�</param>
    public void ChangeScene(int num)
    {
        if (fade != null)
        {
            nextStageNum = num;
            fade.StartFadeOut();
            startFade = true;
            Debug.Log("�`�F���W�X�e�[�W");
        }
    }
}
