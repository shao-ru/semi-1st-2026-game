using UnityEngine;
using UnityEngine.SceneManagement;

public class TimelineSceneChanger : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "Title"; // 遷移先のシーン名

    // タイムラインから呼び出すメソッド
    public void ChangeScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
