using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogBase : MonoBehaviour
{
    
   

    //一文字づつメッセージを表示する
    [SerializeField] Text text;//戦闘時のテキスト
    [SerializeField] float letterPerSeconds;//一文字づつ入力される文字の時間の間隔

    bool isTyping = false;
    public Text Text { get => text; set => text = value; }
    public bool IsTyping { get => isTyping; set => isTyping = value; }

    public virtual IEnumerator TypeDialog(string line, bool auto = true, bool keyOperate = true)//引数がない時には自動的にtrueが入る
    {

        if (!keyOperate && text.text != "")
        {
            yield break;
        }

        if (text.text != "")
        {
            text.text = "";
        }
        

        foreach (char letter in line)//時間差でテキストを出力
        {
                text.text += letter;
                yield return new WaitForSeconds(letterPerSeconds);
        }                   

        if (auto)
        {
            yield return new WaitForSeconds(0.2f);
        }
        else //スペースキーが押されるまで次のテキストには進まない（次の処理までいかない)
        {
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            text.text = "";

        }


    }
}
