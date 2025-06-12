using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GoldUI : BelongingUIBase
{
    

    string curentGold;

    public UnityAction OnClick_G;
    private void Start()
    {
        AppearText.text = $"{player.Battler.HaveGold.ToString()}G";
        curentGold = player.Battler.HaveGold.ToString();
        OnClick_G += base.OpenManage;
    }  

    public void UpdateGoldUI()
    {
        int haveGold = player.Battler.HaveGold;
        string GoldToString = $"{haveGold.ToString()}"; 
       
        if(GoldToString.Length >= curentGold.Length)
        {
            int rollFigure = GoldToString.Length - curentGold.Length;
            while(rollFigure > 0)
            {
                RectTransform currentTextTrans = AppearText.GetComponent<RectTransform>();
                currentTextTrans.sizeDelta = new Vector2(currentTextTrans.sizeDelta.x + 18.7f, currentTextTrans.sizeDelta.y);
                Vector2 newTrans = currentTextTrans.anchoredPosition;
                newTrans.x -= 3.0f;
                currentTextTrans.anchoredPosition = newTrans;
                rollFigure -= 1;
            }
            
        }
        AppearText.text = $"{GoldToString}G";
        curentGold = GoldToString;

    }



    

}


