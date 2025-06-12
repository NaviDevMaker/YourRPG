using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnAudio : MonoBehaviour
{
    //�h�ɔ��܂�������BGM�͕ς��Ȃ�����V���O���g��
    public static InnAudio Instance { get; private set; }
    [SerializeField] AudioSource innAudioSource;
    [SerializeField] AudioClip innAudioClip;
    [SerializeField] InnMurabitoBase innMurabitoBase;
    // Start is called before the first frame update

    private void Awake()
    {
        if (Instance = null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
;       }else if(Instance != null)
        {
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        innMurabitoBase.OnSelectedYes += PlayInnAudio;
    }
    void PlayInnAudio()
    {

        innAudioSource.PlayOneShot(innAudioClip);
    }

}
