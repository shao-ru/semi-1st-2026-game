using TMPro;
using UnityEngine;

/// <summary>
/// GlobalTimer の残り時間を表示する。
/// TextMeshPro のテキストに付けるだけで「3:35」のように表示され、
/// 時間切れになると「TIME UP」と表示される。
/// （UI のテキスト・ワールド空間のテキスト、どちらにも使える）
/// </summary>
[RequireComponent(typeof(TMP_Text))]
public class TimerDisplay : MonoBehaviour
{
    [Header("時間の前に付ける文字（例：「残り 」）。空でもよい")]
    [SerializeField] private string prefix = "";

    [Header("時間切れのときに表示する文字")]
    [SerializeField] private string timeUpText = "TIME UP";

    private TMP_Text label;

    private void Awake()
    {
        label = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        GlobalTimer timer = GlobalTimer.Instance;
        if (timer == null) return;

        label.text = timer.IsTimeUp
            ? timeUpText
            : prefix + GlobalTimer.FormatTime(timer.RemainingSeconds);
    }
}
