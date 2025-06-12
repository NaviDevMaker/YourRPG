using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TipImage : MonoBehaviour
{
    [SerializeField] Text text;
    // Start is called before the first frame update
   public  void InitTexts(string EnterText)//シーンを遷移する際にテキストを入れるためのテキストコンポーネントが必要なため
   {
        text = GetComponentInChildren<Text>();
        text.text += EnterText;//シーン遷移時の処理で入れられたテキストを入れる。
      
   }

    
}
