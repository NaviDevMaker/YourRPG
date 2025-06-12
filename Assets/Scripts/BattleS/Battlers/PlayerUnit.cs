using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUnit : BattleUnit
{
    public static PlayerUnit Instance { get; private set;}
    //UI�ɐݒ肷��e�p�����[�^�[
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] Text HPText;
    [SerializeField] Text atText;
    [SerializeField] Text ExText;
    [SerializeField] Text mPText;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }
    }
    public override void Setup(Battler battler)
    {
        base.Setup(battler);
        //�v���C���[�̃X�e�[�^�X�̐ݒ�
        nameText.text = battler.Base.Name;
        levelText.text =$"Level:{battler.Level}";
        HPText.text = $"HP:{battler.HP}/{battler.MaxHP}";
        atText.text = $"AT:{battler.AT}";
        ExText.text = $"Ex:{battler.BoderExps[battler.Level-1] - battler.HasExp}";
        mPText.text = $"MP:{battler.MagicPoint}/{battler.MaxMP}";
    }

    public override void UpdateUI()//�N���X���p�����Ă��邩��Battler���g����
    {

        levelText.text = $"Level:{Battler.Level}";
        HPText.text = $"HP:{Battler.HP}/{Battler.MaxHP}";
        atText.text = $"AT:{Battler.AT}";
        ExText.text = $"Ex:{Battler.BoderExps[Battler.Level - 1] - Battler.HasExp}";
        mPText.text = $"MP:{Battler.MagicPoint}/{Battler.MaxMP}";
    }

    public override void UpdateStatus()//�N���X���p�����Ă��邩��Battler���g����AUI�̃A�b�v�f�[�g�O�ɂ�����Ăяo��
    {
        Battler.MaxHP = Battler.HPholder[Battler.Level - 1];
        Battler.HP = Battler.HPholder[Battler.Level - 1];
        Battler.Defence = Battler.Defenceholder[Battler.Level - 1];
        Battler.AT = Battler.AttackHolder[Battler.Level - 1];
        Battler.MagicPoint = Battler.MagicPointHolder[Battler.Level - 1];
    }

    public override void ChangeHPColor()
    {
        if (Battler.HP <= Battler.MaxHP / 8)
        {
            HPText.color = Color.red;
        }
        else
        {
            HPText.color = Color.white;
        }

       
    }
}
