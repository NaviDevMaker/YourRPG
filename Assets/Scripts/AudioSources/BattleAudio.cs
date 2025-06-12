using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class BattleAudio : AudioBase
{
    public static BattleAudio Instance { get; private set; }
    [SerializeField] Image enemyPanel;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }else if(Instance != null)
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        AudioSource.loop = true;
        SceneManager.sceneLoaded += OnSceneLoaded;
        AudioSource.clip = AudioClips[0];
    }
    public void PlayBattleAudio()
    {
            MainAudio.Instance.AudioSource.Stop();
            AudioSource.Play();
        
    }

    public  void StopBattleAudio()
    {
        AudioSource.Stop();
        MainAudio.Instance.AudioSource.Play();
    }

    public override void OnSceneLoaded(Scene scene,LoadSceneMode mode)
    {
        foreach (var SceneName in SceneNames)
        {
            if (scene.name == SceneName)
            {
                int sceneIndex = SceneNames.IndexOf(SceneName);
                AudioSource.clip = AudioClips[sceneIndex];
                break;
            }
        }
        
    }
}
