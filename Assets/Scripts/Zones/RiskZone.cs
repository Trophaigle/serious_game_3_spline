using UnityEngine;

public abstract class RiskZone : MonoBehaviour, IRiskZone
{ //code commun, comportement commun à toute les zones à risque
     [Header("Zone Common")]
    [SerializeField] protected Renderer zoneRenderer;
    [SerializeField] protected int zoneNumber;

    protected bool triggered = false;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("CameraTrigger")) return;

        triggered = true;

        Debug.Log("[ZONE] Entrée caméra zone " + zoneNumber);

        OnZoneEnteredVisual();
        RiskGameManager.Instance.StartZone(this);
    }

    protected virtual void OnZoneEnteredVisual() //accessible par classes filles
    {
        if (zoneRenderer != null)
            zoneRenderer.material.color = Color.red;
    }

    // 🔹 Méthodes imposées aux zones concrètes
    public abstract void StartZone();
    public abstract void OnRiskIdentified(IRiskObject risk);
    public abstract void OnSolveRisk();
    public abstract void OnActionResolved();
    public abstract void TriggerRisk();

    protected void SecureZoneVisual()
    {
        if (zoneRenderer != null)
            zoneRenderer.material.color = Color.green;
    }
}
