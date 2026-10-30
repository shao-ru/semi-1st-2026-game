using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButtons : MonoBehaviour
{
    // 表示・非表示を切り替えたいコンフィグパネルのオブジェクト [2]
    [SerializeField] private GameObject configPanel;

 // 前編で作成したメインシーン（ゲーム本編）への遷移処理
    public void StartButton()
    {
        GlobalTimer.ResetTimer(); // 全体タイマーを最初に戻す
        SceneManager.LoadScene("stage_story"); // ゲーム本編の最初のステージ
    }

    public void TutorialButton()
    {
        GlobalTimer.ResetTimer(); // 全体タイマーを最初に戻す
        SceneManager.LoadScene("stage_in_home"); // stage_in_homeステージ
    }

    // 設定画面を表示するメソッド（動画内：超コンフィグパネル）[7]
    public void ShowConfigPanel()
    {
        configPanel.SetActive(true); // パネルを表示にする [7]
    }

    // 設定画面を非表示にするメソッド（動画内：ハイド）[7]
    public void HideConfigPanel()
    {
        configPanel.SetActive(false); // パネルを非表示にする [7]
    }
}