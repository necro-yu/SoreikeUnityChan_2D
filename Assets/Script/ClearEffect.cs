using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearEffect : MonoBehaviour
{
    [Header("�g��k���̃A�j���[�V�����J�[�u")] public AnimationCurve animationCurve;
    [Header("�X�e�[�W�R���g���[���[")] public StageControle stageControle;

    private bool comp = false;
    private float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (!comp)
        {
            if(timer < 1.0f)
            {
                transform.localScale = Vector3.one * animationCurve.Evaluate(timer);
                timer += Time.deltaTime;
            }
            else
            {
                transform.localScale = Vector3.one;
                comp = true;
            }
        }
    }
}
