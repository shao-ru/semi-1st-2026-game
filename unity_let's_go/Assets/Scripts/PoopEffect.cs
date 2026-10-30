using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PoopEffect : MonoBehaviour
{
    public static PoopEffect Instance;

    [Header("画面の汚れ画像")]
    [SerializeField] private Image dirtImage;

    [Header("汚れの表示時間")]
    [SerializeField] private float displayTime = 3f;

    [Header("フェードアウト時間")]
    [SerializeField] private float fadeDuration = 0.8f;

    private Coroutine dirtCoroutine;

    private void Awake()
    {
        Instance = this;

        if (dirtImage != null)
        {
            Color color = dirtImage.color;
            color.a = 0f;
            dirtImage.color = color;

            dirtImage.gameObject.SetActive(false);
        }
    }

    public void ShowDirt()
    {
        if (dirtImage == null)
        {
            Debug.LogWarning("汚れ画像が設定されていません！");
            return;
        }

        if (dirtCoroutine != null)
        {
            StopCoroutine(dirtCoroutine);
        }

        dirtCoroutine = StartCoroutine(ShowDirtCoroutine());
    }

    private IEnumerator ShowDirtCoroutine()
    {
        dirtImage.gameObject.SetActive(true);

        Color color = dirtImage.color;
        color.a = 1f;
        dirtImage.color = color;

        // 汚れを3秒間表示
        yield return new WaitForSeconds(displayTime);

        // 徐々に透明にする
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            color.a = Mathf.Lerp(
                1f,
                0f,
                elapsed / fadeDuration
            );

            dirtImage.color = color;

            yield return null;
        }

        // 完全に消す
        color.a = 0f;
        dirtImage.color = color;

        dirtImage.gameObject.SetActive(false);

        dirtCoroutine = null;
    }
}
///　　　　 　　 ／⌒ヽ
///　　⊂二二二（　＾ω＾）二⊃
///　　　　　 　|　　　 / 　　　　ﾌﾞｰﾝ
///　　　　 　 （　ヽノ
///　　　　　　 ﾉ>ノ
///　　　 三　　レﾚ　　　我が名は「内藤ホライゾン」なんだお。My name is Naitou Horaizonn daooooooooooo.