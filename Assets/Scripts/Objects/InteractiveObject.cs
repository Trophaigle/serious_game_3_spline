using UnityEngine;
using UnityEngine.UI;

public class InteractiveObject : MonoBehaviour, IHoverable
{
    public Outline outline;

    protected virtual void Awake()
    {
        outline.enabled = false;
    }

    public virtual void OnHoverEnter()
    {
        outline.enabled = true;
    }

    public virtual void OnHoverExit()
    {
        outline.enabled = false;
    }
}
