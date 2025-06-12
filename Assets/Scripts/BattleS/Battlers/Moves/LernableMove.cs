using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//どのレベルでどの技を覚えるかを対応付ける
[System.Serializable]
public class LernableMove
{
    [SerializeField] MoveBase moveBase;//moveBaseの変数をキャラクターのScriptableObjectのインスペクターで設定するため
    [SerializeField] int level;

    public MoveBase MoveBase { get => moveBase;}
    public int Level { get => level;}
}
