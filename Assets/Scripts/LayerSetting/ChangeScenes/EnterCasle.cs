using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnterCasle:MonoBehaviour
{
    //PlayerController player;
    [SerializeField] string dialogContent;
    [SerializeField] Image casleDialogImage;
    [SerializeField] Text dialogText;
    [SerializeField] float letterPerSecond;

    public string DialogContent { get => dialogContent;}
    public Image CasleDialogImage { get => casleDialogImage;}

    private void Start()
    {
       
        //player = PlayerController.Instance;
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if(collision.gameObject == player.gameObject)
    //    {
    //        StartCoroutine(TypeDialog(dialogContent));
    //    }
    //}

    public IEnumerator TypeDialog(string line, bool auto = true, bool keyOperate = true)
    {
        
        casleDialogImage.gameObject.SetActive(true);
        // ダイアログ表示処理を行う
        foreach (char letter in line)
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(letterPerSecond); // 適当な待機時間を設定
        }

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        casleDialogImage.gameObject.SetActive(false);
    }
    //public override IEnumerator TypeDialog(string line, bool auto = true, bool keyOperate = true)
    //{
    //    //DontDestroyOnLoad(this.gameObject);
    //    casleDialogImage.gameObject.SetActive(true);
    //    //player.Constraint = true;
    //    yield return base.TypeDialog(line, auto: false, keyOperate);
    //    casleDialogImage.gameObject.SetActive(false);
    //    //yield return player.OpenSceneBases[11].ChangeScene(player.OpenSceneBases[11]);
    //    //player.Constraint = false;
    //    //player.EnterIndex = 15;
    //    //Destroy(this.gameObject);
    //}

}
