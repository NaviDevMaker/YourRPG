using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainAudio : AudioBase
{
    public static MainAudio Instance { get; private set;}

    [SerializeField] AudioClip churchAudio;
    //[SerializeField] AudioSource mainAudioSource;
    //[SerializeField] List<AudioClip> MainAudios;//メインのオウディオを格納
    //[SerializeField] List<string> mainSceneNames;//シーン遷移時にオウディオを変更するため

    //public AudioSource MainAudioSource { get => mainAudioSource; }


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
    void Start()
    {

        AudioSource.loop = true;
        SceneManager.sceneLoaded += OnSceneLoaded;
        int mainSceneIndex = SceneNames.IndexOf("Ground");
        AudioSource.clip = AudioClips[mainSceneIndex];
        AudioSource.Play();
    }

    public void PlayerDeadAction()
    {
        AudioSource.Stop();
        AudioSource.clip = churchAudio;
        AudioSource.Play();
    }
    //シーン遷移時にオウディオを変更
    public override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        foreach (var mainSceneName in SceneNames)
        {
            if (scene.name == mainSceneName)//メインオウディオを流すシーンだったら
            {
                Debug.Log("おっけー");
                int mainSceneIndex = SceneNames.IndexOf(mainSceneName);
                AudioSource.clip = AudioClips[mainSceneIndex];
                AudioSource.Play();
                break;

            }
            else if (scene.name != mainSceneName)//それ以外なら止める
            {
                Debug.Log("あうと");
                AudioSource.Stop();
            }

        }

    }
   　
}
