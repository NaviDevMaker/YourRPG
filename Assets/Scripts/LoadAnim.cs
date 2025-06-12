using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LoadAnim : MonoBehaviour
{
    public static LoadAnim Instance { get; private set; }
   
    // Start is called before the first frame update
    private void Awake()
    {

        this.gameObject.SetActive(false);
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
