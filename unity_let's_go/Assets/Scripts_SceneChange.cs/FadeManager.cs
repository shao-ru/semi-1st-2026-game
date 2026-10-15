using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
    [SerializeField] private Image fadeImage; // フェード用Image
    [SerializeField] private float fadeDuration = 1.0f; // フェードにかける時間（秒）

    // シーン遷移を呼び出すメソッド
    public void ChangeSceneWithFade(string sceneName)
    {
        StartCoroutine(FadeOutAndLoadScene(sceneName));
    }

    private IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        // 画面クリックなどの誤操作を防ぐためにRaycastを有効化
        fadeImage.raycastTarget = true;

        float time = 0f;
        Color color = fadeImage.color;

        // アルファ値を 0 から 1 へ変化させる（フェードアウト）
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            color.a = Mathf.Clamp01(time / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        // 透明度が1になったらシーンをロード
        SceneManager.LoadScene(sceneName);
    }
}