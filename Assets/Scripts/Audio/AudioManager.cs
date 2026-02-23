using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
     public static AudioManager Instance;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioDataSO[] audioDataList;

    private Dictionary<AudioType, AudioDataSO> audioMap;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioMap = new Dictionary<AudioType, AudioDataSO>();

        foreach (var data in audioDataList)
        {
            if (!audioMap.ContainsKey(data.type))
                audioMap.Add(data.type, data);
            else
                Debug.LogWarning($"Duplicate audio type: {data.type}");
        }
    }

    public void Play(AudioType type)
    {
        if (!audioMap.TryGetValue(type, out var data))
        {
            Debug.LogWarning($"Audio not found: {type}");
            return;
        }

        audioSource.PlayOneShot(data.clip, data.volume);
    }
}
