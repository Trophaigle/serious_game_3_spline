using System.Collections;
using UnityEngine;

public class RiskZoneElectric : RiskZone
{
    public Transform shieldDangerous;
    public GameObject cupShield;
    public GameObject lever;
    public RiskDecisionSO riskDecisionSo;
    public RiskDecisionUI decisionUI;

    public override void StartZone()
    {
        Debug.Log("[ZONE ELEC] Start zone");
        RiskGameManager.Instance.UI.ShowInstruction(riskDecisionSo.intruction);
    }

    public override void OnRiskIdentified(IRiskObject risk)
    {
            Debug.Log("[ZONE ELEC] Bon objet identifié");
            RiskGameManager.Instance.UI.HideInstruction();
            //Que dois je faire ? (question à poser au joueur, appeler expert, redresser le bidon ?)
            decisionUI.Show(riskDecisionSo, OnDecisionSelected);
            
           
    }

    private void OnDecisionSelected(int selectedIndex)
    {
        bool correct = selectedIndex == riskDecisionSo.correctIndex;

         if (correct)
        {
            Debug.Log("Bonne décision !");
            RiskGameManager.Instance.UI.ShowInstruction("Good answer !");
            AudioManager.Instance.Play(AudioType.QcmRight);
        }
        else
        {
            Debug.Log("Mauvaise décision, feedback possible");
            RiskGameManager.Instance.UI.ShowInstruction("Wrong answer ! <br> This action does not ensure electrical safety. Electrical risks require power isolation and proper protection.");
         AudioManager.Instance.Play(AudioType.QcmWrong);
        }
    }

    public override void OnSolveRisk()
    {
        Transform cup = shieldDangerous.Find("Cup");

        if (cup == null)
        {
            Debug.LogError("Cup introuvable dans Shield Middle !");
            return;
        }

       StartCoroutine(
    MoveAndRotateKinematicWorld(
        cupShield.GetComponent<Rigidbody>(),
        cup.position,
        cup.rotation,
        30,
        3f
    )
);
    }

    public override void OnActionResolved()
    {
        
        SecureZoneVisual();
        //mettre sur panel
        RiskGameManager.Instance.UI.ShowInstruction(riskDecisionSo.securedText);

        Debug.Log("[ZONE ELEC] Zone terminée"); 
    }

  private IEnumerator MoveAndRotateKinematicWorld(
    Rigidbody rb,
    Vector3 targetWorldPosition,
    Quaternion targetWorldRotation,
    float leverTargetX,
    float duration
)
{
    rb.isKinematic = true;

    Transform t = rb.transform;

    Vector3 startPos = t.position;
    Quaternion startRot = t.rotation;

    //levier
    Vector3 leverStartEuler = lever.transform.localEulerAngles;
    float leverStartX = leverStartEuler.x;
    AudioManager.Instance.Play(AudioType.PowerOff);

    float elapsed = 0f;

    while (elapsed < duration)
    {
        elapsed += Time.deltaTime;
        float t01 = elapsed / duration;

        t.position = Vector3.Lerp(startPos, targetWorldPosition, t01);
        t.rotation = Quaternion.Slerp(startRot, targetWorldRotation, t01);

        // 🔁 Levier (rotation locale X → 30)
        float currentX = Mathf.LerpAngle(leverStartX, leverTargetX, t01);
        lever.transform.localRotation = Quaternion.Euler(
            currentX,
            leverStartEuler.y,
            leverStartEuler.z
        );
        yield return null;
    }

    t.position = targetWorldPosition;
    t.rotation = targetWorldRotation;

    lever.transform.localRotation = Quaternion.Euler(
        leverTargetX,
        leverStartEuler.y,
        leverStartEuler.z
    );

    RiskGameManager.Instance.ActionResolved();
    }

    public override void TriggerRisk()
    {
        cupShield.GetComponent<Rigidbody>().isKinematic = false;
        AudioManager.Instance.Play(AudioType.CupFall);
    }
}
