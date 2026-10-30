using UnityEngine;
using UnityEngine.SceneManagement; // シーン切り替えに必要な名前空間

public class SceneChanger : MonoBehaviour
{
    // ボタンのOnClickイベントから呼び出す関数
    public void LoadGameOverScene()
    {
        SceneManager.LoadScene("Title");
    }
}