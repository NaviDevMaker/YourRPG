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
        Debug.Log("看板の言葉");
        PlayerController.Instance.Constraint = true;
        signBoardimage.gameObject.SetActive(true); ;
        yield return base.TypeDialog(line, auto, keyOperate);
        PlayerController.Instance.Constraint = false;
        signBoardimage.gameObject.SetActive(false);
    }

   void OnCollisionEnter2D(Collision2D collision)
   {
        Debug.Log("看板");
        Debug.Log($"名前は{collision.gameObject.name}");
        if(collision.gameObject == PlayerController.Instance.gameObject && !IsTyping)//
        {
            IsTyping = true;
            Debug.Log("成功");
            
            StartCoroutine(TypeDialog(signBoardContent, auto: false));
           
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("イグジット");
        if (collision.gameObject == PlayerController.Instance.gameObject) IsTyping = false;
    }

}
