using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCanvas : MonoBehaviour
{
    public static ItemCanvas Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Instance != null)
        {
            Destroy(this.gameObject);
        }


    }



}
