using UnityEngine;

[CreateAssetMenu(fileName = "RiskDecisionSO", menuName = "Scriptable Objects/RiskDecisionSO")]
public class RiskDecisionSO : ScriptableObject
{
    [TextArea(2, 5)]
    public string intruction;

    [TextArea(2, 5)]
    public string question;

    public string[] answers = new string[3]; // 3 réponses

    [Range(0, 2)]
    public int correctIndex;

    [TextArea(2, 5)]
    public string securedText;
}
