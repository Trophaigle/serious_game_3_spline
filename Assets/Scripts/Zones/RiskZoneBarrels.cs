using System.Collections;
using UnityEngine;

public class RiskZoneBarrels : RiskZone
{
    public GameObject barrel;
    public MeshFilter meshFilter;
    public RiskDecisionSO riskDecisionSo;
    public RiskDecisionUI decisionUI;

    public override void StartZone()
    {
        Debug.Log("[ZONE BARRELS] Start zone");
        RiskGameManager.Instance.UI.ShowInstruction(riskDecisionSo.intruction);
    }

    public override void OnRiskIdentified(IRiskObject risk)
    {
            Debug.Log("[ZONE BARRELS] Bon objet identifié");
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
            RiskGameManager.Instance.UI.ShowInstruction("Wrong answer ! <br> This action does not adequately reduce the chemical risk. Chemical hazards must be secured to prevent spills and exposure.");
           AudioManager.Instance.Play(AudioType.QcmWrong);
        }
    }

    public override void OnSolveRisk()
    {
         StartCoroutine(
    RotateBarrelKeepingGroundContact(//marche pas trop
        barrel.GetComponent<Rigidbody>(),
        3f,
        LayerMask.GetMask("Ground"),
        3f
    )
);
    }

    public override void OnActionResolved()
    {
        // animation, son, etc. 
       
        SecureZoneVisual();
        //mettre sur panel
        RiskGameManager.Instance.UI.ShowInstruction(riskDecisionSo.securedText);

        Debug.Log("[ZONE BARRELS] Zone terminée"); 
    }

float GetCylinderRadius()
{
    Bounds bounds = meshFilter.sharedMesh.bounds;

    // cylindre couché → rayon = moitié de la hauteur locale
    return bounds.extents.y;
}

 private IEnumerator RotateBarrelKeepingGroundContact(Rigidbody rb,
    float duration,
    LayerMask groundLayer,
    float waitAfter = 1f)
{
    rb.isKinematic = true; 
    float radius = GetCylinderRadius();

    Quaternion startRot = rb.rotation;
    Quaternion endRot = Quaternion.identity;

    Vector3 startPos = rb.position;

    float elapsed = 0f;

    while (elapsed < duration)
    {
        elapsed += Time.deltaTime;
        float t = elapsed / duration;

        // 1️⃣ Appliquer la rotation
        Quaternion currentRot = Quaternion.Slerp(startRot, endRot, t);
        rb.MoveRotation(currentRot);

        // 2️⃣ Trouver le point bas du cylindre dans le monde
        Vector3 down = currentRot * Vector3.down;
        Vector3 bottomPoint = rb.position + down * radius;

        // 3️⃣ Raycast vers le sol
        if (Physics.Raycast(rb.position, Vector3.down, out RaycastHit hit, radius * 2f, groundLayer))
        {
            float offset = - 0.005f;
            float correction = hit.point.y - bottomPoint.y + offset;
            rb.MovePosition(rb.position + Vector3.up * correction);
        }

        yield return null;
    }

    rb.MoveRotation(endRot);

   // if (waitAfter > 0f)
     //  yield return new WaitForSeconds(waitAfter);

    rb.isKinematic = false;
     RiskGameManager.Instance.ActionResolved();
 } 

 public override void TriggerRisk()
    {
        barrel.GetComponent<Rigidbody>().isKinematic = false;
        AudioManager.Instance.Play(AudioType.BarrelFall);
    }
}
