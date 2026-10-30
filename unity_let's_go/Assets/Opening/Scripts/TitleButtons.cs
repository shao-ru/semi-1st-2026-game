using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButtons : MonoBehaviour
{
    // 表示・非表示を切り替えたいコンフィグパネルのオブジェクト
    [SerializeField] private GameObject configPanel;

    [Header("オーディオ設定")]
    [SerializeField] private AudioSource audioSource; // 効果音を鳴らすためのAudioSource
    [SerializeField] private AudioClip buttonSE;      // ボタンを押した時に鳴らす効果音ファイル

    private bool isTransitioning = false; // ボタン連打防止フラグ

    // ボタン音を再生する共通メソッド
    private void PlayButtonSE()
    {
        if (audioSource != null && buttonSE != null)
        {
            audioSource.PlayOneShot(buttonSE);
        }
    }

    // ゲーム開始ボタン
    public void StartButton()
    {
        if (isTransitioning) return;
        StartCoroutine(LoadSceneWithSE("stage_story"));
    }

    // チュートリアルボタン
    public void TutorialButton()
    {
        if (isTransitioning) return;
        StartCoroutine(LoadSceneWithSE("stage_story(2)"));
    }

    // 効果音をしっかり鳴らしてからシーン遷移するコルーチン
    private IEnumerator LoadSceneWithSE(string sceneName)
    {
        isTransitioning = true;
        PlayButtonSE();

        // 効果音の長さ（秒）だけ待機（AudioClipが未設定の場合は0.2秒待機）
        float waitTime = (buttonSE != null) ? buttonSE.length : 0.2f;
        yield return new WaitForSeconds(waitTime);

        GlobalTimer.ResetTimer(); // 全体タイマーを最初に戻す
        SceneManager.LoadScene(sceneName);
    }

    // 設定画面を表示するメソッド
    public void ShowConfigPanel()
    {
        PlayButtonSE();
        configPanel.SetActive(true);
    }

    // 設定画面を非表示にするメソッド
    public void HideConfigPanel()
    {
        PlayButtonSE();
        configPanel.SetActive(false);
    }
}