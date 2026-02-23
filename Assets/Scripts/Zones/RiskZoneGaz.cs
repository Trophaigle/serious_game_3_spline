using System;
using System.Collections;
using UnityEngine;

public class RiskZoneGaz : RiskZone
{
    public ParticleSystem particleSystem;
    public RiskDecisionSO riskDecisionSo;
    public RiskDecisionUI decisionUI;
  

    public override void StartZone()
    {
        Debug.Log("[ZONE GAZ] Start zone");
        RiskGameManager.Instance.UI.ShowInstruction(riskDecisionSo.intruction);
    }

    public override void OnRiskIdentified(IRiskObject risk)
    {
            Debug.Log("[ZONE GAZ] Bon objet identifié");
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
            RiskGameManager.Instance.UI.ShowInstruction("Wrong answer ! <br> This action does not sufficiently address the gas hazard. Gas leaks must be controlled and the area properly ventilated.");
         AudioManager.Instance.Play(AudioType.QcmWrong);
        }
    }

    public override void OnSolveRisk()
    {
    
          StartCoroutine(
    StopParticlesGradually(
        4f
    )
);
        
    }

     public IEnumerator StopParticlesGradually(float duration)
    {
        var emission = particleSystem.emission;
        float startRate = emission.rateOverTime.constant;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            emission.rateOverTime = Mathf.Lerp(startRate, 0f, t);
            yield return null;
        }

        emission.rateOverTime = 0f;

        // Let remaining particles die naturally
        particleSystem.Stop();
        RiskGameManager.Instance.ActionResolved();
    }

    public override void OnActionResolved()
    {
        SecureZoneVisual();
        RiskGameManager.Instance.UI.ShowInstruction(riskDecisionSo.securedText);

        Debug.Log("[ZONE GAZ] Zone terminée"); 
    }
    public override void TriggerRisk()
    {
        AudioManager.Instance.Play(AudioType.GasLeak);
    }

}
