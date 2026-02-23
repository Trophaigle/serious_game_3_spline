using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RiskDecisionUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private Button[] answerButtons;

    private System.Action<int> onAnswerSelected;

    void Awake()
    {
        panel.SetActive(false);
    }

    public void Show(RiskDecisionSO decision, System.Action<int> callback)
    {
        RiskGameManager.Instance.SetState(GameState.InMenu);
        panel.SetActive(true);
        questionText.text = decision.question;
        onAnswerSelected = callback;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i;
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text =
                decision.answers[i];

            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() =>
{
    Debug.Log("BUTTON CLICKED index = " + index);
    SelectAnswer(index);
});
        }
    }

    private void SelectAnswer(int index)
    {
        Debug.Log("Button clicked");
        panel.SetActive(false);
        onAnswerSelected?.Invoke(index);
        RiskGameManager.Instance.SolveRisk();
    }
}
