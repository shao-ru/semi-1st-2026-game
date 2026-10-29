using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        int minutes = Mathf.FloorToInt(timeToDisplay / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("あと <color=#FF5555>{0:00}</color>分<color=#FF5555>{1:00}</color>秒", minutes, seconds);
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

            // 「出席」と表示し、文字色を緑色に変更
            if (titleText != null)
            {
                titleText.text = "出席";
                titleText.color = Color.green; // ★文字色を緑色に変更
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
                titleText.color = originalTitleColor; // 元の色に戻す
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

    void TimeUp()
    {
        if (titleText != null)
        {
            titleText.color = originalTitleColor; // 元の色に戻す
            titleText.text = "欠席";
        }
        if (submitButton != null) submitButton.interactable = false;
    }
}