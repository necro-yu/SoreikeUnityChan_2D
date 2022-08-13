using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpliteRendererBlinker : MonoBehaviour
{
    // �_�ł�����Ώ�
    [SerializeField] private SpriteRenderer _target;

    // �_�Ŏ���[s]
    [SerializeField] private float _cycle = 1.0f;

    private bool _isBlinking;
    private float _defaultAlpha;
    private float _time;

    /// <summary>
    /// �_�ł��J�n����
    /// </summary>
    public void BeginBlink()
    {
        // �_�Œ��͉������Ȃ�
        if (_isBlinking) return;

        _isBlinking = true;

        // ���Ԃ��J�n���_�ɖ߂�
        _time = 0;
    }

    /// <summary>
    /// �_�ł��I������
    /// </summary>
    public void EndBlink()
    {
        _isBlinking = false;

        // ������Ԃ̃A���t�@�l�ɖ߂�
        SetAlpha(_defaultAlpha);
    }

    private void Start()
    {
        _defaultAlpha = _target.color.a;
    }

    private void Update()
    {
        // �_�ł��Ă��Ȃ��Ƃ��͉������Ȃ�
        if (!_isBlinking) return;

        // �����������o�߂�����
        _time += Time.deltaTime * 2.0f;

        // ����cycle�ŌJ��Ԃ��l�̎擾
        // 0�`cycle�͈̔͂̒l��������
        var repeatValue = Mathf.Repeat(_time, _cycle);

        // ��������time�ɂ����閾�ŏ�Ԃ𔽉f
        // Image�̃A���t�@�l��ύX���Ă���
        SetAlpha(repeatValue >= _cycle * 0.5f ? 1 : 0);
    }

    // Image�̃A���t�@�l��ύX���郁�\�b�h
    private void SetAlpha(float alpha)
    {
        var color = _target.color;
        color.a = alpha;
        _target.color = color;
    }
}
