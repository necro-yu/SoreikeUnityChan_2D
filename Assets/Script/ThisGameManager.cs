using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThisGameManager : MonoBehaviour
{
    public static ThisGameManager instance = null;

    [Header("���݂̃X�e�[�W")] public int stageNum;
    [Header("���݂̕��A�ʒu")] public int continueNum;
    [Header("���݂̎c��")] public int lifeNum;
    [Header("�f�t�H���g�c��")] public int defaultLideNum = 3;
    [Header("�c��̏��")] public int limitLifeNum = 99;
    [HideInInspector] public bool isGameOver;
    [HideInInspector] public bool isStageCrear;

    private AudioSource audioSource = null;

    /// <summary>
    /// ���󖢎g�p
    /// </summary>
    [Header("�X�R�A")] public int scoreNum;

    // ����������
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void AddLifeNum()
    {
        if (lifeNum < limitLifeNum)
        {
            ++lifeNum;
        }
    }

    /// <summary>
    /// �c������炷���\�b�h
    /// </summary>
    public void SubLifeNum()
    {
        if (lifeNum > 0)
        {
            --lifeNum;
        }
        else
        {
            isGameOver = true;
        }
    }

    /// <summary>
    ///  �ŏ�����n�߂�ꍇ�̏���
    /// </summary>
    public void RetryGame()
    {
        isGameOver = false;
        lifeNum = defaultLideNum;
        scoreNum = 0;
        stageNum = 1;
        continueNum = 0;
    }

    /// <summary>
    /// ���߂̃Z�[�u�|�C���g����ĊJ����
    /// </summary>
    public void ContinueGame()
    {
        isGameOver = false;
        lifeNum = defaultLideNum;
    }

    /// <summary>
    /// SE��炷���\�b�h
    /// </summary>
    /// <param name="audioClip">�炷SE</param>
    public void PlaySE(AudioClip audioClip)
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(audioClip);
        }
        else
        {
            Debug.Log("GM�ɃI�[�f�B�I�\�[�X���ݒ肳��Ă��Ȃ���B");
        }
    }
}
