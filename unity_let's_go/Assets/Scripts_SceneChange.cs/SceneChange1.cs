using UnityEngine;
using UnityEngine.SceneManagement; // シーン管理に必要な名前空間

public class SceneChange1 : MonoBehaviour
{
    // ボタンのOnClickから呼び出すメソッド
    // 引数 sceneName に移動先のシーン名を入力します
    public void ChangeSceneeasy(string stage_story_easy)
    {
        SceneManager.LoadScene(stage_story_easy);
    }

    public void ChangeScenenomal(string stage_story)
    {
        SceneManager.LoadScene(stage_story);
    }

    public void ChangeScenehald(string stage_story_hard)
    {
        SceneManager.LoadScene(stage_story_hard);
    }

    public void ChangeSceneulutimate(string stage_story_ultimate)
    {
        SceneManager.LoadScene(stage_story_ultimate);
    }
}