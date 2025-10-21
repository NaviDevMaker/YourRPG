using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainAudio : AudioBase
{
    public static MainAudio Instance { get; private set;}

    [SerializeField] AudioClip churchAudio;
    //[SerializeField] AudioSource mainAudioSource;
    //[SerializeField] List<AudioClip> MainAudios;//���C���̃I�E�f�B�I���i�[
    //[SerializeField] List<string> mainSceneNames;//�V�[���J�ڎ��ɃI�E�f�B�I��ύX���邽��

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
    //�V�[���J�ڎ��ɃI�E�f�B�I��ύX
    public override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        foreach (var mainSceneName in SceneNames)
        {
            if (scene.name == mainSceneName)//���C���I�E�f�B�I�𗬂��V�[����������
            {
                Debug.Log("�������[");
                int mainSceneIndex = SceneNames.IndexOf(mainSceneName);
                AudioSource.clip = AudioClips[mainSceneIndex];
                AudioSource.Play();
                break;

            }
            else if (scene.name != mainSceneName)//����ȊO�Ȃ�~�߂�
            {
                Debug.Log("������");
                AudioSource.Stop();
            }

        }

    }
   �@
}
