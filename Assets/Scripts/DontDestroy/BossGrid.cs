using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGrid : MonoBehaviour
{
    public static BossGrid Instance { get; private set; }
    // Start is called before the first frame update
    void Awake()//シングルトン
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

    //SceneObjectManagerで呼びだす関数、コルーチンにすることでDestroyOnLoadのアクティブ化の後に呼び出しアクティブ、非アクティブを設定
   public IEnumerator ExecuteDisable(bool gridSceneActive)
   {
        yield return new WaitForFixedUpdate();
        if (gridSceneActive == false) this.gameObject.SetActive(false);
        else if (gridSceneActive) this.gameObject.gameObject.SetActive(true);


   }

   
}
