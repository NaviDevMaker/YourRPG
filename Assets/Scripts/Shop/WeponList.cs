using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]

public class WeponList : ScriptableObject
{
    [SerializeField] List<WeponBase> townSellledWepons;
    [SerializeField] List<WeponBase> town1SelledWepons;

    public List<WeponBase> TownSelledWepons { get => townSellledWepons;}
    public List<WeponBase> Town1SelledWepons { get => town1SelledWepons;}
}
