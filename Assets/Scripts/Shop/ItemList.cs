using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class ItemList :ScriptableObject
{
    [SerializeField] List<ItemMove_Heal> townselledItems;
    [SerializeField] List<ItemMove_Heal> town1SelledItems;

    public List<ItemMove_Heal> TownSelledItems { get => townselledItems;}
    public List<ItemMove_Heal> Town1SelledItems { get => town1SelledItems;}
}
