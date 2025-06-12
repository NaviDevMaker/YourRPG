using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SignBoardBase : DialogBase
{
    
    [SerializeField] Image signBoardimage;
    [SerializeField] string signBoardContent;

  

    private void Start()
    {
        
        //signBoardimage = GameObject.Find("SignDialog").GetComponent<Image>();
        if(signBoardimage != null) Debug.Log(signBoardimage);

        signBoardimage.gameObject.SetActive(false);
    }
    public override IEnumerator TypeDialog(string line, bool auto = true, bool keyOperate = true)
    {
        Debug.Log("�Ŕ̌��t");
        PlayerController.Instance.Constraint = true;
        signBoardimage.gameObject.SetActive(true); ;
        yield return base.TypeDialog(line, auto, keyOperate);
        PlayerController.Instance.Constraint = false;
        signBoardimage.gameObject.SetActive(false);
    }

   void OnCollisionEnter2D(Collision2D collision)
   {
        Debug.Log("�Ŕ�");
        Debug.Log($"���O��{collision.gameObject.name}");
        if(collision.gameObject == PlayerController.Instance.gameObject && !IsTyping)//
        {
            IsTyping = true;
            Debug.Log("����");
            
            StartCoroutine(TypeDialog(signBoardContent, auto: false));
           
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("�C�O�W�b�g");
        if (collision.gameObject == PlayerController.Instance.gameObject) IsTyping = false;
    }

}
