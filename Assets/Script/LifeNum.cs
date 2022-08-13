using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeNum : MonoBehaviour
{
    private Text lifeText = null;
    private int oldLife = 0;

    // Start is called before the first frame update
    void Start()
    {
        lifeText = GetComponent<Text>();
        if (ThisGameManager.instance != null)
        {
            lifeText.text = "× " + ThisGameManager.instance.lifeNum;
        }
        else
        {
            Debug.Log("ゲームマネージャーがないよ");
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (oldLife != ThisGameManager.instance.lifeNum)
        {
            lifeText.text = "× " + ThisGameManager.instance.lifeNum;
            oldLife = ThisGameManager.instance.lifeNum;
        }
    }
}
