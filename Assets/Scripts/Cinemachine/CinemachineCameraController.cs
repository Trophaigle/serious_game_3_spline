using UnityEngine;
using UnityEngine.Playables;

public class CinemachineCameraController : MonoBehaviour
{
    [Header("References")]
    public PlayableDirector timeline;

    private bool isPaused = false;

    // Appelé quand la caméra doit s'arrêter
    public void PauseCamera()
    {
        if (timeline == null) return;

        timeline.Pause();
        isPaused = true;

        Debug.Log("[CAMERA] PauseCamera()");
    }

    // Appelé quand la caméra doit repartir
    public void PlayCamera()
    {
        if (timeline == null) return;

        timeline.Play();
        isPaused = false;

        Debug.Log("[CAMERA] PlayCamera()");
    }

    public bool IsPaused()
    {
        return isPaused;
    }
}
