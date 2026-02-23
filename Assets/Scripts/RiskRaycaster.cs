
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

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

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

                if(Input.GetMouseButtonDown(0))
                {
                   HandleClick(hit.collider.gameObject);

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
