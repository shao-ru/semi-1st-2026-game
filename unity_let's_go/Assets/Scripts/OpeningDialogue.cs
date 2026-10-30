using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // シーン切り替えに必要
using TMPro;

public class OpeningDialogue : MonoBehaviour
{
    [Header("UI参照")]
    public TMP_Text dialogueText;
    public GameObject dialogueBox;

    [Header("カメラ演出用")]
    public Camera mainCamera;

    [Header("遷移設定")]
    [Tooltip("会話終了後に読み込むシーン名")]
    public string nextSceneName = "OutdoorScene"; // インスペクターから変更可能

    [Header("シナリオテキスト")]
    [TextArea(2, 5)]
    public List<string> sentences = new List<string>()
    {
        "「ピピピピ！ ピピピピ！……」",
        "……んあー、うるさい……あと5分……",
        "ふあぁ……今何時だ……？",
        "……え？",
        "10時15分！？ 嘘でしょ！？？？？",
        "10時30分からの講義、もう3回休んでるから今日休んだら即・落単決定じゃん！！！",
        "あと15分……今すぐ飛び出せばワンチャン間に合う……！！",
        "着替えなんて後だ！ とにかく大学へ走れ！！！！"
    };

    private int currentIndex = 0;
    private bool isTyping = false;
    private string currentSentence = "";

    void Start()
    {
        StartDialogue();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = currentSentence;
                isTyping = false;
            }
            else
            {
                DisplayNextSentence();
            }
        }
    }

    public void StartDialogue()
    {
        dialogueBox.SetActive(true);
        currentIndex = 0;
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (currentIndex < sentences.Count)
        {
            currentSentence = sentences[currentIndex];

            if (currentIndex == 4)
            {
                StartCoroutine(ShakeCamera());
            }

            StartCoroutine(TypeSentence(currentSentence));
            currentIndex++;
        }
        else
        {
            // インスペクターで設定したシーン名へ遷移
            if (!string.IsNullOrEmpty(nextSceneName))
            {
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.LogWarning("遷移先のシーン名が設定されていません。");
            }
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.03f);
        }

        isTyping = false;
    }

    IEnumerator ShakeCamera()
    {
        Vector3 originalPos = mainCamera.transform.localPosition;
        float duration = 0.3f;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-0.1f, 0.1f);
            float y = Random.Range(-0.1f, 0.1f);

            mainCamera.transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.localPosition = originalPos;
    }
}