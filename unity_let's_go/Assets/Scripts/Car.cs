using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour
{
    [Header("車が出現するまで")]
    [SerializeField] private float minSpawnDelay = 2f;
    [SerializeField] private float maxSpawnDelay = 5f;

    [Header("車が出ている時間")]
    [SerializeField] private float visibleTime = 3f;

    [Header("信号")]
    [SerializeField] private SignalController signalController;

    private SpriteRenderer spriteRenderer;
    private Collider2D carCollider;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        carCollider = GetComponent<Collider2D>();

        // 最初は車を隠す
        HideCar();

        // 車の処理を開始
        StartCoroutine(CarRoutine());
    }

    private IEnumerator CarRoutine()
    {
        while (true)
        {
            // -------------------------
            // 青信号なら待つ
            // -------------------------

            if (signalController.IsGreen())
            {
                HideCar();

                yield return null;

                continue;
            }

            // -------------------------
            // 赤信号になった
            // -------------------------

            Debug.Log("赤信号になりました。車を待機します。");

            // ランダムな時間を決める
            float delay = Random.Range(
                minSpawnDelay,
                maxSpawnDelay
            );

            Debug.Log("車が出るまで " + delay + " 秒待ちます。");

            yield return new WaitForSeconds(delay);

            // -------------------------
            // 待っている間に
            // 青になっていないか確認
            // -------------------------

            if (signalController.IsGreen())
            {
                HideCar();
                continue;
            }

            // -------------------------
            // 車を出す
            // -------------------------

            ShowCar();

            Debug.Log("車が出現しました！");

            // -------------------------
            // 車が出ている時間
            // -------------------------

            float timer = 0f;

            while (timer < visibleTime)
            {
                // 青になったら即消す
                if (signalController.IsGreen())
                {
                    break;
                }

                timer += Time.deltaTime;

                yield return null;
            }

            // -------------------------
            // 車を消す
            // -------------------------

            HideCar();

            Debug.Log("車が消えました。");
        }
    }

    private void ShowCar()
    {
        spriteRenderer.enabled = true;
        carCollider.enabled = true;
    }

    private void HideCar()
    {
        spriteRenderer.enabled = false;
        carCollider.enabled = false;
    }
}