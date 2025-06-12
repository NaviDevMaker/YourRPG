using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AreaEnemybase : ScriptableObject
{
    [SerializeField] List<Battler> simpleEnemys;
    [SerializeField] List<Battler> rareEnemys;

    public List<Battler> SimpleEnemys { get => simpleEnemys;}
    public List<Battler> RareEnemys { get => rareEnemys;}
}
