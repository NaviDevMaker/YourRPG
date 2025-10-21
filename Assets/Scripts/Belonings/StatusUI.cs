using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
public class StatusUI : BelongingUIBase
{
    public UnityAction OnClick_M_Status;

    private void Start()
    {
        //プレイヤーコントローラーでスタート時にInitで必要な値は手に入ってるので集める必要はない、またここでinitを呼び出してしまうとHPに初期値のMaxHPが代入されることとなり、最初のクリック時に戦闘後に減ったHPも回復されてしまう
        //player.Battler.Init();//下に示す値を入れるためにそれらの変数の中身に値を入れる
        //AppearTexts[0].text = $"Lv:{player.Battler.Level}";
        //AppearTexts[1].text = $"HP:{player.Battler.HP} / {player.Battler.MaxHP}";
        //AppearTexts[2].text = $"AT:{player.Battler.AT}";
        //AppearTexts[3].text = $"Ex:{player.Battler.BoderExps[player.Battler.Level - 1] - player.Battler.HasExp}";
        
        Debug.Log(OnClick_M_Status);
    }

    public async void OnClicked_M()//クリック時にプレイヤーのステータスを取得し、それをテキストに反映する
    {
        Debug.Log($"現在{player.Battler.HP}最大{player.Battler.MaxHP}");
        await UniTask.Delay(100);
        base.OpenManage();
        AppearTexts[0].text = player.Battler.Base.Name;
        AppearTexts[1].text = $"Lv.{player.Battler.Level}";
        AppearTexts[2].text = $"HP:{player.Battler.HP} / {player.Battler.MaxHP}";
        if(player.Battler.HP <= player.Battler.MaxHP / 8) AppearTexts[2].color = Color.red;
        else if(player.Battler.HP >= (player.Battler.MaxHP / 8) + 1) AppearTexts[2].color = Color.white;
        AppearTexts[3].text = $"AT:{player.Battler.AT}";
        AppearTexts[4].text = $"MP:{player.Battler.MagicPoint}";
        AppearTexts[5].text = $"Ex:{player.Battler.BoderExps[player.Battler.Level - 1] - player.Battler.HasExp}";
    }

    public void OnUsedItem_Heal(int renewedHP)
    {
        AppearTexts[2].text = $"HP:{renewedHP} / {player.Battler.MaxHP}";
        if (renewedHP >= (player.Battler.MaxHP / 8) + 1) AppearTexts[2].color = Color.white;
    }

    public void OnUsedItem_MP(int renewedMP)
    {
        AppearTexts[4].text = $"MP:{renewedMP} / {player.Battler.MaxMP}";
        if (renewedMP >= (player.Battler.MaxMP / 8) + 1) AppearTexts[4].color = Color.white;
    }
    
    public void OnSelectedWepon(int renewedAT)
    {
        AppearTexts[3].text = $"AT:{renewedAT}";
    }
      

}
