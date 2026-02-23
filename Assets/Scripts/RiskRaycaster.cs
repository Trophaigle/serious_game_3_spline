
using UnityEngine;

public class RiskRaycaster : MonoBehaviour
{
    public float radius = 0.3f;
    public float distance = 50f;

    IHoverable currentHover;

    public Camera mainCamera;

    void Awake()
    {
        if(mainCamera == null)
        {
            Debug.LogError("Main Camera not assigned in RiskRaycaster.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(RiskGameManager.Instance.IsState(GameState.InMenu)) return;

        Vector3 screenPos;

if(Application.isMobilePlatform)
{
    if(Input.touchCount > 0){
        screenPos = Input.GetTouch(0).position; // position du premier tap
        Debug.Log("Tap");
    }
    else{
        return; // pas de touch
    }
}
else
{
    screenPos = Input.mousePosition; // desktop
}

Ray ray = mainCamera.ScreenPointToRay(screenPos);

        if(Physics.SphereCast(ray, radius, out RaycastHit hit, distance))
        {
            IHoverable hoverable = hit.collider.GetComponent<IHoverable>();

            if(hoverable != null) 
            {
                if(currentHover != hoverable)
                {
                    currentHover?.OnHoverExit();
                    currentHover = hoverable;
                    currentHover.OnHoverEnter();
                }

                if (Application.isMobilePlatform)
{
    // Mobile : tap sur l'écran
    if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
    {
        Debug.Log("Tap2");
        HandleClick(hit.collider.gameObject);
    }
}
else
{
    // Desktop : clic souris
    if (Input.GetMouseButtonDown(0))
    {
        HandleClick(hit.collider.gameObject);
    }
}
                return;
            }
        }
       currentHover?.OnHoverExit();
       currentHover = null;
    }

    private void HandleClick(GameObject gameObject) //Handles if player clicked on risked object or not
    {
        IRiskObject risk = gameObject.GetComponent<IRiskObject>();
        if(risk != null) //if risked object clicked
        {
            RiskGameManager.Instance.RiskIdentified(risk);
            currentHover?.OnHoverExit();
        }
        else
        {
            Debug.Log("Clicked on non-risk object: " + gameObject.name);
            Debug.Log("Try again");
        }
    }
}
