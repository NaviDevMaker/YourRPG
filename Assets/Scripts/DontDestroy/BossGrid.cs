using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGrid : MonoBehaviour
{
    public static BossGrid Instance { get; private set; }
    // Start is called before the first frame update
    void Awake()//�V���O���g��
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

    //SceneObjectManager�ŌĂт����֐��A�R���[�`���ɂ��邱�Ƃ�DestroyOnLoad�̃A�N�e�B�u���̌�ɌĂяo���A�N�e�B�u�A��A�N�e�B�u��ݒ�
   public IEnumerator ExecuteDisable(bool gridSceneActive)
   {
        yield return new WaitForFixedUpdate();
        if (gridSceneActive == false) this.gameObject.SetActive(false);
        else if (gridSceneActive) this.gameObject.gameObject.SetActive(true);


   }

   
}
