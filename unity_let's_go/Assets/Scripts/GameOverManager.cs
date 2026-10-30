using UnityEngine;
using UnityEngine.SceneManagement; // シーン切り替えに必須

public class GameOverManager : MonoBehaviour
{
    // リトライボタン用（ゲームメインのシーン名を指定）
    public void RetryGame()
    {
        // "GameScene" の部分は実際のメインゲームのシーン名に変えてください
        SceneManager.LoadScene("stage_mountain");
    }

    // タイトルへ戻るボタン用
    public void GoToTitle()
    {
        // "TitleScene" の部分は実際のタイトルシーン名に変えてください
        SceneManager.LoadScene("Title");
    }
}