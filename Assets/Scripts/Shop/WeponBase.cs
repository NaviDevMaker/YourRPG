using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeponBase:ScriptableObject
{
    [SerializeField] string weponName;
    [SerializeField] int weponAT;

    public string WeponName { get => weponName;}
    public int WeponAT { get => weponAT;}
}
