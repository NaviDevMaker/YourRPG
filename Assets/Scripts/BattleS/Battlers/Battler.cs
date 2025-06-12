using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AboutBuff
{
    public bool IsDebuffed_Attack { get; set; } = false;
    public bool IsBuffed_Attack { get; set; } = false;
    public bool IsDebuffed_Defence { get; set; } = false;
    public bool IsBuffed_Defence { get; set; } = false;

    public int OriginalAT { get; set; } 

    public int OriginalDefence { get; set;}

    public int DebuffedDulation_AT { get; set; } = 0;

    public int BuffedDulation_AT { get; set; } = 0;

    public int DebuffedDulation_DF { get; set; } = 0;

    public int BuffedDulation_DF { get; set; } = 0;


    public AboutBuff(int originalAT = 0,int originalDefence = 0)
    {
        this.OriginalAT = originalAT;
        this.OriginalDefence = originalDefence;
    }
    
}


[System.Serializable]//�C���X�y�N�^�[��ɏo������)�APlayerUnit,EnemyUnit�Ŏg������
public class Battler //�I�u�W�F�N�g�ɒ��ڃA�^�b�`���Ȃ�����MonoBehavier�̕K�v�Ȃ�
{
    [SerializeField] BattlerBase _base;
    [SerializeField] BOSSBase _bossBase;
    [SerializeField] int level;
    [SerializeField] int MAX_Level;
    [SerializeField] int haveGold;
    //[SerializeField] List<>

    public AboutBuff _AboutBuff { get; private set; }


    public double HasExp { get; set; }

    
    public BattlerBase Base { get => _base; }//BattleBase�ɂق��̃X�N���v�g���A�N�Z�X���邽��
    public int Level { get => level; }

    //�X�e�[�^�X
    public int MaxHP { get; set; }
    public int HP { get; set; }
    public int AT { get; set; }

    public int Defence { get; set; }

    public int MaxMP { get; set; }
    public int MagicPoint { get; set; }




    public List<Move> Moves {get;set;}

    public List<double>BoderExps { get; set;}
    public List<int> HPholder { get; set; }
    public List<int> Defenceholder { get; set; }

    public List<int> AttackHolder { get; set; }

    public List<int> MagicPointHolder { get; set; }
    public BOSSBase BossBase { get => _bossBase;}
    public int HaveGold { get => haveGold; set => haveGold = value; }



    public void SetOriginalSta() 
    {
        _AboutBuff.OriginalAT = AttackHolder[level - 1];
        _AboutBuff.OriginalDefence =Defenceholder[level - 1];//new AboutBuff(battler.AttackHolder[level - 1], battler.Defenceholder[level - 1]);
   
    }
    //������
    public void Init(bool isEnemy = false)//set�A�N�Z�T���g���Ēl��ݒ�
    {
        

        Moves = new List<Move>();
        foreach (var  lernableMove in Base.LernableMoves)
        {
            if(lernableMove.Level <= Level)
            {
                Moves.Add(new Move(lernableMove.MoveBase));
            }   
        }


        BoderExps = new List<double>();
        HPholder = new List<int>();
        Defenceholder = new List<int>();
        AttackHolder = new List<int>();
        MagicPointHolder = new List<int>();

        for(int i = 0;i < MAX_Level;i++)
        {
            if (i <= 0)
            {
                HPholder.Add(_base.MaxHP);
                Defenceholder.Add(_base.Defence);
                AttackHolder.Add(_base.AT);
                BoderExps.Add(10);
                MagicPointHolder.Add(_base.MagicPoint);
            }
            else//�z���_�[�ɂ��ꂼ��̃��x���ł̒l���i�[���Ă���
            {
                int hPAtThisLevel = (int)(HPholder[i - 1] + 6);
                int defenceAtThisLevel = Defenceholder[i - 1] + 2;
                int attackAtThisLevel = AttackHolder[i - 1] + 2;
                int boderExp = (int)(BoderExps[i - 1] * 1.2);
                int magicPoint = MagicPointHolder[i - 1] + 5;

                BoderExps.Add(boderExp);
                HPholder.Add(hPAtThisLevel);
                Defenceholder.Add(defenceAtThisLevel);
                AttackHolder.Add(attackAtThisLevel);
                MagicPointHolder.Add(magicPoint);
                //Debug.Log($"{i}���x���F{ HPholder[i]}");

            }

           
            
        }
        Debug.Log(Moves.Count);

        MaxHP = _base.MaxHP;
        MaxMP = _base.MagicPoint;
        HP = MaxHP;
        AT = _base.AT;
        Defence = _base.Defence;
        MagicPoint = _base.MagicPoint;

        if (isEnemy) _AboutBuff = new AboutBuff(this.AT, this.Defence);
        else _AboutBuff = new AboutBuff();


    }

    public int TakeDamage(int movePower,Battler attacker,Battler attacked)
    {
        int damage = (attacker.AT + movePower) - Mathf.FloorToInt(attacked.Defence * 1.2f) ;
        if (damage <= 0) damage = 0;
         HP = Mathf.Clamp(HP - damage, 0, MaxHP);//�ő�l�A�ŏ��l�̐ݒ�

        return damage;
    }

    public void HealHP(int moveHeal)
    {
        int heal = moveHeal;
        HP = Mathf.Clamp(HP + heal, 0, MaxHP);//�ő�l�A�ŏ��l�̐ݒ�
    }

    public void HealMP(int moveHeal)
    {
        int heal = moveHeal;
        MagicPoint = Mathf.Clamp(MagicPoint + heal, 0, MaxMP);//�ő�l�A�ŏ��l�̐ݒ�
    }

    public Move GetRandomMove()
    {
        int r = Random.Range(0, Moves.Count);
        return Moves[r];
    }

    public bool isLevelUp()//�v���C���[�̃��x�����グ��B
    {
        while(HasExp >= BoderExps[level- 1])
        {
            HasExp -= BoderExps[level - 1];
            level++;
            Debug.Log(HasExp);
            return true;
           
        }

        return false;
    } 

    public void GetGold(BattlerBase enemy)//�S�[���h�����
    {

        haveGold += enemy.DropGold;
        Debug.Log($"����{ haveGold}");
  
    }

    public Move LearnedMove()
    {
        foreach (var learnableMove in Base.LernableMoves)
        {
            //�܂��o���Ă��Ȃ����̂Ŋo���郏�U������Γo�^����
            if(learnableMove.Level <= Level�@&& !Moves.Exists(move =>move.Base == learnableMove.MoveBase)) // Moves�̒���move��Base��leanableBase��Movebase����v���Ȃ��Ȃ�ǉ�����
            {
                Move move = new Move(learnableMove.MoveBase);
                Moves.Add(move);
                return move;//�V�������U��Ԃ�l�Ƃ��ĕԂ�
            }
        }

        return null;//�V�����o�����Z�������Ȃ�������null��Ԃ�
    }
}
