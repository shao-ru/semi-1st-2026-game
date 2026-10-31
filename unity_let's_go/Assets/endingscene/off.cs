using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "Title"; // 移動先のシーン名
    [SerializeField] private float delayTime = 3.0f;         // 待つ秒数
    [SerializeField] private KeyCode skipKey = KeyCode.Tab;   // スキップキーをTabに変更

    private bool isTransitioning = false; // 二重遷移防止フラグ

    void Start()
    {
        StartCoroutine(LoadSceneAfterDelay());
    }

    void Update()
    {
        // 待機中にTabキーが押されたら即時遷移
        if (!isTransitioning && Input.GetKeyDown(skipKey))
        {
            ChangeScene();
        }
    }

    IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(delayTime);

        // キー入力で既に遷移していなければ実行
        if (!isTransitioning)
        {
            ChangeScene();
        }
    }

    private void ChangeScene()
    {
        isTransitioning = true;
        SceneManager.LoadScene(nextSceneName);
    }
}