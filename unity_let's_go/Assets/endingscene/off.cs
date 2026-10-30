using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // シーン管理に必要

public class SceneController : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "Title"; // 移動先のシーン名
    [SerializeField] private float delayTime = 3.0f;             // 待つ秒数（3秒）

    void Start()
    {
        // コルーチンを開始
        StartCoroutine(LoadSceneAfterDelay());
    }

    IEnumerator LoadSceneAfterDelay()
    {
        // 指定した秒数だけ待つ
        yield return new WaitForSeconds(delayTime);

        // シーンを読み込む（変数 nextSceneName を指定し、閉じカッコとセミコロンを追加）
        SceneManager.LoadScene(nextSceneName);
    }
}
