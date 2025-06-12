using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public abstract class AudioBase : MonoBehaviour
{

    [SerializeField] AudioSource audioSource;
    [SerializeField] List<AudioClip> audioClips;
    [SerializeField] List<string> sceneNames;//�V�[���J�ڎ��ɃI�E�f�B�I��ύX���邽��

    public AudioSource AudioSource { get => audioSource; set => audioSource = value; }
    public  List<AudioClip> AudioClips { get => audioClips;}
    public List<string> SceneNames { get => sceneNames; set => sceneNames = value; }

    public abstract void OnSceneLoaded(Scene scene, LoadSceneMode mode);


}
