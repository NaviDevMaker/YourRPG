using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwoadAnimCanvas : MonoBehaviour
{
    public static SwoadAnimCanvas Instance { get; private set; }
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

