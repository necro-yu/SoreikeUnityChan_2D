using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMChange : MonoBehaviour
{
    [Header("プレイヤー判定")] public PlayerTriggerCheck playerTriggerCheck;
    [SerializeField] private AudioSource[] _audios;
    [Range(0, 0.1f)] public float _mixRate = 0;

    private bool justOne = false;


    // Start is called before the first frame update
    void Start()
    {
        if (playerTriggerCheck == null)
        {
            Debug.Log("トリガーの設定が足りません。");
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTriggerCheck.isOn)
        {
            if (!justOne)
            {
                _audios[0].Play();
                justOne = true;
            }
            _audios[0].volume = 0.1f - _mixRate;
            _audios[1].volume = _mixRate;
        }
        else
        {
            _audios[0].volume = _mixRate;
            _audios[1].volume = 0.1f - _mixRate;
        }
    }
}
