using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEncount : MonoBehaviour
{
    public static BossEncount Instance { get; private set;}

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
}
