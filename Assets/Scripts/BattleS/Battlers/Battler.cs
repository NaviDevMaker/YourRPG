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


[System.Serializable]//インスペクター上に出すため)、PlayerUnit,EnemyUnitで使うため
public class Battler //オブジェクトに直接アタッチしないためMonoBehavierの必要なし
{
    [SerializeField] BattlerBase _base;
    [SerializeField] BOSSBase _bossBase;
    [SerializeField] int level;
    [SerializeField] int MAX_Level;
    [SerializeField] int haveGold;
    //[SerializeField] List<>

    public AboutBuff _AboutBuff { get; private set; }


    public double HasExp { get; set; }

    
    public BattlerBase Base { get => _base; }//BattleBaseにほかのスクリプトがアクセスするため
    public int Level { get => level; }

    //ステータス
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
    //初期化
    public void Init(bool isEnemy = false)//setアクセサを使って値を設定
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
            else//ホルダーにそれぞれのレベルでの値を格納していく
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
                //Debug.Log($"{i}レベル：{ HPholder[i]}");

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
         HP = Mathf.Clamp(HP - damage, 0, MaxHP);//最大値、最小値の設定

        return damage;
    }

    public void HealHP(int moveHeal)
    {
        int heal = moveHeal;
        HP = Mathf.Clamp(HP + heal, 0, MaxHP);//最大値、最小値の設定
    }

    public void HealMP(int moveHeal)
    {
        int heal = moveHeal;
        MagicPoint = Mathf.Clamp(MagicPoint + heal, 0, MaxMP);//最大値、最小値の設定
    }

    public Move GetRandomMove()
    {
        int r = Random.Range(0, Moves.Count);
        return Moves[r];
    }

    public bool isLevelUp()//プレイヤーのレベルを上げる。
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

    public void GetGold(BattlerBase enemy)//ゴールドを入手
    {

        haveGold += enemy.DropGold;
        Debug.Log($"金は{ haveGold}");
  
    }

    public Move LearnedMove()
    {
        foreach (var learnableMove in Base.LernableMoves)
        {
            //まだ覚えていないもので覚えるワザがあれば登録する
            if(learnableMove.Level <= Level　&& !Moves.Exists(move =>move.Base == learnableMove.MoveBase)) // Movesの中のmoveのBaseとleanableBaseのMovebaseが一致しないなら追加する
            {
                Move move = new Move(learnableMove.MoveBase);
                Moves.Add(move);
                return move;//新しいワザを返り値として返す
            }
        }

        return null;//新しく覚えた技が何もなかったらnullを返す
    }
}
