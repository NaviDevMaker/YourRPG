using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StorePlayerItems : ScriptableObject
{
    [SerializeField] List<WeponBase> currentWepons;
    [SerializeField] List<ItemMoveBase> currentItems;

    public List<WeponBase> CurrentWepons { get => currentWepons; set => currentWepons = value; }
    public List<ItemMoveBase> CurrentItems { get => currentItems; set => currentItems = value; }
}
