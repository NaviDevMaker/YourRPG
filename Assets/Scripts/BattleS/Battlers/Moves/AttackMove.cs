using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AttackMove : ActionMoveBase
{

    [SerializeField] int power;
   

    public override string RunMoveResult(BattleUnit sourceUnit, BattleUnit targetUnit)
    {
        string getEnemyName = sourceUnit.Battler.Base.Name;
        string lucky = "ラッキークリーマー";

        Debug.Log(getEnemyName);


        if (lucky == getEnemyName)
        {
            return RunMoveOnRareMoster(sourceUnit,targetUnit);
        }
        getEnemyName = null;

        if (sourceUnit.Battler.Base.name == "Player")
        {
            Debug.Log(sourceUnit.Battler.Base.name);
            sourceUnit.Battler.MagicPoint -= MagicPoint;
        }

        int damage = targetUnit.Battler.TakeDamage(power, sourceUnit.Battler,targetUnit.Battler);
        if (damage == 0) return $"{targetUnit.Battler.Base.Name}はダメージを受けない!";
        return $"{sourceUnit.Battler.Base.Name}の{Name}\n{targetUnit.Battler.Base.Name}は{damage}のダメージ";
        
    }

    public string RunMoveOnRareMoster(BattleUnit sourceUnit, BattleUnit targetUnit)//レアモンスターとの戦闘時
    {
        

       
      return $"{sourceUnit.Battler.Base.Name}の{Name}!\n{targetUnit.Battler.Base.Name}はびくともしない！";
            
    }
}
    

