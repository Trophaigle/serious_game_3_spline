using UnityEngine;

public class RiskObject : InteractiveObject, IRiskObject
{
    //Objet interactif specialisé
    public override void OnHoverEnter()
    {
        base.OnHoverEnter();
    }

    public void OnCorrectlyIdentified()
    {
        outline.enabled = false;
        Debug.Log("[RISK] Risk correctly identified!");
    }

    public void OnWronglyIdentified()
    {
        Debug.Log("[RISK] Risk wrongly identified!");
    }
}
