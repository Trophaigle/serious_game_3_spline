using UnityEngine;
using UnityEngine.Playables;

public class TimelineDebug : MonoBehaviour
{
      public PlayableDirector director;

    void OnEnable()
    {
        director.played += OnPlayed;
        director.paused += OnPaused;
        director.stopped += OnStopped;
    }

    void OnDisable()
    {
        director.played -= OnPlayed;
        director.paused -= OnPaused;
        director.stopped -= OnStopped;
    }

    void OnPlayed(PlayableDirector d)
    {
        Debug.Log("[TIMELINE EVENT] PLAY");
    }

    void OnPaused(PlayableDirector d)
    {
        Debug.Log("[TIMELINE EVENT] PAUSE");
    }

    void OnStopped(PlayableDirector d)
    {
        Debug.Log("[TIMELINE EVENT] STOP");
    }
}
