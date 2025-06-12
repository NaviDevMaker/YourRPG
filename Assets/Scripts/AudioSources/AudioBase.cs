using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public abstract class AudioBase : MonoBehaviour
{

    [SerializeField] AudioSource audioSource;
    [SerializeField] List<AudioClip> audioClips;
    [SerializeField] List<string> sceneNames;//シーン遷移時にオウディオを変更するため

    public AudioSource AudioSource { get => audioSource; set => audioSource = value; }
    public  List<AudioClip> AudioClips { get => audioClips;}
    public List<string> SceneNames { get => sceneNames; set => sceneNames = value; }

    public abstract void OnSceneLoaded(Scene scene, LoadSceneMode mode);


}
