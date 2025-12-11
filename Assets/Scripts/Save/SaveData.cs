using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData 
{
    public int enterIndex;
    public Vector3 playerPos;
    public string sceneName;
    public string playerName;
    public int maxHP;
    public int HP;
    public int defence;
    public int aT;
    public int magicPoint;
    public double exp;
    public List<Move> moves;
    public bool[] detectDefeat = new bool[]{};
    public List<WeponBase> currentWepons;
    public List<ItemMoveBase> currentItems;
    public int gold;
    public string currentWeponName;
}
