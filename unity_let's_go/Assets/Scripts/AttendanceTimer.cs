using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; // ★ シーン遷移に使用
using System.Collections;          // ★ コルーチン待機に使用

public class AttendanceTimer : MonoBehaviour
{
    [Header("UI参照")]
    public TextMeshProUGUI titleText;    // 「出席確認中」等のタイトルテキスト
    public TextMeshProUGUI timerText;    // カウントダウンを表示するテキスト
    public TMP_InputField codeInputField; // 認証コード入力欄
    public Button submitButton;          // 出席登録ボタン
    public GameObject portalPanel;       // ポータル画面全体のパネル

    [Header("タイマー設定")]
    public float remainingTime = 1174f; // 19分34秒
    private bool isRunning = false;

    [Header("正解コード")]
    public string correctCode = "1234";

    [Header("シーン遷移設定")]
    public string gameOverSceneName = "GameOverScene"; // ★ 遷移先のシーン名を入力

    // 元の文字色を保持しておくための変数
    private Color originalTitleColor;

    void Start()
    {
        // タイトルテキストの初期カラーを覚えておく
        if (titleText != null)
        {
            originalTitleColor = titleText.color;
            titleText.text = "出席確認中";
        }

        // 自動で半角にする設定
        if (codeInputField != null)
        {
            codeInputField.onSelect.AddListener(OnInputFieldSelect);
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

    public void OnSubmit()
    {
        if (codeInputField.text == correctCode)
        {
            // 【正解の時】
            isRunning = false;

            if (titleText != null)
            {
                titleText.text = "出席";
                titleText.color = Color.green;
            }

            Debug.Log("出席登録完了！");

            // 1秒後にパネルを閉じる処理
            Invoke("ClosePanel", 1.0f);
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

    private void OnInputFieldSelect(string text)
    {
        Input.imeCompositionMode = IMECompositionMode.Off;
    }

    private void SetHalfWidthInput()
    {
        Input.imeCompositionMode = IMECompositionMode.Off;
        codeInputField.ActivateInputField();
    }

    void ClosePanel()
    {
        if (portalPanel != null) portalPanel.SetActive(false);
    }

    // ★ 時間切れ時に呼ばれるメソッド
    void TimeUp()
    {
        if (titleText != null)
        {
            titleText.color = originalTitleColor;
            titleText.text = "欠席";
        }

        if (submitButton != null)
        {
            submitButton.interactable = false;
        }

        // コルーチン（非同期処理）を開始
        StartCoroutine(GameOverSequence());
    }

    // ★ 演出とシーン遷移を行う処理
    private IEnumerator GameOverSequence()
    {
        // 2秒間「欠席」のまま待機
        yield return new WaitForSeconds(2.0f);

        // 「ゲームオーバー」に表示変更
        if (titleText != null)
        {
            titleText.color = Color.white;
            titleText.text = "ゲームオーバー";
        }

        // さらに1.5秒待機
        yield return new WaitForSeconds(1.5f);

        // ゲームオーバーシーンへ移動
        SceneManager.LoadScene(gameOverSceneName);
    }
}