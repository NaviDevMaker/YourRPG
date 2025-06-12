using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EncountArea : MonoBehaviour
{
    [SerializeField] AreaEnemybase enemy;
    //[SerializeField] List<Battler> rareEnemys;

    public Battler GetRandomBattler()
    {
        int metaruCreamEncount = 5;

        //int rareRatio = -1;
        int rareRatio = UnityEngine.Random.Range(0, 40);
        int r = UnityEngine.Random.Range(0, enemy.SimpleEnemys.Count);
        Debug.Log($"レア確立の数字は{rareRatio}");
        if (metaruCreamEncount == rareRatio)
        {
            if (enemy.RareEnemys.Count > 0) return enemy.RareEnemys[0];

            else Debug.Log("レアキャラはいない");

        }
        //else
        //{
            //int r = UnityEngine.Random.Range(0, enemy.SimpleEnemys.Count);
            return enemy.SimpleEnemys[r];
        //}
        
    }
}
