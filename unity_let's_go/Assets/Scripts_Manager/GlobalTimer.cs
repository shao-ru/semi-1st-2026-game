using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲーム全体で共通の残り時間タイマー。
/// ・シーンを切り替えても時間は続く（DontDestroyOnLoad）
/// ・ゲーム起動時に自動で作られるので、シーンに置く必要はない
/// ・制限時間は Assets/Resources/GlobalTimer.prefab の Inspector で設定する
///
/// 他のスクリプトからの使い方：
///   GlobalTimer.Instance.RemainingSeconds  … 残り秒数（float）
///   GlobalTimer.Instance.ElapsedSeconds    … 経過秒数（float）＝ 制限時間 − 残り秒数
///   GlobalTimer.Instance.IsTimeUp          … 時間切れなら true
///   GlobalTimer.Instance.IsRunning         … カウント中なら true（StopTimer 後は false）
///   GlobalTimer.ResetTimer()               … 残り時間を最初に戻して、カウントを再開する
///   GlobalTimer.StopTimer()                … カウントを止める（クリア時など。値はそのまま残る）
///   GlobalTimer.RetryCurrentScene()        … 残り時間を戻して、今のシーンをやり直す
///   GlobalTimer.FormatTime(秒数)            … 「3:35」のような文字列にする
///   GlobalTimer.OnTimeUp += 関数名;         … 時間切れの瞬間に呼ばれる
///
/// 例：クリア時のスコア
///   GlobalTimer.StopTimer();
///   float left = GlobalTimer.Instance.RemainingSeconds;   // 残り時間が多いほど高得点、など
/// </summary>
public class GlobalTimer : MonoBehaviour
{
    public static GlobalTimer Instance { get; private set; }

    /// <summary>時間切れになった瞬間に 1 回だけ呼ばれる</summary>
    public static event Action OnTimeUp;

    [Header("制限時間（秒）")]
    [SerializeField] private float totalSeconds = 300f;

    [Header("タイマーを止めておくシーン（タイトル画面など）")]
    [SerializeField] private string[] pausedScenes = { "Title" };

    /// <summary>残り秒数</summary>
    public float RemainingSeconds { get; private set; }

    /// <summary>時間切れなら true</summary>
    public bool IsTimeUp => RemainingSeconds <= 0f;

    /// <summary>カウント中なら true（StopTimer で止めると false）</summary>
    public bool IsRunning { get; private set; } = true;

    /// <summary>経過秒数（制限時間 − 残り秒数）</summary>
    public float ElapsedSeconds => totalSeconds - RemainingSeconds;

    /// <summary>制限時間（秒）</summary>
    public float TotalSeconds => totalSeconds;

    // Resources フォルダ内のプレハブ名
    private const string PrefabName = "GlobalTimer";

    // Play のたびに static 変数をリセット（Enter Play Mode の設定に関係なく正しく動くように）
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics()
    {
        Instance = null;
        OnTimeUp = null;
    }

    // ゲーム起動時（最初のシーンを読み込む前）に自動で作る
    // → タイトルから始めても、ステージを直接開いてテストしても動く
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void AutoCreate()
    {
        if (Instance != null) return;

        GlobalTimer prefab = Resources.Load<GlobalTimer>(PrefabName);
        GlobalTimer timer = prefab != null
            ? Instantiate(prefab)
            : new GameObject().AddComponent<GlobalTimer>();
        timer.name = "GlobalTimer";
    }

    private void Awake()
    {
        // すでに存在する場合は、新しい方を消す（重複防止）
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        RemainingSeconds = totalSeconds;
    }

    private void Update()
    {
        if (IsTimeUp || !IsRunning) return;
        if (IsPausedScene(SceneManager.GetActiveScene().name)) return;

        RemainingSeconds -= Time.deltaTime;

        if (RemainingSeconds <= 0f)
        {
            RemainingSeconds = 0f;
            OnTimeUp?.Invoke();

            // --- ここに追記 ---
            ResetTimer();
            SceneManager.LoadScene("stage_game_over");
            // ------------------
        }
    }

    private bool IsPausedScene(string sceneName)
    {
        foreach (string s in pausedScenes)
        {
            if (s == sceneName) return true;
        }
        return false;
    }

    /// <summary>残り時間を最初に戻して、カウントを再開する（新しくゲームを始めるとき・リトライのとき）</summary>
    public static void ResetTimer()
    {
        if (Instance != null)
        {
            Instance.RemainingSeconds = Instance.totalSeconds;
            Instance.IsRunning = true;
        }
    }

    /// <summary>カウントを止める（クリアしたときなど）。残り時間の値はそのまま残る</summary>
    public static void StopTimer()
    {
        if (Instance != null)
        {
            Instance.IsRunning = false;
        }
    }

    /// <summary>残り時間を戻して、今のシーンを最初からやり直す</summary>
    public static void RetryCurrentScene()
    {
        Time.timeScale = 1f;
        ResetTimer();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>秒数を「分:秒」（例 3:35）の文字列にする</summary>
    public static string FormatTime(float seconds)
    {
        int total = Mathf.CeilToInt(Mathf.Max(0f, seconds));
        return $"{total / 60}:{total % 60:00}";
    }


}
