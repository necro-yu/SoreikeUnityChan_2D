using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneController : MonoBehaviour
{
	[Header("�t�F�[�h")] public FadeImage fade;
	private bool goNextScene = false;

	public void OnClick()
	{
		Debug.Log("Go Next Scene!");
		fade.StartFadeOut();
		ThisGameManager.instance.PlaySE(SESetScript.instance.continueSE);
	}

	private void Update()
	{
		if (!goNextScene && fade.IsFadeOutComplete())
		{
			// LoadSceneMode.Single�ɂ��Ă���̂́A�^�C�g���Ȃ̂ŏ����c���K�v���Ȃ��Ǝv�����߁B
			SceneManager.LoadScene("Stage1", LoadSceneMode.Single);
			goNextScene = true;
		}
	}
}