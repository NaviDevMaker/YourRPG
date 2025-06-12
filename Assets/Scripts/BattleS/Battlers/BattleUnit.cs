using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    //Uiの管理
    //Battlerの管理
   public Battler Battler { get; set; }

    public virtual void Setup(Battler battler)
    {
        Battler = battler;//引数のバトラーを設定（キャラクターの設定）
        //UIの初期化
        //Enemy:画像と名前の設定
        //Player:ステータスの設定
    }


    public virtual void UpdateUI()
    {

    }

    public virtual void UpdateStatus()
    {

    }

    public virtual void ChangeHPColor()
    {

    }
    
}
