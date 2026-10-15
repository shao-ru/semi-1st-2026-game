using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;       // スライダーなどのUI機能を扱うために追加 [4]
using UnityEngine.Audio;    // オーディオミキサーを扱うために追加 [4]

public class AudioConfig : MonoBehaviour
{
    // インスペクターから登録するオーディオソース、スライダー、ミキサー [3, 4, 6]
    [SerializeField] private AudioSource seAudioSource;
    [SerializeField] private AudioSource bgmAudioSource;
    
    [SerializeField] private Slider seSlider;
    [SerializeField] private Slider bgmSlider;
    
    [SerializeField] private AudioMixer audioMixer;

    void Start()
    {
        // BGMスライダーの値が変わったときのイベント [4]
        bgmSlider.onValueChanged.AddListener((value) =>
        {
            // 値が確実に0〜1の間になるように制限 [5]
            value = Mathf.Clamp01(value);
            
            // 対数を使って音量（デシベル）を計算 [5]
            float decibel = 20f * Mathf.Log10(value);
            
            // スライダーが0（無音）のときに値がマイナス無限大になるのを防ぐため、-80〜0の間に丸める [5]
            decibel = Mathf.Clamp(decibel, -80f, 0f);
            
            // オーディオミキサーの「BGM」パラメータにセット [6]
            audioMixer.SetFloat("BGM", decibel);
        });

        // SEスライダーの値が変わったときのイベント [6]
        seSlider.onValueChanged.AddListener((value) =>
        {
            value = Mathf.Clamp01(value);
            float decibel = 20f * Mathf.Log10(value);
            decibel = Mathf.Clamp(decibel, -80f, 0f);
            
            // オーディオミキサーの「SE」パラメータにセット [6]
            audioMixer.SetFloat("SE", decibel);
        });
    }

    void Update()
    {
        // スペースキーが押されたときに、テスト用にSEを鳴らす処理 [1, 3]
        if (Input.GetKeyDown(KeyCode.Space))
        {
            seAudioSource.Play();
        }
    }
}