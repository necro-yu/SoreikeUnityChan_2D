using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneController : MonoBehaviour
{
	[Header("フェード")] public FadeImage fade;
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
			// LoadSceneMode.Singleにしているのは、タイトルなので情報を残す必要がないと思うため。
			SceneManager.LoadScene("Stage1", LoadSceneMode.Single);
			goNextScene = true;
		}
	}
}