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
    [Header("車の出現音")]
    [SerializeField] private AudioClip carSpawnSound;
    [Range(0f, 1f)]
    [SerializeField] private float soundVolume = 1f;
    [Header("音が聞こえるX座標の範囲")]
    [SerializeField] private float soundRangeX = 5f;
    private SpriteRenderer spriteRenderer;
    private Collider2D carCollider;
    private AudioSource audioSource;
    private Transform playerTransform;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        carCollider = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        // 2Dサウンドとして再生
        audioSource.spatialBlend = 0f;
        // Playerタグのオブジェクトを探す
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogWarning("Playerタグのオブジェクトが見つかりません！");
        }
        HideCar();
        StartCoroutine(CarRoutine());
    }
    private IEnumerator CarRoutine()
    {
        while (true)
        {
            if (signalController.IsGreen())
            {
                HideCar();
                yield return null;
                continue;
            }
            Debug.Log("赤信号になりました。車を待機します。");
            float delay = Random.Range(
                minSpawnDelay,
                maxSpawnDelay
            );
            yield return new WaitForSeconds(delay);
            if (signalController.IsGreen())
            {
                HideCar();
                continue;
            }
            ShowCar();
            Debug.Log("車が出現しました！");
            // プレイヤーとのX座標の距離を確認
            if (carSpawnSound != null && playerTransform != null)
            {
                float distanceX = Mathf.Abs(
                    transform.position.x - playerTransform.position.x
                );
                // 指定範囲内にいるときだけ音を鳴らす
                if (distanceX <= soundRangeX)
                {
                    audioSource.PlayOneShot(
                        carSpawnSound,
                        soundVolume
                    );
                }
            }
            float timer = 0f;
            while (timer < visibleTime)
            {
                if (signalController.IsGreen())
                {
                    break;
                }
                timer += Time.deltaTime;
                yield return null;
            }
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