using UnityEngine;

public interface IRiskZone 
{//Ce que les zones à risque peuvent faire
    void StartZone();
    void OnRiskIdentified(IRiskObject risk);
    void OnSolveRisk();
    void OnActionResolved();
    void TriggerRisk();
}
