using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class RiskGameManager : MonoBehaviour
{
    public static RiskGameManager Instance;

    

    [SerializeField] private float introDuration = 7f;
    [SerializeField] private float fadeDuration = 2f;

    public CinemachineCamera cinemachineCamera;
    public RiskUIController UI;

    [Header("Camera Targets Order")]
    [SerializeField] private Transform[] cameraTargets;
    [Header("Zones Order")]
    [SerializeField] private RiskZone[] zones;

    
    private int currentTargetIndex = -1;

    private IRiskZone currentRiskZone;
    private GameState currentState;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartCoroutine(IntroSequence());
    }

    private IEnumerator IntroSequence()
    {
        SetState(GameState.Intro);

        // Wait for reading time
        yield return new WaitForSeconds(introDuration);

        // Fade out intro panel
        yield return StartCoroutine(UI.FadeOutIntroPanel(fadeDuration));

        // Start the game
        StartGame();
    }

    void StartGame()
    {
       NextTarget();
    }

    public void SetState(GameState state)
    {
        currentState = state;
        Debug.Log("[Game] GameState set to " + currentState);
    }

    public bool IsState(GameState state)
    {
        return currentState == state;
    }

    public void StartZone(IRiskZone riskZone)
    {
        cinemachineCamera.GetComponent<CinemachineCameraController>().PauseCamera();
        //currentRiskZone = riskZone;
        SetState(GameState.InZone);
        currentRiskZone.StartZone();
    }

    public void RiskIdentified(IRiskObject risk) //Called when risk as been identified
    {
        currentRiskZone.OnRiskIdentified(risk);
    }

     public void SolveRisk()
    {
        currentRiskZone.OnSolveRisk();
    }

    public void ActionResolved()
    {
        currentRiskZone.OnActionResolved();
        StartCoroutine(SimulateIntervention());
    }

    private IEnumerator SimulateIntervention()
    {
        yield return new WaitForSeconds(4f); // animation / son / effet
        NextTarget(); 
    }


    public void NextTarget()
    {
        currentTargetIndex++;

        if (currentTargetIndex >= cameraTargets.Length)
        {
            Debug.Log("[GAME] Fin des zones");
            UI.ShowEndPanel();
            return;
        }

        Debug.Log("[GAME] Nouvelle camera target : " + cameraTargets[currentTargetIndex].name);
        cinemachineCamera.LookAt = cameraTargets[currentTargetIndex];
        cinemachineCamera.GetComponent<CinemachineCameraController>().PlayCamera();
        SetState(GameState.Moving); 
        currentRiskZone = zones[currentTargetIndex];
        currentRiskZone.TriggerRisk();
    }
}
