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
        //�v���C���[�R���g���[���[�ŃX�^�[�g����Init�ŕK�v�Ȓl�͎�ɓ����Ă�̂ŏW�߂�K�v�͂Ȃ��A�܂�������init���Ăяo���Ă��܂���HP�ɏ����l��MaxHP���������邱�ƂƂȂ�A�ŏ��̃N���b�N���ɐ퓬��Ɍ�����HP���񕜂���Ă��܂�
        //player.Battler.Init();//���Ɏ����l�����邽�߂ɂ����̕ϐ��̒��g�ɒl������
        //AppearTexts[0].text = $"Lv:{player.Battler.Level}";
        //AppearTexts[1].text = $"HP:{player.Battler.HP} / {player.Battler.MaxHP}";
        //AppearTexts[2].text = $"AT:{player.Battler.AT}";
        //AppearTexts[3].text = $"Ex:{player.Battler.BoderExps[player.Battler.Level - 1] - player.Battler.HasExp}";
        
        Debug.Log(OnClick_M_Status);
    }

    public async void OnClicked_M()//�N���b�N���Ƀv���C���[�̃X�e�[�^�X���擾���A������e�L�X�g�ɔ��f����
    {
        Debug.Log($"����{player.Battler.HP}�ő�{player.Battler.MaxHP}");
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
