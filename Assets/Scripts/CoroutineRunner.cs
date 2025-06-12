using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineRunner : MonoBehaviour
{
   public  static CoroutineRunner Instance { get; private set; }

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

    public void RunCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}
