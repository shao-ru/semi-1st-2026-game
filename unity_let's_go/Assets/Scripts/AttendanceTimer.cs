using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class AttendanceTimer : MonoBehaviour
{
    [Header("UI参照")]
    public TextMeshProUGUI titleText;    // 「出席確認中」等のタイトルテキスト
    public TextMeshProUGUI timerText;    // カウントダウンを表示するテキスト
    public TMP_InputField codeInputField; // 認証コード入力欄
    public Button submitButton;          // 出席登録ボタン
    public GameObject portalPanel;       // ポータル画面全体のパネル

    [Header("タイマー設定")]
    public float remainingTime = 1174f;
    private bool isRunning = false;

    [Header("正解コード")]
    public string correctCode = "1234";

    [Header("シーン遷移設定")]
    public string clearSceneName = "ClearScene";      // クリア時に移動するシーン名
    public string gameOverSceneName = "GameOverScene"; // 時間切れ時に移動するシーン名

    [Header("オーディオ設定")]
    public AudioSource audioSource;     // 効果音用AudioSource（単発音用）
    public AudioSource heartbeatSource; // ★ 心臓音専用のAudioSource（Loop設定用）
    public AudioClip clearSE;           // 正解時の「ぱっぱかパー」効果音
    public AudioClip heartbeatSE;       // ★ 心臓の鼓動音ファイル

    private Color originalTitleColor;

    void Start()
    {
        if (titleText != null)
        {
            originalTitleColor = titleText.color;
            titleText.text = "出席確認中";
        }

        if (codeInputField != null)
        {
            codeInputField.onSelect.AddListener(OnInputFieldSelect);
            // ★ テキストが入力・変更された時のイベントリスナーを追加
            codeInputField.onValueChanged.AddListener(OnInputChanged);
            SetHalfWidthInput();
        }

        StartTimer();
    }

    void Update()
    {
        if (!isRunning) return;

        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            UpdateDisplay(remainingTime);
        }
        else
        {
            remainingTime = 0;
            isRunning = false;
            UpdateDisplay(0);
            TimeUp();
        }
    }

    void UpdateDisplay(float timeToDisplay)
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timeToDisplay / 60);
            int seconds = Mathf.FloorToInt(timeToDisplay % 60);
            timerText.text = string.Format("あと <color=#FF5555>{0:00}</color>分<color=#FF5555>{1:00}</color>秒", minutes, seconds);
        }
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    // ★ テキストに入力がある間だけ心臓音を鳴らす処理
    private void OnInputChanged(string input)
    {
        // 1文字以上入力されていて、かつタイマー作動中の場合
        if (!string.IsNullOrEmpty(input) && isRunning)
        {
            PlayHeartbeat();
        }
        else
        {
            StopHeartbeat();
        }
    }

    // ★ 心臓音再生
    private void PlayHeartbeat()
    {
        if (heartbeatSource != null && heartbeatSE != null)
        {
            if (!heartbeatSource.isPlaying)
            {
                heartbeatSource.clip = heartbeatSE;
                heartbeatSource.loop = true; // 繰り返し再生
                heartbeatSource.Play();
            }
        }
    }

    // ★ 心臓音停止
    private void StopHeartbeat()
    {
        if (heartbeatSource != null && heartbeatSource.isPlaying)
        {
            heartbeatSource.Stop();
        }
    }

    public void OnSubmit()
    {
        // 送信したら心臓音を止める
        StopHeartbeat();

        if (codeInputField.text == correctCode)
        {
            // 【正解の時】
            isRunning = false;

            if (submitButton != null) submitButton.interactable = false;

            StartCoroutine(GameClearSequence());
        }
        else
        {
            // 【不正解の時】
            if (titleText != null)
            {
                titleText.color = originalTitleColor;
                titleText.text = "コードが違う";
            }

            codeInputField.text = "";
            SetHalfWidthInput();
        }
    }

    private IEnumerator GameClearSequence()
    {
        if (audioSource != null && clearSE != null)
        {
            audioSource.PlayOneShot(clearSE);
        }

        if (titleText != null)
        {
            titleText.color = Color.green;
            titleText.text = "出席";
        }

        yield return new WaitForSeconds(3.0f);

        if (titleText != null)
        {
            titleText.color = Color.yellow;
            titleText.text = "★！CLEAR! ★";
        }

        yield return new WaitForSeconds(5.0f);

        SceneManager.LoadScene(clearSceneName);
    }

    private void OnInputFieldSelect(string text)
    {
        Input.imeCompositionMode = IMECompositionMode.Off;
    }

    private void SetHalfWidthInput()
    {
        Input.imeCompositionMode = IMECompositionMode.Off;
        codeInputField.ActivateInputField();
    }

    void TimeUp()
    {
        // タイムアップ時も心臓音を止める
        StopHeartbeat();

        if (titleText != null)
        {
            titleText.color = originalTitleColor;
            titleText.text = "欠席";
        }

        if (submitButton != null)
        {
            submitButton.interactable = false;
        }

        StartCoroutine(GameOverSequence());
    }

    private IEnumerator GameOverSequence()
    {
        yield return new WaitForSeconds(2.0f);

        if (titleText != null)
        {
            titleText.color = Color.white;
            titleText.text = "ゲームオーバー";
        }

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene(gameOverSceneName);
    }
}