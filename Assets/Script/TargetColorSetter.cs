using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class TargetColorSetter : MonoBehaviour
{
    [SerializeField] private UnityEngine.Color _targetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Animator _animator;
    [SerializeField] private SignalPoint _signalPoint;
    [SerializeField] private SignalOn _signal;
    [SerializeField] private AudioSource _audio;

    public void Change()
    {
        _renderer.color = _targetColor;
    }

    private void OnEnable()
    {
        _signalPoint.Entered += OnEntered;
        _signalPoint.CameOut += OnCameOut;
    }

    private void OnDisable()
    {
        _signalPoint.Entered -= OnEntered;
        _signalPoint.CameOut -= OnCameOut;
    }

    private void OnEntered()
    {
        _signal.gameObject.SetActive(true);
        _animator.SetBool("isPlaying", true);
        _renderer.enabled = true;
        _audio.enabled = true;
        var volume = StartCoroutine(AddSound());
    }

    private void OnCameOut()
    {
        _signal.gameObject.SetActive(false);
        _animator.SetBool("isPlaying", false);
        _renderer.enabled = false;
        var volume = StartCoroutine(DownSound());
        Invoke("AudioOff", 5f);
    }

    private void AudioOff()
    {
        _audio.enabled = false;
    }

    private IEnumerator AddSound()
    {
        var volume = _audio.volume;
        var waitForOneSeconds = new WaitForSeconds(0.5f);

        for (float i = 0; i <= 1; i += 0.1f)
        {
            volume += 0.1f;
            _audio.volume = volume;

            yield return waitForOneSeconds;
        }
    }

    private IEnumerator DownSound()
    {
        var volume = _audio.volume;
        var waitForOneSeconds = new WaitForSeconds(0.5f);

        for (float i = 1; i >= 0; i -= 0.1f)
        {
            volume -= 0.1f;
            _audio.volume = volume;

            yield return waitForOneSeconds;
        }
    }
}
