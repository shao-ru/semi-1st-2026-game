using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [Header("タイマー設定")]
    public float timeRemaining = 60f;
    private bool isTimerRunning = false;

    [Header("UI参照")]
    public TextMeshProUGUI timerText;
    public GameObject gameOverPanel; // ゲームオーバー画面のパネル

    void Start()
    {
        isTimerRunning = true;

        // 開始時はゲームオーバーパネルを確実に非表示にしておく
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (isTimerRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerDisplay(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                isTimerRunning = false;
                UpdateTimerDisplay(timeRemaining);
                ShowGameOver();
            }
        }
    }

    // タイマー表示のフォーマット
    void UpdateTimerDisplay(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        // 「TIME: 」を削除し、時間数字だけを表示するように変更
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // 0秒になったときに呼ばれる処理
    void ShowGameOver()
    {
        Debug.Log("Game Over!");

        // ゲームオーバーパネルを表示
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // 必要に応じてゲーム全体の動きを止める場合
        // Time.timeScale = 0f;
    }

    // ボタンから呼び出すリトライ用関数
    public void RetryGame()
    {
        // 時間の進みを元に戻してからシーンを再読み込み
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}