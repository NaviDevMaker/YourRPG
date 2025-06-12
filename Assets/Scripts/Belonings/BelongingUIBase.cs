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
   
    //�E�B���h�E�̊J��
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

    //�L�[���͂ɂ��āA���܂̂Ƃ���͎g���Ă��Ȃ�
    //public bool InputSpace(KeyCode key = KeyCode.Space, bool ableAction = true)// = 
    //{
    //    Debug.Log($"ableAction: {ableAction}, Space Key Down: {Input.GetKeyDown(key)}");
    //    return ableAction && Input.GetKeyDown(key);
    //}


}




