using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialBossBattle 
{
    SiblingBossBattle siblingBossBatlle;
    LastBoss lastBoss;
    MadousiBoss madousiBoss;

    Dictionary<string, ISpecialBossHandler> events = new Dictionary<string, ISpecialBossHandler>();
       
    public SiblingBossBattle SiblingBossBatlle { get => siblingBossBatlle;}
    public LastBoss LastBoss { get => lastBoss; set => lastBoss = value; }

    public void InitBossInfo()
    {
        siblingBossBatlle = new SiblingBossBattle();
        lastBoss = new LastBoss();
        madousiBoss = new MadousiBoss();

        events.Add("CasleFloar2", siblingBossBatlle);
        events.Add("CasleFloar3", madousiBoss);
        events.Add("CasleFloar4", lastBoss);

    }



    public void ExecuteBossEvent(int bossIndex,SceneObjectManager sceneObjectManager, string activeSceneName)
    {
        if ((events.TryGetValue(activeSceneName, out ISpecialBossHandler specialBossHandler)))
        {
            // ç≈èâÇÃÉLÅ[ÇéÊìæ
            string LastBossKey = new List<string>(events.Keys)[2];

            if (activeSceneName == LastBossKey) lastBoss.DefeatCounts.Add(true);
            specialBossHandler.HandleDefeat(bossIndex, sceneObjectManager);
            
        }
    }

 
}
