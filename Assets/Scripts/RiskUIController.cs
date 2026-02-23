using System.Collections;
using TMPro;
using UnityEngine;

public class RiskUIController : MonoBehaviour
{
    [SerializeField] private GameObject introPanel;
    [SerializeField] private GameObject infoPanel;
     [SerializeField] private GameObject endPanel;
    [SerializeField] private TextMeshProUGUI instructionText;

      void Awake()
    {
        introPanel.SetActive(true);
        infoPanel.SetActive(false);
        endPanel.SetActive(false);
    }

    public void ShowInstruction(string message)
    {
        instructionText.text = message;
        infoPanel.SetActive(true);
    }

    public void HideInstruction()
    {
        infoPanel.SetActive(false);
    } 

    public void ShowEndPanel()
    {
        infoPanel.SetActive(false);
        endPanel.SetActive(true);
    }

    public IEnumerator FadeOutIntroPanel(float duration)
    {
        CanvasGroup canvasGroup = introPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = introPanel.AddComponent<CanvasGroup>();

        float startAlpha = canvasGroup.alpha;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, t / duration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        introPanel.SetActive(false);
    }

   
}
