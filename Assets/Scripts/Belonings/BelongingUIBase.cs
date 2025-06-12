using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class BelongingUIBase : MonoBehaviour
{
    [SerializeField] protected PlayerController player;
    [SerializeField] protected Text AppearText;
    [SerializeField] protected List<Text> AppearTexts;

   protected bool opend = false;
   
    //ウィンドウの開閉
    public void OpenManage()
    {
        if (!opend)
        {
            opend = true;
            //player.Constraint = true;
            this.gameObject.SetActive(true);
           
        }
        else if (opend)
        {
            opend = false;
            player.Constraint = false;
            this.gameObject.SetActive(false);
           
        }

    }

    //キー入力について、いまのところは使っていない
    //public bool InputSpace(KeyCode key = KeyCode.Space, bool ableAction = true)// = 
    //{
    //    Debug.Log($"ableAction: {ableAction}, Space Key Down: {Input.GetKeyDown(key)}");
    //    return ableAction && Input.GetKeyDown(key);
    //}


}




